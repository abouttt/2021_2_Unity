using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    [SerializeField]
    private Vector3 _delta = Vector3.zero;

    private GameObject _objectNameText = null;

    private void Start()
    {
        if (_objectNameText == null)
            _objectNameText = UI_WorldSpaceCanvas.Instance.AddObjectNameText(gameObject, _delta);
    }

    public void AddObjectNameText(Vector3 delta)
    {
        _objectNameText = UI_WorldSpaceCanvas.Instance.AddObjectNameText(gameObject, delta);
    }

    public void Destroy()
    {
        UI_WorldSpaceCanvas.Instance.DestroyObjectNameText(_objectNameText);
        Managers.Resource.Destroy(gameObject);
    }
}
