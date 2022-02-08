using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static LayerMask GetLayerMask(Define.Layer layer)
    {
        string name = Enum.GetName(typeof(Define.Layer), layer);
        return LayerMask.GetMask(name);
    }
}
