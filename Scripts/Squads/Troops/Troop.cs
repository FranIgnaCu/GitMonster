using BattleMonster.Scripts.Player;
using Godot;

namespace BattleMonster.Scripts.Squads.Troops;

public partial class Troop : CharacterBody3D, ISelectable, IClickable
{
    
    [Export]
    public int FallAcceleration {get; set;} = 75;

    [Export]
    public float Speed = 13.0f;

    [Export]
    public float ArrivalThreshold = 0.15f;

    private MovementPreview _preview;

    private Vector3 _targetVelocity = Vector3.Zero;
    private Selector _selector;
    private Squad _squad;
    private Vector3 _targetPosition;
    
    public override void _Ready()
    {
        base._Ready();
        _selector = GetNode<Selector>("Pivot/selector");
        _selector.ForObject(this);
        _preview = GetNode<MovementPreview>("BaseTroopPreview");
        _squad = (Squad)GetParentNode3D();

        _targetPosition = GlobalPosition;

        ((Node)_preview).SetOwner(this);
    }
    public override void _PhysicsProcess(double delta){
        var direction = Vector3.Zero;

        if (!IsOnFloor())
        {
            _targetVelocity.Y -= FallAcceleration * (float)delta;
        }
        else
        {

            if (GlobalPosition.DistanceTo(_targetPosition) > ArrivalThreshold)
            {
                direction = (_targetPosition - GlobalPosition).Normalized();
            }
            else
            {
                direction = Vector3.Zero;
                GlobalPosition = _targetPosition;
            }
        }

        _targetVelocity.X = direction.X * Speed;
        _targetVelocity.Z = direction.Z * Speed;
        
        Velocity = _targetVelocity;
        MoveAndSlide();

    }

    private void OnMouseEntered(){
        _selector.MouseEnteredObject();
    }

    private void OnMouseExited(){
        _selector.MouseExitedObject();
    }

    public void Clicked(){
        _selector.Clicked();
    }


    public void Select() { 
        _selector.Select();
    }
    public void UnSelect()
    {
        _selector.UnSelect();
    }

    public bool IsSelected()
    {
        return _selector.IsSelected(); 
    }

    public void InformSelected()
    {
        _squad.Selected(this);
    }
    public void InformUnSelected()
    {
        _squad.UnSelected(this);
    }


    internal void MoveTo(Vector3 newPosition)
    {
        newPosition.Y = Hight();
        if (!IsPositionOccupied(newPosition))
        {
            _targetPosition = newPosition;
        }
        else
        {
            GD.Print("Invalid Position, already Ocupied");
        }

    }

    private bool IsPositionOccupied(Vector3 posibleTargetPosition)
    {
        var collisionInfo = MoveAndCollide(posibleTargetPosition,true);
        return collisionInfo != null;
    }


    internal MovementPreview GetMovementPreview()
    {
        return _preview;
    }

    internal static float Hight()
    {
        return 1;
    }
}