using System;
using CodeBase.Ball;
using CodeBase.Common;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    [RequireComponent(typeof(UIPanel))]
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private Button tapToPlayBtn;
        [SerializeField] private TextMeshProUGUI bestScoreText;
        [SerializeField] private TextMeshProUGUI gameCountText;
        [SerializeField] private TextMeshProUGUI gemsCounter;
        [SerializeField] private BallControl ballControl;

        [SerializeField] private Button soundBtn;
        [SerializeField] private Image soundImage;
        [SerializeField] private Sprite soundOnSprite;
        [SerializeField] private Sprite soundOffSprite;
        
        private UIPanel _panel;
        private int _gameCount = -1;
        private const string SoundStateSaveKey = "SoundState";
        private const string GameCountSaveKey = "GameCount";
        private static  EventsHolder EventsHolder => EventsHolder.Instance;
        private static SoundsHolder SoundsHolder => SoundsHolder.Instance;

        private void Awake()
        {
            LoadSaves();
            IncrementGameCount();
        }

        private void Start()
        {
            _panel ??= GetComponent<UIPanel>();
            
            AddListeners();
            ActivateStartUI();
        }

        private void AddListeners()
        {
            tapToPlayBtn.onClick.AddListener(OnPlayBtnClick);
            soundBtn.onClick.AddListener(OnSoundBtnClick);
        }

        private void ActivateStartUI()
        {
            tapToPlayBtn.targetGraphic.DOFade(0f, 1f).SetLoops(-1, LoopType.Yoyo);
            gemsCounter.text = "x " + ballControl.CollectedGems;
            bestScoreText.text += ballControl.BestScore;
            gameCountText.text += _gameCount;
            SetSoundImageState();
        }

        private void OnPlayBtnClick()
        {
            EventsHolder.startEvent?.Invoke();
            tapToPlayBtn.targetGraphic.DOKill();
            _panel.SwitchPanelByParameter(false);
            SoundsHolder.clickSound.Play();
        }

        private void OnSoundBtnClick()
        {
            AudioListener.volume = AudioListener.volume == 1f ? 0f : 1f;
            SetSoundImageState();
            SoundsHolder.clickSound.Play();
            SaveSoundState();
        }

        private void SetSoundImageState()
        {
            soundImage.sprite = AudioListener.volume == 0 ? soundOffSprite : soundOnSprite;
        }

        private void IncrementGameCount()
        {
            _gameCount++;
            SaveGameCount();
        }

        private void SaveSoundState()
        {
            PlayerPrefs.SetFloat(SoundStateSaveKey, AudioListener.volume);
        }
        
        private void SaveGameCount()
        {
            PlayerPrefs.SetInt(GameCountSaveKey, _gameCount);
        }

        private void LoadSaves()
        {
            AudioListener.volume = PlayerPrefs.GetFloat(SoundStateSaveKey, AudioListener.volume);
            _gameCount = PlayerPrefs.GetInt(GameCountSaveKey, _gameCount);
        }
    }
}