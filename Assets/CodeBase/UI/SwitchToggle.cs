using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    [RequireComponent(typeof(Toggle))]
    public class SwitchToggle : MonoBehaviour
    {
        public event Action<bool> OnSwitchValue;

        [SerializeField] private Toggle toggle;
        [SerializeField] RectTransform uiHandleRectTransform;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image handleImage;

        [SerializeField] Color backgroundActiveColor;
        [SerializeField] Color handleActiveColor;
        
        private Color backgroundDefaultColor, handleDefaultColor;
        private Vector2 handlePosition;

        private void Start()
        {
            toggle.onValueChanged.AddListener(OnSwitch);

            handlePosition = uiHandleRectTransform.anchoredPosition;

            SetDefaultValues();

            if (toggle.isOn)
                OnSwitch(true);
        }

        private void OnSwitch(bool on)
        {
            uiHandleRectTransform.DOAnchorPos(on ? handlePosition * -1 : handlePosition, .4f).SetEase(Ease.InOutBack);
            backgroundImage.DOColor(on ? backgroundActiveColor : backgroundDefaultColor, .6f);
            handleImage.DOColor(on ? handleActiveColor : handleDefaultColor, .4f);
            OnSwitchValue?.Invoke(on);
        }

        public void SetToggle(bool value) => toggle.isOn = value;

        private void SetDefaultValues()
        {
            backgroundDefaultColor = backgroundImage.color;
            handleDefaultColor = handleImage.color;
        }

        private void OnDestroy()
        {
            toggle.onValueChanged.RemoveListener(OnSwitch);
        }
    }
}