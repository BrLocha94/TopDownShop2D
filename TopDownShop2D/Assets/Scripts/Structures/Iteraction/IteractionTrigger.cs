namespace Project.Structures.Iteraction
{
    using UnityEngine;
    using Project.Enums;
    using Project.Contracts;

    [RequireComponent(typeof(Collider2D))]
    public class IteractionTrigger : MonoBehaviour, IReceiver<GameState>
    {
        [SerializeField]
        private Direction iteractionDirection = Direction.NULL;

        private IteractionObjectBase target = null;
        private bool canCheckInput = false;

        private GameState currentGameState = GameState.NULL;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (target != null) return;

            IteractionObjectBase enterTarget = collision.gameObject.GetComponent<IteractionObjectBase>();

            if (enterTarget != null)
            {
                canCheckInput = true;
                target = enterTarget;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (target == null) return;

            IteractionObjectBase exitTarget = collision.gameObject.GetComponent<IteractionObjectBase>();

            if (exitTarget == target)
            {
                canCheckInput = false;
                target = null;
            }
        }

        private void Update()
        {
            if (currentGameState != GameState.RUNNING) return;

            if (!canCheckInput) return;

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
                target.ExecuteIteraction(iteractionDirection);
        }

        public void ReceiveUpdate(GameState updatedValue)
        {
            currentGameState = updatedValue;
        }
    }
}