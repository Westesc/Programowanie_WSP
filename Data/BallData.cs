using TPW.Data.Components;

// About
//  Represents a drawable circular (2D) object.
//  When creating a IBallData we're returning BallData.

namespace TPW.Data {
    public interface IBallData {
        int Identifier { get; set; }
        ITransform Transform { get; set; }
        IRigidBody RigidBody { get; set; }
    }

    internal class BallData : IBallData {
        public int Identifier { get; set; }
        public ITransform Transform { get; set; }
        public IRigidBody RigidBody { get; set; }
        public BallData (
            int newIdentifier,
            ITransform newTransform,
            IRigidBody newRigidBody
        ) {
            Identifier = newIdentifier;
            Transform = newTransform;
            RigidBody = newRigidBody;
        }
	}
}
