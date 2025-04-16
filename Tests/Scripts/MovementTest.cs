using Godot;
using System;

public partial class MovementTest : Node3D
{
    private const int MaxSimulationSpeed = 10_000;
    public override void _Ready()
    {
        base._Ready();
        Engine.MaxPhysicsStepsPerFrame = 1;
        Engine.PhysicsTicksPerSecond = MaxSimulationSpeed * 60;
        Engine.TimeScale = 3;
    }
}
