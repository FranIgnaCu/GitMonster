using BattleMonster.Scripts.Squads.Troops;
using Godot;

namespace BattleMonster.Scripts.Squads;

public partial class Squad : Node3D
{
	Troop _selectedTroop;
	MovementPreview _troopPreview;

	public override void _Ready()
	{
		base._Ready();
		_selectedTroop = null;

		_troopPreview = null;
	}

	public void Selected(Troop aTroop)
	{
		if (_selectedTroop != null) {
			_selectedTroop.UnSelect();
		}
		_selectedTroop = aTroop;

		_troopPreview = _selectedTroop.GetMovementPreview();
		_troopPreview.Show();
	}
	public void UnSelected(Troop aTroop)
	{
		if(_selectedTroop == aTroop)
		{
			_selectedTroop = null;
			_troopPreview.Hide();
		}
	}


	public void MoveTroopTo(Vector3 newPosition) {
		if (_selectedTroop != null)
		{
			_selectedTroop.MoveTo(newPosition);
		}
	}


	public void MoveMovementPreview(Vector3 desiredPosition)
	{
		_troopPreview.MoveTo(desiredPosition);
	}

	internal bool AnyTroopSelected()
	{
		return _selectedTroop != null;
	}
}