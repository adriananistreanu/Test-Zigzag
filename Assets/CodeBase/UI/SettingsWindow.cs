using System;
using CodeBase.Common;
using CodeBase.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class SettingsWindow : UIWindow
    {
        public event Action<bool> AutoControlSwitch;
        
        [SerializeField] private SwitchToggle autoControlSwitch;
        [SerializeField] private Button generalSettingsBtn;
        [SerializeField] private Button generalPauseBtn;
        [SerializeField] private Button quitBtn;
        [SerializeField] private Button soundBtn;
        [SerializeField] private Image soundImage;
        [SerializeField] private Sprite soundOnSprite;
        [SerializeField] private Sprite soundOffSprite;
        [SerializeField] private Button closeBtn;

        private static SoundsHolder SoundsHolder => SoundsHolder.Instance;

        protected override void Awake()
        {
            base.Awake();
            LoadSaves();
        }

        private void Start()
        {
            AddListeners();
            SetSoundImageState();
        }

        public override void ActivateWindow()
        {
            base.ActivateWindow();
            generalSettingsBtn.gameObject.SetActive(false);
            generalPauseBtn.gameObject.SetActive(false);
        }
        
        private void AddListeners()
        {
            quitBtn.onClick.AddListener(OnQuitBtnClick);
            soundBtn.onClick.AddListener(OnSoundBtnClick);
            closeBtn.onClick.AddListener(OnCloseBtnClick);
            autoControlSwitch.OnSwitchValue += AutoControlSwitchValue;
        }

        private void OnCloseBtnClick()
        {
            DeactivateWindow();
            generalSettingsBtn.gameObject.SetActive(true);
            generalPauseBtn.gameObject.SetActive(true);
            SoundsHolder.clickSound.Play();
        }

        private void OnQuitBtnClick()
        {
            Application.Quit();
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

        private void AutoControlSwitchValue(bool on)
        {
            AutoControlSwitch?.Invoke(on);
            SoundsHolder.clickSound.Play();
        }
        
        private void SaveSoundState()
        {
            PlayerPrefs.SetFloat(SaveKeys.SoundStateSaveKey, AudioListener.volume);
        }
        
        private void LoadSaves()
        {
            AudioListener.volume = PlayerPrefs.GetFloat(SaveKeys.SoundStateSaveKey, AudioListener.volume);
            var audioControlOn = PlayerPrefs.GetInt(SaveKeys.AutoControlSaveKey, 0) == 1  ? true : false;
            autoControlSwitch.SetToggle(audioControlOn);
        }

        private void OnDestroy()
        {
            autoControlSwitch.OnSwitchValue -= AutoControlSwitchValue;
        }
    }
}