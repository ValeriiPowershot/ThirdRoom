using CodeBase.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Inventory.View
{
    [DisallowMultipleComponent]
    public class InventoryView : MonoBehaviour
    {
        [field: SerializeField] public GameObject InventoryPanel {get; private set; }
        [field: SerializeField] public TextMeshProUGUI ItemNameText {get; private set; }
        [field: SerializeField] public TextMeshProUGUI ItemDescriptionText {get; private set; }
        [field: SerializeField] public Button NextButton {get; private set; }
        [field: SerializeField] public Button PrevButton {get; private set; }

        private GameObject _currentItemDisplay;
        private ObjectRotation _objectRotation;
        private Transform _itemDisplayParent;

        [Inject]
        public void Construct(ObjectRotation objectRotation, PlayerPrefab playerPrefab)
        {
            _objectRotation = objectRotation;
            _itemDisplayParent = playerPrefab.InventoryPoint;
        }
        
        public void ShowInventory()
        {
            InventoryPanel.SetActive(true);
        }

        public void HideInventory()
        {
            InventoryPanel.SetActive(false);
            ClearDisplay();
        }

        public void DisplayItem(Item item)
        {
            if (item == null)
                return;

            ItemNameText.text = item.ItemName;
            ItemDescriptionText.text = item.ItemDescription;

            ClearDisplay();

            if (item.Item3DModel == null) return;
            
            _currentItemDisplay = Instantiate(item.Item3DModel, _itemDisplayParent);
            _objectRotation.Activate(_currentItemDisplay.transform, false, _itemDisplayParent);
        }

        private void ClearDisplay()
        {
            foreach (Transform child in _itemDisplayParent) 
                Destroy(child.gameObject);
        }
    }
}
