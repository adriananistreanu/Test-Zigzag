using CodeBase.Ball;
using CodeBase.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class LoseUI : UIWindow
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI bestScoreText;
        [SerializeField] private BallControl ballControl;
        [SerializeField] private Button retryButton;

        private static  EventsHolder EventsHolder => EventsHolder.Instance;
        private static SoundsHolder SoundsHolder => SoundsHolder.Instance;
        
        private void Start()
        {
            AddListeners();
        }

        public override void ActivateWindow()
        {
            base.ActivateWindow();
            scoreText.text = ballControl.Score.ToString();
            bestScoreText.text = ballControl.BestScore.ToString();
        }

        private void AddListeners()
        {
            retryButton.onClick.AddListener(OnRetryBtnClick);
        }

        private void OnRetryBtnClick()
        {
            LevelLoader.Retry();
            SoundsHolder.clickSound.Play();
        }

        private void OnEnable()
        {
            EventsHolder.dieEvent.AddListener(ActivateWindow);
        }

        private void OnDisable()
        {
            EventsHolder?.dieEvent.RemoveListener(ActivateWindow);
        }

    }
}