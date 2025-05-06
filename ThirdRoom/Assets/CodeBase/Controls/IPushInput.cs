using System;
using UnityEngine;

namespace CodeBase.Controls
{
    public interface IPushInput
    {
        Vector2 PushDirection { get; }
        bool IsPushInteractPressed { get; }
    }
}