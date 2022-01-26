using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    private static MapEditor s_instance = null;
    public static MapEditor Instance { get { Init(); return s_instance; } }

    public void SetupMap(Vector3 startPoint)
    {
        DrawWayPathLine(startPoint);
        GameObject arrow = ResourceManager.Instance.Instantiate("Environment/PathArrow");
        Util.FindOrAddParentSetChild("Environment", arrow.transform);
    }

    private void DrawWayPathLine(Vector3 startPoint)
    {
        LineRenderer lineRenderer = Util.GetOrAddComponent<LineRenderer>(gameObject);

        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        lineRenderer.positionCount = SpawnManager.Instance.WayPoints.Count + 1;

        lineRenderer.SetPosition(0, startPoint);
        for (int i = 0; i < SpawnManager.Instance.WayPoints.Count; i++)
        {
            lineRenderer.SetPosition(i + 1, SpawnManager.Instance.WayPoints[i].position);
        }
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("MapEditor");
            if (go == null)
            {
                go = new GameObject { name = "MapEditor" };
                go.AddComponent<MapEditor>();
            }

            s_instance = go.GetComponent<MapEditor>();
            Util.FindOrAddParentSetChild("Managers", s_instance.transform);
        }
    }
}
