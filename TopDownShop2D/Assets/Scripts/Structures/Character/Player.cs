namespace Project.Structures.Characters
{
    using UnityEngine;
    using Project.Enums;
    using Project.Utils;
    using Project.Structures.Inventory;

    public class Player : CharacterBase
    {
        [Header("Player")]
        [SerializeField]
        private float moveSpeed = 5f;

        private Direction currentDirection = Direction.NULL;
        private Vector2 moveVector = Vector2.zero;

        private void OnClothEquiped(Item item)
        {
            if (item == null)
            {
                cloth.color = Color.white;
                return;
            }

            cloth.color = item.GetItemColor();
        }

        private void OnHatEquiped(Item item)
        {
            if (item == null)
            {
                hat.gameObject.SetActive(false);
                return;
            }

            hat.gameObject.SetActive(true);
            hat.sprite = item.GetItemSprite();
        }

        private void FixedUpdate()
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

        private void Update()
        {
            rigidBody.velocity = moveVector * moveSpeed * Time.deltaTime;
        }

        protected override void UpdateDirection(Direction nextDirection)
        {
            if (nextDirection == currentDirection) return;

            if (nextDirection == Direction.NULL)
                animator.Play("idle_" + currentDirection.ToString().ToLower());
            else
                animator.Play("walk_" + nextDirection.ToString().ToLower());

            currentDirection = nextDirection;

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