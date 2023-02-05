using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anvil : MonoBehaviour
{
    public float speed;
    public float lifeDuration;
    private bool active = true;
    
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player" && active)
        {
            other.gameObject.GetComponent<Player>().Death();
            if (active)
            {
                StartCoroutine(EndCoroutine());
            }
            active = false;
        }
        else
        {
            Debug.Log(other.gameObject);
            if (active)
            {
                StartCoroutine(EndCoroutine());
            }
            active = false;
        }
    }

    public void StartFalling()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, -1, 0) * speed;
    }

    private IEnumerator EndCoroutine()
    {
        float progress = 0.0f;
        while (progress < lifeDuration)
        {
            yield return null;
            progress += Time.deltaTime;
        }
        Destroy(transform.parent.gameObject);
    }


}
