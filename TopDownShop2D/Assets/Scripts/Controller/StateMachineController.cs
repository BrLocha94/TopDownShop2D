namespace Project.Core
{
    using UnityEngine;
    using Project.Enums;

    public static class StateMachineController
    {
        public delegate void OnGameStateChangeHandler(GameState gameState);
        public static event OnGameStateChangeHandler onGameStateChangeEvent;

        private static GameState lastGameState = GameState.NULL;
        private static GameState _currentGameState = GameState.NULL;

        public static GameState CurrentGameState
        {
            get
            {
                return _currentGameState;
            }
            private set
            {
                _currentGameState = value;
                onGameStateChangeEvent?.Invoke(_currentGameState);
            }
        }

        private static bool CheckCurrentState(GameState gameState) => _currentGameState == gameState;

        public static void InitializeStateMachine()
        {
            lastGameState = GameState.NULL;
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

            lastGameState = _currentGameState;
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
                    return true;
            }

            return false;
        }
    }
}