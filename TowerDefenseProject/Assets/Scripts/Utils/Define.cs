using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scene
    {
        Unknown,
        MainMenu,
        SelectMap,
        OnePath,
        TwoPath,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        Click,
        Wheel,
        ZoomIn,
        ZoomOut,
    }

    public enum CameraMode
    {
        OriginalView,
        FreeView,
    }
}
