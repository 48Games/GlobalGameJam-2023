using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInLobby : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Lobby").GetComponent<Lobby>().OnPlayerJoin(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
