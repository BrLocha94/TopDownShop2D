namespace Project.Structures.Iteraction
{
    using Project.Enums;
    using UnityEngine;

    public sealed class IteractionDummy : IteractionObjectBase
    {
        public override void ExecuteIteraction(Direction direction)
        {
            Debug.Log("TRIGGERED ITERACTION");

            base.ExecuteIteraction(direction);
        }

        protected override void AdvanceIteraction()
        {
            throw new System.NotImplementedException();
        }
    }
}