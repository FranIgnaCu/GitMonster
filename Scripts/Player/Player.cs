using System;
using Godot;

namespace BattleMonster.Scripts.Player;

public interface IClickable
{
    void Clicked();
}

public partial class Player : Node3D
{
    private const float _rayCastLenght = 1000;
    RayCast3D _rayCast;
    Camera3D _camera;
    Squads.Squad _squad;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        _rayCast = (RayCast3D)GetNode("Camera3D/RayCast");

        _camera = GetViewport().GetCamera3D();

        _squad = (Squads.Squad)GetParent().GetNode("Squad");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (_squad.AnyTroopSelected())
        {
            UpdateMovementPreview();
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        // Check for left mouse button click
        switch (@event)
        {
            case InputEventMouseButton mouse:
                if (!mouse.Pressed) return;
                
                switch (mouse.ButtonIndex)
                {
                    case (MouseButton.Left):
                        try
                        {
                            CastRayFrom(mouse.Position);

                            if (_rayCast.IsColliding())
                            {
                                var collider = _rayCast.GetCollider();

                                if (collider is IClickable)
                                {
                                    ((IClickable)collider).Clicked();
                                }
                                else
                                {
                                    Vector3 collisionPosition = _rayCast.GetCollisionPoint();
                                    MoveCharacterTo(collisionPosition);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GD.PrintErr("Error al procesar el raycast: ", ex.Message);
                        }
                        break;
                }
                break;
        }
        
    }


    private void CastRayFrom(Vector2 positionOnScreen)
    {
        Vector3 from = _camera.ProjectRayOrigin(positionOnScreen);
        Vector3 to = from + _camera.ProjectLocalRayNormal(positionOnScreen) * _rayCastLenght;


        _rayCast.TargetPosition = to;
        _rayCast.ForceRaycastUpdate();
    }

    private Vector3 CenterInTile(Vector3 positionClickedByMouse)
    {

        float centeredX = CenterFloat(positionClickedByMouse.X);
        float centeredZ = CenterFloat(positionClickedByMouse.Z);

        return new Vector3(centeredX, positionClickedByMouse.Y, centeredZ);
    }
    private float CenterFloat(float aNumber)
    {
        aNumber = (float)Math.Floor(aNumber);

        if (aNumber % 2 != 0)
        {
            aNumber++;
        }

        return aNumber;
    }

    private void MoveCharacterTo(Vector3 clickedPosition)
    {
        Vector3 newPosition = CenterInTile(clickedPosition);

        _squad.MoveTroopTo(newPosition);
    }


    private void UpdateMovementPreview()
    {
        CastRayFrom(GetViewport().GetMousePosition());

        if (_rayCast.IsColliding())
        {
            Vector3 collisionPosition = _rayCast.GetCollisionPoint();
            Vector3 newPosition = CenterInTile(collisionPosition);

            _squad.MoveMovementPreview(newPosition);
        }
    }

    
}