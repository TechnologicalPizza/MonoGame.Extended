﻿using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Tiled
{
    public sealed class TiledMapPolylineObject : TiledMapObject
    {
        public TiledMapPolylineObject(int identifier, string name, PointF[] points, SizeF size, Vector2 position, float rotation = 0, float opacity = 1, bool isVisible = true, string type = null) 
            : base(identifier, name, size, position, rotation, opacity, isVisible, type)
        {
            Points = points;
        }
        
        public PointF[] Points { get; }
    }
}