using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_WorldSpaceCanvas : MonoBehaviour
{
    #region ÄÚ·çÆ¾
    private static UI_WorldSpaceCanvas s_instance;
    public static UI_WorldSpaceCanvas Instance { get { return s_instance; } }

    private void Awake()
    {
        if (s_instance == null)
            s_instance = this;
    }
    #endregion

    [SerializeField]
    private Transform ObjectNamesTransform = null;

    public GameObject AddObjectNameText(GameObject target, Vector3 delta)
    {
        if (target == null)
            return null;

        GameObject go = Managers.Resource.Instantiate("UI/ObjectName", ObjectNamesTransform);
        go.GetComponent<FollowTargetWorldToScreen>().SetTarget(target, delta);

        ItemInfo itemInfo = target.GetComponent<ItemInfo>();
        if (itemInfo != null)
        {
            TextMeshProUGUI tm = Util.FindChild<TextMeshProUGUI>(go, null, true);

            tm.text = itemInfo.Name;

            switch (itemInfo.Tier)
            {
                case Define.ItemTier.Normal:
                    tm.color = Color.white;
                    break;
                case Define.ItemTier.Rare:
                    tm.color = Color.blue;
                    break;
                case Define.ItemTier.Legend:
                    tm.color = Color.yellow;
                    break;
            }
        }

        return go;
    }

    public void DestroyObjectNameText(GameObject objectNameText)
    {
        if (objectNameText == null)
            return;

        Managers.Resource.Destroy(objectNameText);
    }
}
