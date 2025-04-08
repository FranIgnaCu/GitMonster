using GdUnit4;

namespace BattleMonster.Tests.Unit;

using static GdUnit4.Assertions;

[TestSuite]
public class GdUnitExampleTest
{
   [TestCase]
   public void StringToLower() {
	  AssertString("AbcD".ToLower()).IsEqual("abcd");
   }
}
