using BattleMonster.Scripts.Squads;
using BattleMonster.Scripts.Squads.Troops;
using GD_NET_ScOUT;
using Godot;
using Godot.Collections;


namespace BattleMonster.Tests.Scripts;

[Test]
public partial class TroopTest : Troop
{
    private Vector3 _initialPosition;
    private Vector3 _initialPositionNonTested;
    private Squad _testSquad;
    private Troop _nonTestedTroop;
    private Node _zones;

    public Vector3 GetZonePosition(string zoneName)
    {
        return _zones.GetNode<Area3D>(zoneName).GlobalPosition;
    }
    private void AssertTroopMovesTo(Vector3 position)
    {
        Assert.IsTrue(IsSelected());
        _testSquad.MoveTroopTo(position);
        //this.GetTestRunner().WaitForSignal(GetNode<Area3D>("../../Zones/"+direction), Area3D.SignalName.BodyEntered,() => Assert.AreEqual(GlobalPosition, _initialPosition), 2);
        this.GetTestRunner()
            .WaitSeconds(timeSeconds: .2, () => Assert.IsTrue(GlobalPosition.DistanceTo(position)<ArrivalThreshold));
    }
    
    private void AssertTroopDoesNotMoveTo(Vector3 position)
    {
        _testSquad.MoveTroopTo(position);
        this.GetTestRunner().WaitSeconds(timeSeconds:.1, () => Assert.AreEqual(GlobalPosition, _initialPosition));
    }

    private void AssertTroopMovesToZone(string zoneName)
    {
        AssertTroopMovesTo(GetZonePosition(zoneName));
    }
    
    [BeforeAll]
    public void Setup()
    {
        _initialPosition = GlobalPosition;
        _testSquad = (Squad)GetParentNode3D();
        _nonTestedTroop = _testSquad.GetNode<Troop>("NonTestedTroop");
        _initialPositionNonTested = _nonTestedTroop.GlobalPosition;
        
        _zones = GetNode<Node>("%Zones");

    }
    
    [AfterEach]
    public void Clean()
    {
        if (IsSelected())
        {
            _testSquad.MoveTroopTo(_initialPosition);
            Clicked();
        }
        GlobalPosition = _initialPosition;
        
    }
    

    [Test]
    public void TroopStartsUnSelected()
    {
        Assert.IsFalse(IsSelected());
    }

    [Test]
    public void SelectedTroopBecomesSelectedAfterClicked()
    {
        Assert.IsFalse(IsSelected());
        Clicked();
        Assert.IsTrue(IsSelected());
    }
    
    [Test]
    public void TroopDoesNotMoveIfUnselected()
    {
        Assert.IsFalse(IsSelected());
        AssertTroopDoesNotMoveTo(GetZonePosition("South"));
    }

    [Test]
    public void TroopDoesMovesNorth()
    {
        string direction = "North";
        Clicked();
        AssertTroopMovesToZone(direction);
    }

    [Test]
    public void TroopDoesMovesSouth()
    {
        string direction = "South";
        Clicked();
        AssertTroopMovesToZone(direction);
    }

    [Test]
    public void TroopDoesMovesWest()
    {
        string direction = "West";
        Clicked();
        AssertTroopMovesToZone(direction);
    }

    [Test]
    public void TroopDoesMovesEast()
    {
        string direction = "East";
        Clicked();
        AssertTroopMovesToZone(direction);
    }

    [Test]
    public void OnlySelectedTroopMoves()
    {
        Clicked();
        AssertTroopMovesToZone("South");
        Assert.IsFalse(_nonTestedTroop.IsSelected());
        Assert.AreEqual(_nonTestedTroop.GlobalPosition, _initialPositionNonTested);
    }

    [Test]
    public void TroopCanNotMoveToOcuppiedSpace()
    {
        Clicked();
        AssertTroopDoesNotMoveTo(_initialPositionNonTested);
    }

    [Test]
    public void PreviewDoesNotOcuppieSpace()
    {
        Clicked();
        _testSquad.MoveMovementPreview(GetZonePosition("South"));
        AssertTroopMovesTo(GetZonePosition("South"));
    }
    
    
    
    
    /*
    [Test]
    public void TroopCanMoveBehindOtherTroops()
    {
        Clicked();
        AssertTroopMovesTo(GetZonePosition("BehindOtherTroop"));
    }
    */
}