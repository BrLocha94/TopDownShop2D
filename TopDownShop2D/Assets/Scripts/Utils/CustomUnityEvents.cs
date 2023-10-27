namespace Project.Utils
{
    using System;
    using UnityEngine.Events;
    using Project.Enums;

    [Serializable]
    public sealed class UnityGameStateEvent : UnityEvent<GameState> { }
}