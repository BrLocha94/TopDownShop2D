namespace Project.Structures.Character
{
    using UnityEngine;
    using Project.Enums;
    using Project.Utils;
    using Project.Structures.Inventory;
    using Project.Core;

    public class Player : CharacterBase
    {
        [Header("Player")]
        [SerializeField]
        private float moveSpeed = 5f;
        [SerializeField]
        private UnityDirectionEvent onDirectionUpdate;

        private Direction currentDirection = Direction.NULL;
        private Vector2 moveVector = Vector2.zero;

        private void Start()
        {
            DataController.onClothEquiped += OnClothEquiped;
            DataController.onHatEquiped += OnHatEquiped;
        }

        private void OnDestroy()
        {
            DataController.onClothEquiped -= OnClothEquiped;
            DataController.onHatEquiped -= OnHatEquiped;
        }

        private void OnClothEquiped(Item item)
        {
            if (item == null)
            {
                cloth.color = Color.white;
                cloth.gameObject.SetActive(false);
                return;
            }

            cloth.color = item.GetItemColor();
            cloth.gameObject.SetActive(true);
        }

        private void OnHatEquiped(Item item)
        {
            if (item == null)
            {
                hat.gameObject.SetActive(false);
                return;
            }

            hat.sprite = item.GetItemSprite();
            hat.gameObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            rigidBody.velocity = moveVector * moveSpeed * Time.fixedDeltaTime;
        }

        private void Update()
        {
            if (currentGameState != GameState.RUNNING)
            {
                UpdateDirection(Direction.NULL);
                return;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                UpdateDirection(Direction.UP);
                return;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                UpdateDirection(Direction.DOWN);
                return;
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                UpdateDirection(Direction.LEFT);
                return;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                UpdateDirection(Direction.RIGHT);
                return;
            }

            UpdateDirection(Direction.NULL);
        }

        protected override void UpdateDirection(Direction nextDirection)
        {
            if (nextDirection == currentDirection) return;

            if (nextDirection == Direction.NULL)
                animator.Play("idle_" + currentDirection.ToString().ToLower());
            else
                animator.Play("walk_" + nextDirection.ToString().ToLower());

            currentDirection = nextDirection;
            onDirectionUpdate?.Invoke(currentDirection);

            UpdateMovimentVector();
        }

        private void UpdateMovimentVector()
        {
            if (currentDirection == Direction.NULL)
            {
                moveVector = Vector2.zero;
                return;
            }

            if (currentDirection == Direction.UP)
            {
                moveVector = new Vector2(0, 1);
                return;
            }

            if (currentDirection == Direction.DOWN)
            {
                moveVector = new Vector2(0, -1);
                return;
            }

            if (currentDirection == Direction.LEFT)
            {
                moveVector = new Vector2(-1, 0);
                return;
            }

            if (currentDirection == Direction.RIGHT)
            {
                moveVector = new Vector2(1, 0);
                return;
            }
        }
    }
}