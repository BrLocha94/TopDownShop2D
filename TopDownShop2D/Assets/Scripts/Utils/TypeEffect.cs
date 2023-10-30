namespace Project.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;

    public class TypeEffect : MonoBehaviour
    {
        public Action onTypeEffectEnd;

        [SerializeField]
        private TextMeshProUGUI targetText;
        [SerializeField]
        private float typeFrequency = 0.01f;

        Coroutine typeRoutine = null;

        public bool isExecuting => typeRoutine != null;

        public void ClearText()
        {
            if (typeRoutine != null)
            {
                StopCoroutine(typeRoutine);
                typeRoutine = null;
            }

            targetText.text = string.Empty;
            targetText.maxVisibleCharacters = 0;
        }

        public void StartTypingText(string text)
        {
            targetText.text = text;
            
            if(typeRoutine != null)
            {
                StopCoroutine(typeRoutine);
                typeRoutine = null;
            }

            typeRoutine = StartCoroutine(TypeRoutine());
        }

        public void StopCurrentTyping()
        {
            if (typeRoutine != null)
            {
                StopCoroutine(typeRoutine);
                typeRoutine = null;
            }

            targetText.maxVisibleCharacters = targetText.text.Length;
            onTypeEffectEnd?.Invoke();
        }

        private IEnumerator TypeRoutine()
        {
            targetText.maxVisibleCharacters = 0;

            while(targetText.maxVisibleCharacters < targetText.text.Length)
            {
                targetText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typeFrequency);
            }

            typeRoutine = null;
            onTypeEffectEnd?.Invoke();
        }
    }
}