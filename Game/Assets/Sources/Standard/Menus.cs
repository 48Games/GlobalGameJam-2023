using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
     private int mouvement = 0;
    public void SetFront()
    {
        mouvement = -100;
    }

    public void SetBack()
    {
        mouvement = 100;
    }

    void Update()
    {
        if (mouvement != 0)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + mouvement, gameObject.transform.position.y, gameObject.transform.position.z); ;
            mouvement = (int)(mouvement / 1.1);
            if (mouvement < 1 && mouvement > -1)
                mouvement = 0;
        }
    }

}
