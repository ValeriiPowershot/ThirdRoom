using System;
using System.Collections;
using CodeBase.Data;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase
{
    public class ObtainerUI : MonoBehaviour
    {
        public event Action<Transform> OnMoveToStashRequested;
        public event Action OnDestroyRequested;
    
        [SerializeField] private ObtainedNotificationUI _obtainedNotificationUI;
        [SerializeField] private CanvasGroup _mainCanvasGroup;
        [SerializeField] private CanvasGroup _headerCanvasGroup;
        [SerializeField] private CanvasGroup _hintsCanvasGroup;
        [SerializeField] private CanvasGroup _fullDescriptionCanvasGroup;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
    
        [Header("Full Description")]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _fulldDescriptionText;

        [Header("Texts")]
        [SerializeField] private string _header;
        [SerializeField, TextArea] private string _description;
        [SerializeField] private string _notificationItem;

        [Header("Settings")]
        [SerializeField] private float _alphaFadeDuration = 0.25f;
        [SerializeField] private float _delayBeforePrintingCharacter = 0.25f;

        [Header("Behavior")]
        [SerializeField] private bool _shouldMoveToStash;
        [SerializeField] private bool _shouldDestroy;
        [SerializeField] private bool _shouldDisplayNotification;

        private Transform _target;

        public bool IsMainCanvasVisible => _mainCanvasGroup.alpha > 0.95f;
        public bool IsFullDescriptionVisible => _fullDescriptionCanvasGroup.alpha > 0.95f;

        public void Display(Transform target, Item item)
        {
            _target = target;
            _headerText.text = item.ItemName;
            _descriptionText.text = item.ItemDescription;

            StartCoroutine(DisplayRoutine());
        }

        public void ToggleMainCanvas(bool visible) => ToggleCanvasGroup(_mainCanvasGroup, visible);
        public void ToggleFullDescriptionCanvas(bool visible, string title = "", string description = "")
        {
            if (visible)
            {
                _titleText.text = title;
                _descriptionText.text = description;
            }
            ToggleCanvasGroup(_fullDescriptionCanvasGroup, visible);
        }

        private void ToggleCanvasGroup(CanvasGroup group, bool visible)
        {
            group.DOFade(visible ? 1f : 0f, _alphaFadeDuration).SetUpdate(true);
            group.interactable = visible;
            group.blocksRaycasts = visible;
        }

        private void ClearText()
        {
            _descriptionText.text = "";
            _headerText.text = "";
            _hintsCanvasGroup.DOKill();
        }

        private IEnumerator DisplayRoutine()
        {
            ToggleCanvasGroup(_mainCanvasGroup, true);
            //yield return _mainCanvasGroup.DOFade(1f, _alphaFadeDuration).WaitForCompletion();
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
            //ToggleCanvasGroup(_mainCanvasGroup, false);
        }
    }
}
