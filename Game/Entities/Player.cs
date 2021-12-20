﻿using HunterXSavageness.Game.Entities.Abstractions;
using HunterXSavageness.Game.Helpers;
using SFML.Graphics;
using SFML.System;

namespace HunterXSavageness.Game.Entities;

public class Player : EntityBase
{
    public override Shape GameObject { get; }
    
    public override EntityType Type => EntityType.Player;

    public override Vector2f Velocity { get; } = new();
    
    protected override float WanderingSpeed => 200f;
    
    protected override float RunningSpeed => 12f; // Let it be for future mechanics

    private Vector2f _lastRecordedMousePosition;
    
    public Player()
    {
        float triangleRadius = GameSettings.GetTriangleCircumradius();
        GameObject = new CircleShape(triangleRadius, 3)
        {
            Position = new Vector2f(), // center of the map
            Origin = new Vector2f(triangleRadius, triangleRadius), // Origin in the center of the triangle  
            FillColor = GameRenderer.FieldColor,
            OutlineColor = Color.Magenta,
            OutlineThickness = 2f
        };
    }

    public void HandleRotation(Vector2f mousePosition)
    {
        _lastRecordedMousePosition = mousePosition;
        GameObject.Rotation = GameObject.Position.GetRotationAngle(mousePosition) + 90; // + 90 because of triangle shape
    }
    
    public void HandleMovement()
    {
        var destinationPoint = _lastRecordedMousePosition - GameObject.Position;
        if (destinationPoint.GetMagnitude() < 2f)
        {
            // Do nothing, we are already at destination point
            return;
        }
        
        float length = destinationPoint.GetMagnitude();
        var unitVector = destinationPoint / length;
        GameObject.Position += WanderingSpeed * GameLoop.DeltaTime * unitVector;
    }
}