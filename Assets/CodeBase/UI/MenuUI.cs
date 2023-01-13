using CodeBase.Ball;
using CodeBase.Common;
using CodeBase.Helpers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class MenuUI : UIWindow
    {
        [SerializeField] private Button tapToPlayBtn;
        [SerializeField] private TextMeshProUGUI bestScoreText;
        [SerializeField] private TextMeshProUGUI gameCountText;
        [SerializeField] private TextMeshProUGUI gemsCounter;
        [SerializeField] private BallControl ballControl;
        
        [SerializeField] private Button settingsBtn;
        [SerializeField] private UIWindow settingsWindow;

        private int _gameCount = -1;
        private static EventsHolder EventsHolder => EventsHolder.Instance;
        private static SoundsHolder SoundsHolder => SoundsHolder.Instance;

        protected override void Awake()
        {
            base.Awake();
            LoadSaves();
            IncrementGameCount();
        }

        private void Start()
        {
            AddListeners();
            ActivateStartUI();
        }

        private void AddListeners()
        {
            tapToPlayBtn.onClick.AddListener(OnPlayBtnClick);
            settingsBtn.onClick.AddListener(() => settingsWindow.ActivateWindow());
        }

        private void ActivateStartUI()
        {
            tapToPlayBtn.targetGraphic.DOFade(0f, 1f).SetLoops(-1, LoopType.Yoyo);
            gemsCounter.text = "x " + ballControl.CollectedGems;
            bestScoreText.text += ballControl.BestScore;
            gameCountText.text += _gameCount;
        }

        private void OnPlayBtnClick()
        {
            EventsHolder.startEvent?.Invoke();
            tapToPlayBtn.targetGraphic.DOKill();
            DeactivateWindow();
            settingsBtn.interactable = false;
            SoundsHolder.clickSound.Play();
        }
        
        private void IncrementGameCount()
        {
            _gameCount++;
            SaveGameCount();
        }
        
        private void SaveGameCount()
        {
            PlayerPrefs.SetInt(SaveKeys.GameCountSaveKey, _gameCount);
        }

        private void LoadSaves()
        {
            _gameCount = PlayerPrefs.GetInt(SaveKeys.GameCountSaveKey, _gameCount);
        }
    }
}