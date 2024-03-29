﻿using HunterXSavageness.Game.Behaviors;
using HunterXSavageness.Game.Behaviors.BehaviorScripts;
using HunterXSavageness.Game.Entities.Abstractions;
using HunterXSavageness.Game.Helpers;
using SFML.Graphics;
using SFML.System;

namespace HunterXSavageness.Game.Entities;

public class Wolf : NpcBase
{
    public override Shape GameObject { get; }
    
    public override NpcType Type => NpcType.Wolf;

    public override float WanderingSpeed => 0.9f;

    public override float RunningSpeed => 1.3f;
    
    public override Vector2f Velocity { get; set; }
    
    public override bool IsDead { get; set; }
    
    public override float ActivationRadius { get; } = GameSettings.GetDiagonal() * GameSettings.GetDiagonal();
    
    protected override Region SpawnArea { get; }

    protected override FlockBehaviorBase Behavior { get; } = new CompositeBehavior(
        new FlockBehaviorBase[] { new SearchVictimBehavior() },
        new[] { 1f });

    private float _healthAmount = 100f;

    public Wolf(Region spawnArea)
    {
        SpawnArea = spawnArea;

        float spawnX = GameLoop.RandomGenerator.NextSingle() * (spawnArea.EndPoint.X - spawnArea.StartPoint.X);
        float spawnY = GameLoop.RandomGenerator.NextSingle() * (spawnArea.EndPoint.Y - spawnArea.StartPoint.Y);

        float triangleRadius = GameSettings.GetTriangleCircumradius();
        GameObject = new CircleShape(triangleRadius, 3)
        {
            Position = new Vector2f(spawnX, spawnY),
            Origin = new Vector2f(triangleRadius, triangleRadius),
            FillColor = GameRenderer.FieldColor,
            OutlineColor = Color.Red,
            OutlineThickness = 2f
        };
    }
    
    public override void FixedUpdate()
    {
        KillIfVeryHungry();
        HandleIfOutsideCircle();
        
        GameObject.Position += Velocity * GameLoop.DeltaTime;
    }
    
    private void KillIfVeryHungry()
    {
        if (_healthAmount <= 0)
        {
            IsDead = true;
            RemoveAgent(this);
        }

        _healthAmount -= 0.005f;
    }
}
