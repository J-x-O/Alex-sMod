using UnityEngine;

namespace AlexMod.Input {
    public interface IMovementSource {
        
        /* GatherInputs takes local inputs of the client and puts them into MoveData.
        * When no inputs are available the method is exited early. Note that data is set
        * to default at the very beginning of the method. You should pass default data into the
        * replicate method. Like when the server sends reconcile, the data will send redundantly to help
        * ensure it goes through, and also will stop sending data that hasn't changed after a few iterations.
        * You are welcome to always fill out data instead of sending default when there is no input
        * but this will cost you bandwidth. */
        public MoveData GatherInputSnapshot();

    }
    
    /* It's strongly recommended to use structures for your data.
 * Datas are cached on client and server, and will create garbage
 * if you use a class. */

    /* MoveData may be named whatever you like. In my script it's used to
     * store client inputs, which are later used to move the object identically
     * on the server and owner. */
    public struct MoveData {
        
        public Vector2 Movement;
        public bool Jumping;
        
        public MoveData(Vector2 movement, bool jumping) {
            Movement = movement;
            Jumping = jumping;
        }
    }
}