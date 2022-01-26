using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : MonoBehaviour
{
    public CreepController Creep = null;


    private void Update()
    {
        if (Creep == null)
            return;

        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;

        float ratio = Creep.Hp / (float)Creep.MaxHp;
        SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
        Util.FindChild<Slider>(gameObject).value = ratio;
    }
}
