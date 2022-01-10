using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    public bool IsFinal { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SpawnBorder")
        {
            if (IsFinal)
            {
                StairSpawner.GetInstance.SpawnStair(2);
            }
        }
        else if (collision.tag == "ReturnBorder")
        {
            IsFinal = false;
            StairSpawner.GetInstance.StairPool.Return(this.gameObject);
        }
    }
}
