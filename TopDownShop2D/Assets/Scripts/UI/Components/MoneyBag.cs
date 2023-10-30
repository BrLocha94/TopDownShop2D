namespace Project.UI.Components
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;
    using Project.Core;

    public class MoneyBag : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI targetText;

        private void Awake()
        {
            DataController.onPlayerMoneyChanged += OnValueChanged;
        }

        private void OnDisable()
        {
            DataController.onPlayerMoneyChanged -= OnValueChanged;
        }

        private void OnValueChanged(int value)
        {
            targetText.text = value.ToString();
        }
    }
}