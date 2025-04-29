using System;
using System.Collections;
using CodeBase.Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField] private CanvasGroup _hintsCanvasGroup;
        [SerializeField] private CanvasGroup _fullDescriptionCanvasGroup;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [Header("Full Description")]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _fulldDescriptionText;
        [Space(10)]
        
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

        public void Display(Transform target, Item item)
        {
            _target = target;
            _headerText.text = item.ItemName;
            _descriptionText.text = item.ItemDescription;

            StartCoroutine(DisplayRoutine());
        }

        private void ClearText()
        {
            _descriptionText.text = "";
            _headerText.text = "";
            _hintsCanvasGroup.DOKill();
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
            
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));

            if (_shouldMoveToStash) OnMoveToStashRequested?.Invoke(_target);
            if (_shouldDestroy) OnDestroyRequested?.Invoke();
            if (_shouldDisplayNotification) _obtainedNotificationUI.ShowNotification(_notificationItem);
            
            ClearText();
            
            yield return _backgroundCanvasGroup.DOFade(0f, _alphaFadeDuration).WaitForCompletion();
        }
    }
}
