using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportSystem : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;
    [SerializeField]
    private Vector3 _teleportPoint;
    [SerializeField]
    private bool _isTeleportScene = false;

    private void OnTriggerEnter(Collider other)
    {
        //other.transform.position = _teleportPoint;
    }
}
