using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    public AudioClip selectionSFX;
    public AudioSource selectionSFXSource;
    GameObject btn = null;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if(selected != btn)
        {
            if(btn != null)
                btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
            btn = selected;
            btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            selectionSFXSource.PlayOneShot(selectionSFX);
        }


    }
}
