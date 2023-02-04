using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInLobby : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindObjectOfType<Lobby>().OnPlayerJoin(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
