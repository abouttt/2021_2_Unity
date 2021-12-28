using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGrenade : MonoBehaviour
{
    [SerializeField]
    private GameObject _grenadePrefab;
    [SerializeField]
    private float      _drawForce;
    [SerializeField]
    private Transform  _drawPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Draw();
        }
    }

    private void Draw()
    {
        GameObject grenade = Instantiate(_grenadePrefab);
        Rigidbody grenadeRigidbody = grenade.GetComponent<Rigidbody>();
        grenade.transform.position = _drawPosition.position;
        grenadeRigidbody.AddForce(new Vector3(0, 20, 0) + transform.forward * _drawForce);
    }
}
