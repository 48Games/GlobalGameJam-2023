using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Visuals;

public class Lobby:MonoBehaviour
{
    public static int MAX_PLAYER = 4;

    public static InputDevice[] players = new InputDevice[MAX_PLAYER];
    public static GameObject[] playerObjects = new GameObject[MAX_PLAYER];

    static int playernumber = 0;
    
    public void OnPlayerJoin(GameObject player)
    {

        int pos = 0;
        for(int i = 0; i < MAX_PLAYER; i++)
        {
            if(playerObjects[i] == null)
            {
                pos = i;
                break;
            }
        }

        player.transform.position = GetPosition(pos);
        playerObjects[pos] = player;
        players[pos] = player.GetComponent<PlayerInput>().GetDevice<InputDevice>();
        player.GetComponentInChildren<CharacterVisual>().SetCharacterColor(player.GetComponent<PlayerInLobby>().playerColors[pos]);
        playernumber++;

        if (playernumber > 1)
        {
            GameObject.FindGameObjectWithTag("SelectMenu").GetComponent<Menus>().launchText.SetActive(true);
        }
            
    }

    public void OnPlayerLeft(GameObject player)
    {
        int pos = 0;
        for(int i = 0; i < MAX_PLAYER; i++)
        {
            if(playerObjects[i] == player)
            {
                pos = i;
                break;
            }
        }

        playernumber--;
        playerObjects[pos].GetComponent<PlayerInLobby>().Remove();
        players[pos] = null;
        playerObjects[pos] = null;

        if (playernumber < 2)
        {
            GameObject.FindGameObjectWithTag("SelectMenu").GetComponent<Menus>().launchText.SetActive(false);
        }

    }

    public void GetCancelAction(InputAction.CallbackContext context)
    {
        if(players.Contains(context.control.device))
        {
            int pos = 0;
            for (int i = 0; i < MAX_PLAYER; i++)
            {
                if (players[i] == context.control.device)
                {
                    pos = i;
                    break;
                }
            }
            OnPlayerLeft(playerObjects[pos]);
        }
        else
        {
            GameObject.FindGameObjectWithTag("SelectMenu").GetComponent<Menus>().ReturnToMainScreen();
        }
    }

    public static Vector3 GetPosition(int n)
    {
        int posX = 2;
        int posY = 1;
        int posZ = -7;

        if (n == 0)
            return new Vector3(-posX, posY, posZ);
        else if (n == 1)
            return new Vector3(posX, posY, posZ);
        else if (n == 2)
            return new Vector3(- posX, - posY, posZ);
        else if (n == 3)
            return new Vector3(posX, - posY, posZ);
        else
             return new Vector3(0, 0, 0);
    }
}
