using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPeek : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q");
            //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 30f);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 30)), Time.time * 0.1f);
        }
    }
}
