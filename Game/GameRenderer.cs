﻿using SFML.Graphics;

namespace HunterXSavageness.Game;

public static class GameRenderer
{
    public static Color FieldColor => new(80, 80, 80);
    
    public static void RenderFrame(RenderWindow window, IEnumerable<Shape> targets)
    {
        window.SetActive(true);
        
        window.Clear(Color.Black);
        
        foreach (var target in targets)
        {
            window.Draw(target);
        }
        
        window.Display();
        
        window.SetActive(false);
    }
}
