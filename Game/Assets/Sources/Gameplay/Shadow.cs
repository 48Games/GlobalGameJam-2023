using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Shadow : MonoBehaviour
{
    public float timeBeforeAnvil;
    public float scale;
    public GameObject anvil;
    public GameObject shadow;

    private float currentScale = 0;
    private float currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime < timeBeforeAnvil)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= timeBeforeAnvil)
            {
                currentTime = timeBeforeAnvil;
                // Active anvil
                anvil.GetComponent<Anvil>().StartFalling();
            }
            currentScale = scale * currentTime / timeBeforeAnvil;
            shadow.GetComponent<DecalProjector>().size = new Vector3(currentScale, currentScale, currentScale);
        }
    }
}
