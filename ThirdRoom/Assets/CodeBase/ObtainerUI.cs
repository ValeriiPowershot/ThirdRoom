using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase
{
    [DisallowMultipleComponent]
    public class ObtainerUI : MonoBehaviour
    {
        public event Action<Transform> OnMoveToStashRequested;
        public event Action OnDestroyRequested;
        
        [SerializeField] private ObtainedNotificationUI _obtainedNotificationUI;
        [SerializeField] private CanvasGroup _mainCanvasGroup;
        [SerializeField] private CanvasGroup _backgroundCanvasGroup;
        [SerializeField] private CanvasGroup _headerCanvasGroup;
        [SerializeField] private CanvasGroup _pressKeyCanvasGroup;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _pressKeyText;
        [SerializeField] private string _header;
        [SerializeField, TextArea] private string _description;
        [SerializeField] private string _notificationItem;
        
        [Header("Settings")]
        [SerializeField] private float _alphaFadeDuration = 0.25f;
        [SerializeField] private float _delayBeforePrintingCharacter = 0.25f;
        [SerializeField] private float _blinkDuration = 0.55f;

        [Space(10)] 
        [SerializeField] private bool _shouldMoveToStash;
        [SerializeField] private bool _shouldDestroy;
        [SerializeField] private bool _shouldDisplayNotification;
        
        private Transform _target;

        public void Display(Transform target)
        {
            _target = target;
            _headerText.text = _header;
            _descriptionText.text = "";

            StartCoroutine(DisplayRoutine());
        }

        private void ClearText()
        {
            _descriptionText.text = "";
            _headerText.text = "";
            _pressKeyCanvasGroup.DOKill();
            _pressKeyText.alpha = 0f;
        }

        private IEnumerator DisplayRoutine()
        {
            yield return _mainCanvasGroup.DOFade(1f, _alphaFadeDuration).WaitForCompletion();
            yield return _backgroundCanvasGroup.DOFade(1f, _alphaFadeDuration).WaitForCompletion();
            yield return _headerCanvasGroup.DOFade(1f, _alphaFadeDuration);
            
            foreach (char character in _description)
            {
                _descriptionText.text += character;
                yield return new WaitForSeconds(_delayBeforePrintingCharacter);
            }
            
            yield return _pressKeyCanvasGroup.DOFade(1f, _alphaFadeDuration).WaitForCompletion();
            
            _pressKeyCanvasGroup.DOFade(0f, _blinkDuration).SetLoops(-1, LoopType.Yoyo);
            
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

            if (_shouldMoveToStash) OnMoveToStashRequested?.Invoke(_target);
            if (_shouldDestroy) OnDestroyRequested?.Invoke();
            if (_shouldDisplayNotification) _obtainedNotificationUI.ShowNotification(_notificationItem);
            
            ClearText();
            
            yield return _backgroundCanvasGroup.DOFade(0f, _alphaFadeDuration).WaitForCompletion();
        }
    }
}
