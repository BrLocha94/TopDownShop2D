namespace Project.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Project.Utils;
    using Project.Enums;
    using Project.Contracts;
    using Project.Structures.Inventory;

    public class GameController : MonoSingleton<GameController>, IReceiver<GameState>
    {
        [SerializeField]
        private CameraControler cameraControler;

        GameState currentGameState = GameState.NULL;

        protected override void ExecuteOnAwake()
        {
            base.ExecuteOnAwake();

            StateMachineController.InitializeStateMachine();
        }

        private void Start()
        {
            this.InvokeAfterFrame(() => StateMachineController.ExecuteTransition(GameState.INITIALIZING));
        }

        public void ReceiveUpdate(GameState updatedValue)
        {
            currentGameState = updatedValue;

            // Remove this to an loading screen logic
            if (currentGameState == GameState.INITIALIZING)
            {
                this.Invoke(0.5f, () => StateMachineController.ExecuteTransition(GameState.RUNNING));
                return;
            }
        }
    }
}