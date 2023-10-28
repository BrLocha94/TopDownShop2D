namespace Project.Structures.Iteraction
{
    using UnityEngine;
    using Project.Enums;

    public class IteractionCheckManager : MonoBehaviour
    {
        [SerializeField]
        private IteractionTrigger upTrigger;
        [SerializeField]
        private IteractionTrigger downTrigger;
        [SerializeField]
        private IteractionTrigger leftTrigger;
        [SerializeField]
        private IteractionTrigger rightTrigger;

        public void UpdateDirection(Direction direction)
        {
            if (direction == Direction.NULL) return;

            upTrigger.gameObject.SetActive(direction == Direction.UP);
            downTrigger.gameObject.SetActive(direction == Direction.DOWN);
            leftTrigger.gameObject.SetActive(direction == Direction.LEFT);
            rightTrigger.gameObject.SetActive(direction == Direction.RIGHT);
        }
    }
}