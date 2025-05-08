using System;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class Item
    {
        public string ItemName { get; private set; } 
        public GameObject Item3DModel { get; private set; }
        public string ItemDescription { get; private set; }
        public int Index;
        
        public Item(string itemName, string itemDescription)
        {
            ItemName = itemName;
            ItemDescription = itemDescription;
        }
    }
}
