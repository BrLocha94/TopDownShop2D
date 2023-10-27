namespace Project.Utils
{
    using UnityEngine;

    public abstract class OnlyEditorObject : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_EDITOR
            return;
#endif
            DestroyObject(gameObject);
        }
    }
}