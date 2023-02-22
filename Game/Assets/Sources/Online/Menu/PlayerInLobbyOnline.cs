using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visuals;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using Photon.Pun;
using TMPro;

public class PlayerInLobbyOnline : MonoBehaviour
{
    [Header("Visuals")]
    public Color[] playerColors;
    public string playerName;
    public int position = -1;

    private void Start()
    {

    }

    public void UpdateData()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            GetComponent<PhotonView>().RPC("SetName", RpcTarget.All, Options.GetPlayerName());
            if(position != -1)
            {
                GetComponent<PhotonView>().RPC("SetPosition", RpcTarget.All, position);
            }
        }
    }

    [PunRPC]
    public void UpdatePosition(int position)
    {
        this.position = position;
    }

    [PunRPC]
    public void SetPosition(int position)
    {
        transform.position = Lobby.GetPosition(position);
        GetComponentInChildren<CharacterVisual>().SetCharacterColor(playerColors[position]);
        GetComponentInChildren<TextMeshPro>().color = playerColors[position];
    }

    [PunRPC]
    public void SetName(string name)
    {
        playerName = name;
        GetComponentInChildren<TextMeshPro>().text = playerName;
    }

}
