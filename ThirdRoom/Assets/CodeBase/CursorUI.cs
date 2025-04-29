using CodeBase.Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase
{
    [DisallowMultipleComponent]
    public class CursorUI : MonoBehaviour
    {
        [SerializeField] private Image _cursorImage;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        
        [Space]
        [SerializeField] private Sprite _defaultCursor;
        [SerializeField] private Sprite _interactableCursor;
        
        private Interactor _interactor;

        [Inject]
        public void Construct(PlayerPrefab playerPrefab) 
            => _interactor = playerPrefab.Interactor;

        private void Start()
        {
            _interactor.OnInteractObjectStateChanged += OnInteractObjectStateChanged;
        }

        private void OnDestroy()
        {
            _interactor.OnInteractObjectStateChanged -= OnInteractObjectStateChanged;
        }

        private void OnInteractObjectStateChanged(bool isFocused, string description)
        {
            _cursorImage.sprite = isFocused ? _interactableCursor : _defaultCursor;
            _descriptionText.text = description;
        }
    }
}
