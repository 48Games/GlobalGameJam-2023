using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public GameObject road;
    public GameObject saw;

    public Transform limitA;
    public Transform limitB;
    public float outTime;
    public float moveTime;
    public float rotateSpeed;

    public bool inProcess = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!inProcess)
        {
            SpawnSaw();
        }
    }

    void SpawnSaw()
    {
        inProcess = true;
        saw.SetActive(true);
        saw.transform.position = new Vector3(limitA.position.x, -1.5f, limitA.position.z);
        saw.transform.rotation = new Quaternion(0, 0, 0, 0);
        StartCoroutine(SpawnSawCoroutine());
    }

    private IEnumerator SpawnSawCoroutine()
    {
        // Random time waiting
        float progress = 0.0f;
        float randomTime = Random.Range(4, 10);
        while (progress < randomTime)
        {
            yield return null;
            progress += Time.deltaTime;
        }


        progress = 0.0f;
        while (progress < outTime)
        {  
            yield return null;
            saw.transform.position = new Vector3(saw.transform.position.x, (progress / outTime) * 1.5f - 1.5f , saw.transform.position.z);
            progress += Time.deltaTime;
        }

        progress = 0.0f;
        while (progress < moveTime)
        {
            yield return null;
            if(limitA.transform.position.x != limitB.transform.position.x)
                saw.transform.position = new Vector3(limitA.transform.position.x + (limitB.transform.position.x - limitA.transform.position.x) * (progress / moveTime), saw.transform.position.y, saw.transform.position.z);
            else
                saw.transform.position = new Vector3(saw.transform.position.x, saw.transform.position.y, saw.transform.position.z);
            saw.transform.Rotate(new Vector3(saw.transform.rotation.x + Time.deltaTime * rotateSpeed, 0, 0));
            progress += Time.deltaTime;
        }

        progress = 0.0f;
        while (progress < outTime)
        {
            yield return null;
            saw.transform.position = new Vector3(saw.transform.position.x, (-progress / outTime) * 1.5f, saw.transform.position.z);
            progress += Time.deltaTime;
        }
        inProcess = false;

    }

    /*
    void spawnSaw()
    {
        TakeTwoPoints();
        transform.position = center;
        Vector3 polar = Vector3.zero;
        float angle = 0;
        polar.x = pointA.x - center.x;
        if(center.x < 0)
        {
            polar.x = pointA.x + center.x;
        }
        polar.z = pointA.z - center.z;
        if (center.z < 0)
        {
            polar.z = pointA.z + center.z;
        }
        if (polar.x > 0)
        {
            if(polar.z >= 0)
            {
                angle = Mathf.Atan(polar.z/polar.x);
            }
            else
            {
                angle = Mathf.Atan(polar.z/polar.x) + Mathf.PI * 2;
            }
        }
        else if(polar.x < 0)
        {
            angle = Mathf.Atan(polar.z / polar.x) + Mathf.PI;
        }
        else
        {
            if(polar.z > 0)
            {
                angle = Mathf.PI / 2;
            }
            else if(polar.z < 0)
            {
                angle = Mathf.PI * 3 / 2;
            }
        }
        Debug.Log(angle);
        angle = angle * Mathf.Rad2Deg;
        transform.position = center;
        road.SetActive(true);
        road.transform.localScale = new Vector3(1, 1, getScale());
        transform.Rotate(new Vector3(0, angle, 0));


    }

    private float getScale()
    {
        float x = Mathf.Abs(pointA.x - pointB.x);
        float y = Mathf.Abs(pointA.y - pointB.y);
        float length = Mathf.Sqrt(x*x + y*y);
        return length * ratioScale;
    }

    void TakeTwoPoints()
    {
        // Calculate axes
        int axe1 = Random.Range(1, 5);
        if (axe1 == 5) axe1 = 4;
        int axe2 = Random.Range(1, 5);

        pointA = Vector3.zero;
        pointB = Vector3.zero;
        

        if(axe1 == 1)
        {
            pointA = GetRandomPointBeetweenTwoPoints(limitA.position, limitB.position);
            pointB = GetRandomPointBeetweenTwoPoints(limitC.position, limitD.position);
        } else if(axe1 == 2)
        {
            pointA = GetRandomPointBeetweenTwoPoints(limitB.position, limitC.position);
            pointB = GetRandomPointBeetweenTwoPoints(limitA.position, limitD.position);
        }
        else if (axe1 == 3)
        {
            pointA = GetRandomPointBeetweenTwoPoints(limitC.position, limitD.position);
            pointB = GetRandomPointBeetweenTwoPoints(limitA.position, limitB.position);
        }
        else if (axe1 == 4)
        {
            pointA = GetRandomPointBeetweenTwoPoints(limitD.position, limitA.position);
            pointB = GetRandomPointBeetweenTwoPoints(limitB.position, limitC.position);
        }

        center = new Vector3((pointA.x + pointB.x) / 2, 0, (pointA.z + pointB.z) / 2);
    }

    Vector3 GetRandomPointBeetweenTwoPoints(Vector3 A, Vector3 B)
    {
        Vector3 p = Vector3.zero;
        if (A.x < B.x)
            p.x = Random.Range(A.x + 3, B.x - 3);
        else if (A.x > B.x)
            p.x = Random.Range(B.x + 3, A.x - 3);
        else
            p.x = A.x;

        if (A.z < B.z)
            p.z= Random.Range(A.z + 3, B.z - 3);
        else if (A.z > B.z)
            p.z = Random.Range(B.z + 3, A.z - 3);
        else
            p.z = A.z;
        return p;
    }*/
}
