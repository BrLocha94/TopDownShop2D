namespace Project.Structures.Character
{
    using UnityEngine;
    using Project.Enums;
    using Project.Contracts;

    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class CharacterBase : MonoBehaviour, IReceiver<GameState>
    {
        [Header("External references")]
        [SerializeField]
        protected Animator animator;
        [SerializeField]
        protected Rigidbody2D rigidBody;
        [SerializeField]
        protected SpriteRenderer cloth;
        [SerializeField]
        protected SpriteRenderer hat;

        protected GameState currentGameState = GameState.NULL;

        public virtual void ReceiveUpdate(GameState updatedValue)
        {
            currentGameState = updatedValue;
        }

        protected abstract void UpdateDirection(Direction nextDirection);
    }
}