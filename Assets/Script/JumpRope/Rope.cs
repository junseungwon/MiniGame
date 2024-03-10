using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<JumpRopePlayer>())
        {
            Debug.Log(other.gameObject.name);
            other.GetComponent<JumpRopePlayer>().PlayerLose();
        }
    }
}
