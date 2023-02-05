using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visuals;

public class PlayerInLobby : MonoBehaviour
{
    [Header("Visuals")]
    public Color[] playerColors;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Lobby").GetComponent<Lobby>().OnPlayerJoin(this.gameObject);

        var characterVisual = GetComponentInChildren<CharacterVisual>();
        characterVisual.SetCharacterColor(playerColors[GameObject.FindGameObjectsWithTag("Player").Length - 1]);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
