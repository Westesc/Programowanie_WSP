using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace TPW.Data.Components {
    public interface ITransform {
        Vector2 Position { get; set; }
        float Radius { get; set; }
    }
    
    internal class Transform : ITransform {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }
    
        public Transform (
            Vector2 newVelocity,
            float newRadius
        ) {
            Position = newVelocity;
            Radius = newRadius;
        }
    }
}
