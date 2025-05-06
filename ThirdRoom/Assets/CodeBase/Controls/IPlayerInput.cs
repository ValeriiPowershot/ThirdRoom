using UnityEngine;

namespace CodeBase.Controls
{
    public interface IPlayerInput
    {
        Vector2 MoveDirection { get; }
        bool IsJumpPressed { get; }
        bool IsFirePressed { get; }
        bool IsInteractPressed { get; }
        bool IsOpenInventoryPressed { get; }
    }
}