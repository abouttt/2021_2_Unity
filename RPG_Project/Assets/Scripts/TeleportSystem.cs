using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSystem : MonoBehaviour
{
    [SerializeField]
    private Vector3 _teleportPoint;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = _teleportPoint;
    }
}
