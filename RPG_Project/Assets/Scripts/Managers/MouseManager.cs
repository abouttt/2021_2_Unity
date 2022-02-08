using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager
{
    public System.Action<Define.MouseEvent> MouseAction = null;

    private int _mask = (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Interaction);

    private GameObject _mousePointTarget = null;
    private GameObject _objectNameCanvas = null;
    private GameObject _objectNameText = null;

    private bool _pressed = false;
    private float _pressedTime = 0;

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        NoInput();
        Input();
        Both();
    }

    private void NoInput()
    {
        if (UnityEngine.Input.GetMouseButton(0))
            return;

    }

    private void Input()
    {
        if (MouseAction != null)
        {
            if (UnityEngine.Input.GetMouseButton(0))
            {
                if (!_pressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                if (_pressed)
                {
                    if (Time.time < _pressedTime + 0.1f)
                    {
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    }
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                _pressed = false;
                _pressedTime = 0;
            }
        }
    }

    private void Both()
    {
        Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, _mask))
        {
            if (_mousePointTarget == hit.collider.gameObject)
                return;
            else
                ClearObject();

            _mousePointTarget = hit.collider.gameObject;

            if (_mousePointTarget.gameObject.layer == (int)Define.Layer.Interaction)
            {
                SetObjectNameText();
            }
            if (_mousePointTarget.gameObject.layer == (int)Define.Layer.Monster)
            {
                _mousePointTarget.GetComponent<Outline>().enabled = true;
            }
        }
        else
        {
            ClearObject();
        }
    }

    private void ClearObject()
    {
        if (_mousePointTarget == null)
            return;

        if (_mousePointTarget.layer == (int)Define.Layer.Interaction)
        {
            _objectNameText.SetActive(false);
        }
        if (_mousePointTarget.layer == (int)Define.Layer.Monster)
        {
            _mousePointTarget.GetComponent<Outline>().enabled = false;
        }

        _mousePointTarget = null;
    }

    private void SetObjectNameText()
    {
        _objectNameText.SetActive(true);
        _objectNameText.GetComponent<FollowTargetWorldToScreen>().SetTarget(_mousePointTarget);
        _objectNameText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _mousePointTarget.GetComponent<ObjectInfo>().Name;
    }

    public void Init()
    {
        GameObject root = new GameObject("MouseManager_Root");
        Object.DontDestroyOnLoad(root);

        _objectNameCanvas = Managers.Resource.Instantiate("UI/ObjectNameCanvas");
        _objectNameCanvas.transform.SetParent(root.transform); 
        _objectNameText = _objectNameCanvas.transform.GetChild(0).gameObject;
        _objectNameText.SetActive(false);
    }
}
