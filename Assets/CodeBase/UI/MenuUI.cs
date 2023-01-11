using CodeBase.Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    [RequireComponent(typeof(UIPanel))]
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private Button tapToPlayBtn;

        private UIPanel _panel;
        private EventsHolder EventsHolder => EventsHolder.Instance;
        
        private void Start()
        {
            _panel ??= GetComponent<UIPanel>();
            
            AddListeners();
            ActivateStartUI();
        }

        private void ActivateStartUI()
        {
            tapToPlayBtn.gameObject.SetActive(true);
            tapToPlayBtn.targetGraphic.DOFade(0f, 1f).SetLoops(-1, LoopType.Yoyo);
        }

        private void AddListeners()
        {
            tapToPlayBtn.onClick.AddListener(OnClickPlayBtn);
        }

        private void OnClickPlayBtn()
        {
            EventsHolder.startEvent?.Invoke();
            tapToPlayBtn.targetGraphic.DOKill();
            _panel.SwitchPanelByParameter(false);
        }
    }
}