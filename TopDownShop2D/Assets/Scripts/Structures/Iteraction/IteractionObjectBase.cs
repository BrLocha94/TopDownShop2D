namespace Project.Structures.Iteraction
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Project.Utils;
    using Project.Enums;
    using Project.Core;

    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]

    public abstract class IteractionObjectBase : MonoBehaviour
    {
        [SerializeField]
        private UnityDirectionEvent onIteractionTriggerDirectionEvent;

        public virtual void ExecuteIteraction(Direction direction)
        {
            onIteractionTriggerDirectionEvent?.Invoke(direction);

            //NOTIFY
        }
    }
}