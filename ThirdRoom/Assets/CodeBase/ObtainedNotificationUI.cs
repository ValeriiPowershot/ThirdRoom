using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase
{
    [DisallowMultipleComponent]
    public class ObtainedNotificationUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _notificationPanel;
        [SerializeField] private TextMeshProUGUI _notificationText;
        [SerializeField] private float _slideDuration = 0.5f;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private float _displayDuration = 2f;

        private Vector2 _hiddenPosition;
        private Vector2 _visiblePosition;

        private void Awake()
        {
            _hiddenPosition = new Vector2(Screen.width + _notificationPanel.rect.width, 50f);
            _visiblePosition = new Vector2(Screen.width - _notificationPanel.rect.width / 2 - 100f, 50f);
            _notificationPanel.anchoredPosition = _hiddenPosition;
            _notificationPanel.GetComponent<CanvasGroup>().alpha = 0f;
        }

        public void ShowNotification(string itemName)
        {
            _notificationText.text = $"{itemName} Obtained";
            CanvasGroup canvasGroup = _notificationPanel.GetComponent<CanvasGroup>();

            _notificationPanel.DOAnchorPos(_visiblePosition, _slideDuration).SetEase(Ease.OutQuad);
            canvasGroup.DOFade(1f, _fadeDuration);

            DOVirtual.DelayedCall(_displayDuration, () =>
            {
                _notificationPanel.DOAnchorPos(_hiddenPosition, _slideDuration).SetEase(Ease.InQuad);
                canvasGroup.DOFade(0f, _fadeDuration);
            });
        }
    }
}
