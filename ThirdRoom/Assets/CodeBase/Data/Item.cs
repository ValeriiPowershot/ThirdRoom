using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Data
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        public string ItemName; 
        public GameObject Item3DModel;
        public string ItemDescription;
        public int Index;
    }
}
