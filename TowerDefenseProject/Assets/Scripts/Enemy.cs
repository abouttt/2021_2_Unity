using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _hp = 0.0f;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        _hp -= damage;
    }
}
