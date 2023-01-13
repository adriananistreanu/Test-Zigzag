using DG.Tweening;
using UnityEngine;

namespace CodeBase.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIWindow : MonoBehaviour
    {
        private CanvasGroup _panel = null;

        protected virtual void Awake()
        {
            _panel ??= GetComponent<CanvasGroup>();
        }

        public virtual void ActivateWindow()
        {
            SwitchPanelByParameter(true);
        }
        
        public virtual void DeactivateWindow()
        {
            SwitchPanelByParameter(false);
        }

        protected void SwitchPanel(float duration = 0.5f)
        {
            bool enablePanel = _panel.alpha > 0;

            _panel.blocksRaycasts = !enablePanel;
            _panel.interactable = !enablePanel;
            _panel.DOFade(enablePanel ? 0 : 1, duration);
        }

        protected void SwitchPanelByParameter(bool enablePanel, float duration = 0.5f)
        {
            _panel.blocksRaycasts = enablePanel;
            _panel.interactable = enablePanel;
            _panel.DOFade(enablePanel ? 1 : 0, duration);
        }
    }
}