using System.Collections;
using System.Collections.Generic;
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

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static void SetColorInChildren(GameObject go, Color color)
    {
        if (go == null)
        {
            Debug.Log("Util.SetColorChildren() : GameObject is null");
            return;
        }

        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.Log("Util.SetColorChildren() : renderers.Length is 0");
            return;
        }

        foreach (Renderer render in go.GetComponentsInChildren<Renderer>())
        {
            render.material.color = color;
        }
    }

    public static void DrawCircle(GameObject go, float radius, float lineWidth)
    {
        int segments = 360;
        LineRenderer line = GetOrAddComponent<LineRenderer>(go);
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;

        int pointCount = segments + 1;
        Vector3[] points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            float radian = Mathf.Deg2Rad * (i * 360.0f / segments);
            points[i] = new Vector3(Mathf.Sin(radian) * radius, 0.0f, Mathf.Cos(radian) * radius);
        }

        line.SetPositions(points);
    }

    public static void SetManagersChild(Transform child)
    {
        GameObject managers = GameObject.Find("Managers");
        if (managers == null)
        {
            managers = new GameObject { name = "Managers" };
        }

        child.parent = managers.transform;
    }
}
