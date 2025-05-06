using System;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.Controls
{
    public interface IPushInput
    {
        PlayerInputActions.PushActions PushActions { get; }
        Vector2 PushDirection { get; }
        bool IsPushInteractPressed { get; }
        bool IsPushInteractReleased { get; }
    }
}