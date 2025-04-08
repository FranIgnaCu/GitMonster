using BattleMonster.Scripts.Player;
using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

namespace BattleMonster.Tests.Unit
{

	[TestSuite]
	public class Position_Test
	{

	
		Player _player;
		private void assert_PositionCentersTo(Vector3 clickedPosition, Vector3 centeredPosition)
		{
			//var actualPosition = _player.centerInTile(clickedPosition);

			//AssertBool(actualPosition == centeredPosition).IsTrue(); 
		}

		[Before]
		public void setup()
		{
			_player = new Player();
		}
	
		[TestCase]
		public void StringToUpper()
		{
			AssertString("AbcD".ToUpper()).IsEqual("ABCD");
		}

	}
}
