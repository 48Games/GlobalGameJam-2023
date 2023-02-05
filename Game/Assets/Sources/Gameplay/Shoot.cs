using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    public float speed;
    public Player owner;

    public GameObject hitPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        var go = Instantiate(hitPrefab);
        go.transform.position = transform.position;
        Destroy(go, 0.5f);
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

    private void OnCollisionEnter(Collision other)
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
