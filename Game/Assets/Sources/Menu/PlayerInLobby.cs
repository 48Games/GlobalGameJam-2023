using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visuals;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerInLobby : MonoBehaviour
{
    [Header("Visuals")]
    public Color[] playerColors;
    PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Lobby").GetComponent<Lobby>().OnPlayerJoin(this.gameObject);
        playerInput = GetComponent<PlayerInput>();
    }

    public void Submit()
    {
        if(!transform.GetChild(0).gameObject.activeInHierarchy)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("Lobby").GetComponent<Lobby>().OnPlayerJoin(this.gameObject);
        }
    }

    public void Remove()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Back(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Started)
        {
            GameObject.FindGameObjectWithTag("Lobby").GetComponent<Lobby>().GetCancelAction(context);
        }
    }



}
