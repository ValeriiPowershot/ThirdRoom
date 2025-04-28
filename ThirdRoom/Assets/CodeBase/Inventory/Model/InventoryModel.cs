using System.Collections.Generic;
using CodeBase.Data;

namespace CodeBase.Inventory.Model
{
    public class InventoryModel
    {
        private readonly List<Item> _items = new();
        public IReadOnlyList<Item> Items => _items;
        public int CurrentIndex { get; private set; }

        public bool AddItem(Item newItem)
        {
            if (_items.Exists(i => i.Index == newItem.Index))
                return false;

            _items.Add(newItem);
            return true;
        }

        public Item GetCurrentItem()
        {
            if (_items.Count == 0)
                return null;

            return _items[CurrentIndex];
        }

        public void NextItem()
        {
            if (_items.Count == 0)
                return;

            CurrentIndex = (CurrentIndex + 1) % _items.Count;
        }

        public void PreviousItem()
        {
            if (_items.Count == 0)
                return;

            CurrentIndex = (CurrentIndex - 1 + _items.Count) % _items.Count;
        }
    }
}
