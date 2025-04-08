using GdUnit4.Api;

namespace BattleMonster.gdunit4_testadapter;

public partial class TestAdapterRunner : TestRunner
{
    public override void _Ready()
        => _ = RunTests();
}
