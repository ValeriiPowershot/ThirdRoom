using UnityEngine;

namespace CodeBase.Controls
{
    public interface IInputService
    {
        public Vector2 CurrentMoveDirection { get; }

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
        
        //Push Action Map
        Vector2 PushDirection { get; }
        
        bool IsPushInteractPressed { get; }
        

        void EnableActionMap(string actionMapName);
        void DisableActionMap(string actionMapName);
        void DisableAllActionMaps();
        PlayerInputActions GetPlayerInputActions();
    }
}