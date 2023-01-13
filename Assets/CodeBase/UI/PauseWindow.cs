using CodeBase.Common;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class PauseWindow : UIWindow
    {
        [SerializeField] private Button restartBtn;
        [SerializeField] private Button resumeBtn;
        [SerializeField] private Button generalPauseBtn;

        private SoundsHolder SoundsHolder => SoundsHolder.Instance;
        
        private void Start()
        {
            AddListeners();
        }

        public override void ActivateWindow()
        {
            base.ActivateWindow();
            LevelLoader.PauseGame();
            generalPauseBtn.gameObject.SetActive(false);
        }

        public override void DeactivateWindow()
        {
            SwitchPanelByParameter(false, 0f);
        }

        private void AddListeners()
        {
            resumeBtn.onClick.AddListener(OnResumeBtnClick);
            restartBtn.onClick.AddListener(OnRestartBtnClick);
        }

        private void OnResumeBtnClick()
        {
            LevelLoader.UnpauseGame();
            DeactivateWindow();
            generalPauseBtn.gameObject.SetActive(true);
            SoundsHolder.clickSound.Play();
        }

        private void OnRestartBtnClick()
        {
            SoundsHolder.clickSound.Play();
            LevelLoader.UnpauseGame();
            LevelLoader.Retry();
        }
    }
}