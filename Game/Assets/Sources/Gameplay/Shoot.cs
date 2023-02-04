using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    public float speed;
    public Player owner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetVelocity(Vector3 direction)
    {
        GetComponent<Rigidbody>().velocity = direction * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        Player p = other.gameObject.GetComponent<Player>();
        if (p == null)
        {
            Destroy(gameObject);
        }
        else
        {
            p.Root();
            Destroy(gameObject);
        }
    }
}
