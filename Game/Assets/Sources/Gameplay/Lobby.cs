using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Lobby:MonoBehaviour
{
    public static int MAX_PLAYER = 4;

    public static InputDevice[] players = new InputDevice[MAX_PLAYER];
    static int playernumber = 0;


    public void OnPlayerJoin( GameObject player)
    {
        player.transform.position = GetPosition(playernumber);
        players[playernumber] = player.GetComponent<PlayerInput>().GetDevice<InputDevice>();
        playernumber++;

        if (playernumber > 0)
            GameObject.FindGameObjectWithTag("BtnGo").GetComponent<Button>().interactable = true;

    }

    public void OnPlayerLeft(GameObject player)
    {
        Debug.Log("Player Left");
        playernumber--;
        //Destroy(players[playernumber].gameObject);
        players[playernumber] = null;


        if (playernumber > 1)
            GameObject.FindGameObjectWithTag("BtnGo").GetComponent<Button>().interactable = false;
    }

    private static Vector3 GetPosition(int n)
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
