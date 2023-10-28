namespace Project.Structures.Character
{
    using System.Collections;
    using System.Collections.Generic;
    using Project.Enums;
    using UnityEngine;

    public class NonPlayableCharacter : CharacterBase
    {
        public void OnDirectionEvent(Direction direction)
        {
            UpdateDirection(direction);
        }

        protected override void UpdateDirection(Direction targetDirection)
        {
            animator.Play("idle_" + targetDirection.ToString().ToLower());
        }
    }
}