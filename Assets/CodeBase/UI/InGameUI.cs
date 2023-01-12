using System;
using CodeBase.Ball;
using CodeBase.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class InGameUI : UIWindow
    {
        [SerializeField] private BallControl ballControl;
        [SerializeField] private TextMeshProUGUI directionChangeCounter;
        [SerializeField] private Button pauseBtn;
        [SerializeField] private Button settingsBtn;
        [SerializeField] private UIWindow pauseWindow;
        [SerializeField] private UIWindow settingsWindow;
        
        private static  EventsHolder EventsHolder => EventsHolder.Instance;
        
        private void Start()
        {
            AddListeners();
        }

        private void AddListeners()
        {
            ballControl.ScoreChange += OnDirectionChange;
            pauseBtn.onClick.AddListener(() => pauseWindow.ActivateWindow());
            settingsBtn.onClick.AddListener(() => settingsWindow.ActivateWindow());
        }

        private void RemoveListeners()
        {
            ballControl.ScoreChange -= OnDirectionChange;
        }

        private void OnDirectionChange(int count)
        {
            directionChangeCounter.text = count.ToString();
        }

        public override void DeactivateWindow()
        {
            base.DeactivateWindow();
            pauseBtn.interactable = false;
        }

        private void OnEnable()
        {
            EventsHolder.startEvent.AddListener(ActivateWindow);
            EventsHolder.dieEvent.AddListener(DeactivateWindow);
        }

        private void OnDisable()
        {
            EventsHolder?.startEvent.RemoveListener(ActivateWindow);
            EventsHolder?.dieEvent.RemoveListener(DeactivateWindow);
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
    }
}