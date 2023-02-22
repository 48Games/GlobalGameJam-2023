using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var pos = new List<int>();
        var players = new List<GameObject>();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<PlayerInLobbyOnline>().UpdateData();
            if (player.GetComponent<PlayerInLobbyOnline>().position != -1)
            {
                pos.Add(player.GetComponent<PlayerInLobbyOnline>().position);
            }
            else
            {
                players.Add(player);
            }
        }

        pos.Sort();
        int listPos = 0;
        foreach (GameObject player in players)
        {
            int value = 0;
            for(int i = 0; i < 4; i++)
            {
                if(listPos >= pos.Count)
                {
                    value = i;
                    break;
                }
                if(i < pos[listPos])
                {
                    value = i;
                    break;
                }
                listPos++;
            }
            // Position
            player.GetComponent<PhotonView>().RPC("UpdatePosition", RpcTarget.All, value);

        }
    }
}
