namespace Project.Core
{
    using UnityEngine;
    using Project.Enums;

    public static class StateMachineController
    {
        public delegate void OnGameStateChangeHandler(GameState gameState);
        public static event OnGameStateChangeHandler onGameStateChangeEvent;

        private static GameState _lastGameState = GameState.NULL;
        private static GameState _currentGameState = GameState.NULL;

        public static GameState CurrentGameState
        {
            get
            {
                return _currentGameState;
            }
            private set
            {
                _lastGameState = _currentGameState;
                _currentGameState = value;
                onGameStateChangeEvent?.Invoke(_currentGameState);
            }
        }

        private static bool CheckCurrentState(GameState gameState) => _currentGameState == gameState;

        public static void InitializeStateMachine()
        {
            _lastGameState = GameState.NULL;
            CurrentGameState = GameState.NULL;
        }

        public static void ExecuteTransition(GameState nextState)
        {
            if (nextState == CurrentGameState) return;

            if (!CheckTransition(nextState))
            {
                Debug.Log("Unauthorized transition: " + CurrentGameState + " to " + nextState);
                return;
            }

            _lastGameState = _currentGameState;
            CurrentGameState = nextState;
        }

        private static bool CheckTransition(GameState nextState)
        {
            switch (_currentGameState)
            {
                case GameState.NULL:
                    if (nextState == GameState.INITIALIZING) return true;
                    break;

                case GameState.INITIALIZING:
                    if (nextState == GameState.RUNNING) return true;
                    break;

                case GameState.RUNNING:
                    if (nextState == GameState.DIALOG) return true;
                    if (nextState == GameState.INVENTORY) return true;
                    if (nextState == GameState.HELP) return true;
                    break;

                case GameState.HELP:
                    if (nextState == GameState.RUNNING) return true;
                    break;

                case GameState.DIALOG:
                    if (nextState == GameState.RUNNING) return true;
                    if (nextState == GameState.SHOP) return true;
                    break;

                case GameState.INVENTORY:
                    if (nextState == GameState.RUNNING) return true;
                    break;

                case GameState.SHOP:
                    if (nextState == GameState.DIALOG) return true;
                    if (nextState == GameState.SHOP_DIALOG) return true;
                    break;

                case GameState.SHOP_DIALOG:
                    if (nextState == GameState.SHOP) return true;
                    if (nextState == GameState.RUNNING) return true;
                    break;
            }

            return false;
        }
    }
}