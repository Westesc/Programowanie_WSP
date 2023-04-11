using System.Numerics;

// About
//  Represents a drawable circular (2D) object.
//  When creating a IBallData we're returning BallData.

namespace TPW.Data {
    public interface IBallData {
        Vector2 Position { get; set; }
    }

    internal class BallData : IBallData {
        public Vector2 Position { get; set; }

        public BallData(Vector2 newPosition) {
            Position = newPosition;
        }
	}
}
