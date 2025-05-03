using CodeBase.Inventory.View;
using UnityEngine;

namespace CodeBase
{
    [DisallowMultipleComponent]
    public class CentralUI : MonoBehaviour
    {
        [field : SerializeField] public ObtainerUI ObtainerUI { get; private set; }
        [field : SerializeField] public InventoryView InventoryView { get; private set; }
        [field : SerializeField] public CursorUI CursorUI { get; private set; }
    }
}
