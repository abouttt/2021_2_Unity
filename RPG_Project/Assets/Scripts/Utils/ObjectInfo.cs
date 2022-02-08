using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo : MonoBehaviour
{
    [SerializeField]
    private string _name = "None";
    [SerializeField]
    private Define.ObjectTier _tier = Define.ObjectTier.Normal;

    public string Name { get { return _name; } }
    public Define.ObjectTier Tier { get { return _tier; } }
}
