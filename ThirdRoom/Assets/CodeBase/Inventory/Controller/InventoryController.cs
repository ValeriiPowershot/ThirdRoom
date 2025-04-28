using CodeBase.Data;
using CodeBase.Inventory.Model;
using CodeBase.Inventory.View;
using UnityEngine;
using Zenject;

namespace CodeBase.Inventory.Controller
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryView _view;
        private InventoryModel _model;
        private PlayerPrefab _playerPrefab;

        [Inject]
        public void Construct(PlayerPrefab playerPrefab)
        {
            _playerPrefab = playerPrefab;
            _model = new InventoryModel();
        }

        private void Start()
        {
            _view.NextButton.onClick.AddListener(OnNextItem);
            _view.PrevButton.onClick.AddListener(OnPrevItem);
            _view.HideInventory();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleInventory();
            }
        }

        private void ToggleInventory()
        {
            if (_view.InventoryPanel.activeSelf)
            {
                _view.HideInventory();
                _playerPrefab.UnlockCursor();
                _playerPrefab.UnblockInput();
            }
            else
            {
                _view.ShowInventory();
                _playerPrefab.LockCursor();
                _playerPrefab.BlockInput();
                _view.DisplayItem(_model.GetCurrentItem());
            }
        }

        private void OnNextItem()
        {
            _model.NextItem();
            _view.DisplayItem(_model.GetCurrentItem());
        }

        private void OnPrevItem()
        {
            _model.PreviousItem();
            _view.DisplayItem(_model.GetCurrentItem());
        }

        public void AddItem(Item item)
        {
            if (_model.AddItem(item))
            {
                Debug.Log("Item added to inventory.");
            }
            else
            {
                Debug.Log("Item already exists in inventory.");
            }
        }
    }
}
