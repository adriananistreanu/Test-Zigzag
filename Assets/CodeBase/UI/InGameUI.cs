using System;
using CodeBase.Ball;
using CodeBase.Common;
using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    [RequireComponent(typeof(UIPanel))]
    public class InGameUI : MonoBehaviour
    {
        [SerializeField] private BallMovement ballMovement;
        [SerializeField] private TextMeshProUGUI directionChangeCounter;
        
        private UIPanel _panel;
        private EventsHolder EventsHolder => EventsHolder.Instance;
        
        private void Start()
        {
            _panel ??= GetComponent<UIPanel>();
            AddListeners();
        }

        private void ActivatePanel()
        {
            _panel.SwitchPanelByParameter(true);
        }

        private void AddListeners()
        {
            ballMovement.DirectionChange += OnDirectionChange;
        }

        private void OnDirectionChange(int count)
        {
            directionChangeCounter.text = count.ToString();
        }

        private void RemoveListeners()
        {
            ballMovement.DirectionChange -= OnDirectionChange;
        }

        private void OnEnable()
        {
            EventsHolder.startEvent.AddListener(ActivatePanel);
        }

        private void OnDisable()
        {
            EventsHolder?.startEvent.RemoveListener(ActivatePanel);
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
    }
}