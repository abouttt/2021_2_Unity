using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{ 
    public bool IsFinal
    {
        get; set;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.GetInstance.IsStart)
        {
            if (collision.tag == "ReturnBorder")
            {
                IsFinal = false;
                StairSpawner.GetInstance.StairPool.Return(this.gameObject);
            }
        }
    }
}
