using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{
    public AudioClip validationSFX;
    public AudioSource validationSFXSource;
    public GameObject inputManager;
    public GameObject launchText;

    public void Play()
    {
        Instantiate(inputManager);
        validationSFXSource.PlayOneShot(validationSFX);
        GameObject.FindGameObjectWithTag("Transition").GetComponent<Transition>().DoTransition(
                () =>
                {
                    GameObject.FindGameObjectWithTag("Menu").transform.GetChild(0).gameObject.SetActive(false);
                    GameObject.FindGameObjectWithTag("SelectMenu").transform.GetChild(0).gameObject.SetActive(true);
                }
            );
    }

    public void ReturnToMainScreen()
    {
        Destroy(GameObject.FindGameObjectWithTag("Lobby"));
        validationSFXSource.PlayOneShot(validationSFX);
        GameObject.FindGameObjectWithTag("Transition").GetComponent<Transition>().DoTransition(
                () =>
                {

                    foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        Destroy(player);
                    }
                    GameObject eventSystem = GameObject.FindGameObjectWithTag("EventSystem");
                    eventSystem.SetActive(false);
                    eventSystem.SetActive(true);
                    GameObject.FindGameObjectWithTag("Menu").transform.GetChild(0).gameObject.SetActive(true);
                    GameObject.FindGameObjectWithTag("SelectMenu").transform.GetChild(0).gameObject.SetActive(false);
                }
            );
    }

}
