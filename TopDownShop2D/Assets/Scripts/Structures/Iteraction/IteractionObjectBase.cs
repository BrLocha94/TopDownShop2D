namespace Project.Structures.Iteraction
{
    using UnityEngine;
    using Project.Utils;
    using Project.Enums;

    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]

    public abstract class IteractionObjectBase : MonoBehaviour
    {
        [SerializeField]
        private UnityDirectionEvent onIteractionTriggerDirectionEvent;

        public virtual void ExecuteIteraction(Direction direction)
        {
            onIteractionTriggerDirectionEvent?.Invoke(direction);
        }

        protected abstract void AdvanceIteraction();
    }
}