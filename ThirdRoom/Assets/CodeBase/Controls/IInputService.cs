using UnityEngine;

namespace CodeBase.Controls
{
    public interface IInputService
    {
        //Player Action Map
        Vector2 MoveDirection { get; }
        bool IsJumpPressed { get; }
        bool IsFirePressed { get; }
        bool IsInteractPressed { get; }
        
        //Obtainer Action Map
        bool IsObtainerReadPressed { get; }
        bool IsObtainerTakePressed { get; }
        bool IsObtainerEscapePressed { get; }
        
        //Inventory Action Map
        bool IsNextButtonPressed { get; }

        bool IsPreviousButtonPressed { get; }

        void EnableActionMap(string actionMapName);
        void DisableActionMap(string actionMapName);
        void DisableAllActionMaps();
        PlayerInputActions GetPlayerInputActions();
    }
}