using CodeBase.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Inventory.View
{
    public class InventoryView : MonoBehaviour
    {
        public GameObject InventoryPanel;
        public TextMeshProUGUI ItemNameText;
        public TextMeshProUGUI ItemDescriptionText;
        public Transform ItemDisplayParent;
        public Button NextButton;
        public Button PrevButton;

        private GameObject _currentItemDisplay;

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

            if (item.Item3DModel != null)
            {
                _currentItemDisplay = Instantiate(item.Item3DModel, ItemDisplayParent);
            }
        }

        private void ClearDisplay()
        {
            foreach (Transform child in ItemDisplayParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
