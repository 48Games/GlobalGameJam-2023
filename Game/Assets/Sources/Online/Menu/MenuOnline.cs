using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuOnline : MonoBehaviour
{

    public TextMeshProUGUI input;
    public Launcher launcher;

    public void Create()
    {
        LobbyData.CreateGame = true;
        launcher.Connect();
    }

    public void Join()
    {
        LobbyData.CreateGame = false;
        LobbyData.RoomName = input.text.Substring(0, input.text.Length - 1);
        launcher.Connect();
    }

    public void Leave()
    {
        LobbyData.RoomName = "";
        launcher.Disconnect();
    }


}
