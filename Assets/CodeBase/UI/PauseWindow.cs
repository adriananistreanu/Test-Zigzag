using System;
using CodeBase.Common;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class PauseWindow : UIWindow
    {
        [SerializeField] private Button resumeBtn;
        
        private void Start()
        {
            AddListeners();
        }

        public override void ActivateWindow()
        {
            base.ActivateWindow();
            StartCoroutine(LevelLoader.PauseGame(0.5f));
        }

        private void AddListeners()
        {
            resumeBtn.onClick.AddListener(OnResumeBtnClick);
        }

        private void OnResumeBtnClick()
        {
            LevelLoader.UnpauseGame();
            SwitchPanelByParameter(false, 0f);
        }
        
    }
}