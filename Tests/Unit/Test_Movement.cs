using GdUnit4;
using static GdUnit4.Assertions;

namespace BattleMonster.Tests.Unit
{
	
	[TestSuite]
	public class Movement_Tests
	{
		[TestCase]
		public void StringToUpper()
		{
			AssertString("AbcD".ToUpper()).IsEqual("ABCD");
		}
		
	}
}
