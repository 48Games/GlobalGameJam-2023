using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawObject : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Player p = other.gameObject.GetComponent<Player>();
        if (p != null)
        {
            p.Death();
        }
    }
}
