namespace Project.Structures.Iteraction
{
    using Project.Enums;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class IteractionDummy : IteractionObjectBase
    {
        public override void ExecuteIteraction(Direction direction)
        {
            Debug.Log("TRIGGERED ITERACTION");

            base.ExecuteIteraction(direction);
        }
    }
}