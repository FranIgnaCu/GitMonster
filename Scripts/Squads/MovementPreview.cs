using BattleMonster.Scripts.Squads.Troops;
using Godot;

namespace BattleMonster.Scripts.Squads;

public partial class MovementPreview : CharacterBody3D
{

	Troop _troopPreviewing;

	public Mesh Mesh()
	{
		MeshInstance3D meshInstance = GetNode<MeshInstance3D>("Mesh");

		return meshInstance.Mesh; 
	}

	internal void MoveTo(Vector3 desiredPosition)
	{
		desiredPosition.Y = Troop.Hight();
		GlobalPosition = desiredPosition;
		
	}

	internal void SetOwner(Troop troop)
	{
		if (_troopPreviewing != null) {
			GD.PrintErr("Falta Definir Owner en el selector");
			GetTree().Quit();
			
		}
		_troopPreviewing = troop;
	}
}