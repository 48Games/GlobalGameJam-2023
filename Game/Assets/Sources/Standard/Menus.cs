using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{
     private float mouvement = 0;
    public void SetFront()
    {
        mouvement = -80;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.GetComponentInChildren<Button>()?.Select();
    }

    public void SetBack()
    {
        mouvement = 80;
    }

    void Update()
    {
        if (mouvement != 0)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + mouvement, gameObject.transform.position.y, gameObject.transform.position.z); ;
            mouvement = (float)(mouvement / 1.1);
            if (mouvement < 1 && mouvement > -1)
            {
                if(mouvement > 0)
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);

                mouvement = 0;
            }
        }
    }

}
