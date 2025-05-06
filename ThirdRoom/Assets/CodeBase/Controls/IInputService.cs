using UnityEngine;

namespace CodeBase.Controls
{
    public interface IInputService : IInputManager, IPlayerInput, IInventoryInput, IObtainerInput, IPushInput
    {
        Vector2 CurrentMoveDirection { get; }
    }
}