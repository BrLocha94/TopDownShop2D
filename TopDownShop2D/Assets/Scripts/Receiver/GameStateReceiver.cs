namespace Project.Receivers
{
    using UnityEngine;
    using Project.Enums;
    using Project.Utils;
    using Project.Core;

    public sealed class GameStateReceiver : ReceiverBase<GameState>
    {
        [SerializeField]
        private UnityGameStateEvent onReceive;

        protected override void RegisterReceiver()
        {
            StateMachineController.onGameStateChangeEvent += OnReceiveUpdate;
            OnReceiveUpdate(StateMachineController.CurrentGameState);
        }

        protected override void UnregisterReceiver()
        {
            StateMachineController.onGameStateChangeEvent -= OnReceiveUpdate;
        }

        protected override void OnReceiveUpdate(GameState param)
        {
            onReceive?.Invoke(param);
        }
    }
}