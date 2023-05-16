using Data.Components;
using System.Numerics;

// About
//  Represents a drawable circular (2D) object.
//  When creating a IBallData we're returning BallData.

namespace TPW.Data {
    public interface IBallData {
        ITransform Transform { get; set; }
        IRigidBody RigidBody { get; set; }
    }

    internal class BallData : IBallData {
        public ITransform Transform { get; set; }

        public IRigidBody RigidBody { get; set; }

        public BallData (
            ITransform newTransform,
            IRigidBody newRigidBody
        ) {
            Transform = newTransform;
            RigidBody = newRigidBody;
        }
	}
}
