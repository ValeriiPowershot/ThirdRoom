using UnityEngine;

namespace CodeBase.Controls
{
    public interface IInputService
    {
        Vector2 MoveDirection { get; }
        bool IsJumpPressed { get; }
        bool IsFirePressed { get; }
        bool IsInteractPressed { get; }
        
        void EnableActionMap(string actionMapName);
        void DisableActionMap(string actionMapName);
        void DisableAllActionMaps();
        PlayerInputActions GetPlayerInputActions();
    }
}