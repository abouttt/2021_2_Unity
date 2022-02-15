using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private TextMeshProUGUI _amountText = null;

    public Action UpdateItemAction { get; private set; }
    public bool IsEquipment { get; set; } = false;

    private Transform _canvas = null;
    private Transform _prevParent = null;
    private RectTransform _rect = null;
    private CanvasGroup _canvasGroup = null;

    private GameObject _player = null;

    public Transform PrevParent => _prevParent;

    private void Awake()
    {
        UpdateItemAction += SetItemAmount;

        _canvas = ItemInventorySystem.Instance.gameObject.transform.parent;
        _rect = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.blocksRaycasts = true;
        UpdateItemAction.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        UI_PopupCanvas.Instance.IsDragging = true;
        _prevParent = transform.parent;

        if (IsEquipment)
            _prevParent.GetComponent<UI_EquipmentSlot>().IsHasItem = false;
        else
            _prevParent.GetComponent<UI_ItemSlot>().IsHasItem = false;

        transform.SetParent(_canvas);
        transform.SetAsLastSibling();

        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UI_PopupCanvas.Instance.IsDragging = false;

        if (transform.parent == _canvas)
        {
            if (IsEquipment)
            {
                transform.SetParent(_prevParent);
                GetComponent<RectTransform>().position = _prevParent.GetComponent<RectTransform>().position;
            }
            else
            {
                GameObject go = Managers.Resource.Instantiate("FieldItem");
                go.GetComponent<ItemInfo>().CopyItemInfo(GetComponent<ItemInfo>());
                go.GetComponent<FieldItem>().AddObjectNameText(new Vector3(0.0f, 0.5f, 0.5f));
                go.transform.position = _player.transform.position;

                Managers.Resource.Destroy(gameObject);
                //transform.SetParent(_prevParent);
                //_rect.position = _prevParent.GetComponent<RectTransform>().position;
            }
        }

        _canvasGroup.alpha = 1.0f;
        _canvasGroup.blocksRaycasts = true;
    }

    private void SetItemAmount()
    {
        ItemInfo itemInfo = GetComponent<ItemInfo>();
        if (itemInfo.IsCountable)
        {
            _amountText.gameObject.SetActive(true);
            _amountText.text = itemInfo.Amount.ToString();
        }
        else
        {
            _amountText.gameObject.SetActive(false);
        }
    }
}
