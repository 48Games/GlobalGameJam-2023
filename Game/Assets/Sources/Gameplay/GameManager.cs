using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public GameObject[] spawns;
    public GameObject anvil;
    public float spawnAnvilMin;
    public float spawnAnvilMax;
   
    List<GameObject> players = new();
    List<GameObject> playerDead = new();
    GameState state = GameState.BEGIN;
    private bool spawningAnvil = false;
    private PlayerInputManager playerInputManager = null;

    // Start is called before the first frame update
    void Start()
    {
        playerInputManager = GameObject.FindGameObjectWithTag("inputManager").GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.BEGIN:
                if(players.Count >= 2)
                {
                    state = GameState.RUNNING;
                    StartCoroutine(SpawnAnvilCoroutine());
                }
                break;
            case GameState.RUNNING:
                // Anvils
                if(!spawningAnvil)
                {
                    StartCoroutine(SpawnAnvilCoroutine());
                }
                break;
            case GameState.END:
                state = GameState.OFF;
                StartCoroutine(EndRoundCoroutine());
                break;

        }
    }

    private IEnumerator SpawnAnvilCoroutine()
    {
        spawningAnvil = true;
        float progress = 0.0f;
        while (progress < Random.value * (spawnAnvilMax - spawnAnvilMin) + spawnAnvilMin)
        {
            yield return null;
            if (state != GameState.RUNNING)
            {
                break;
            }
            progress += Time.deltaTime;
        }
        if (state == GameState.RUNNING)
        {
            players.ForEach((player) =>
            {
                Instantiate(anvil).transform.position = player.transform.position;
            });
            spawningAnvil = false;
        }
    }
    private IEnumerator EndRoundCoroutine()
    {
        float progress = 0.0f;
        while (progress < 1)
        {
            yield return null;
            progress += Time.deltaTime;
        }
        NewRound();
    }
    public void PlayerDie(GameObject player)
    {
        playerDead.Add(player);
        players.Remove(player);
        if (players.Count <= 1)
        {
            if (state == GameState.RUNNING)
                state = GameState.END;
        }

    }

    public void NewRound()
    {
        if(players.Count == 0)
        {
            Debug.Log("Draw");
        }
        else
        {
            Debug.Log("A player won");
        }
        state = GameState.BEGIN;
        playerDead.ForEach((player) =>
        {
            players.Add(player);
            player.gameObject.SetActive(true);
        });
        playerDead.Clear();
        for(int i = 0; i < players.Count; i++)
        {
            players[i].SetActive(true);
            players[i].transform.position = new Vector3(spawns[i].transform.position.x, players[i].transform.position.y, spawns[i].transform.position.z);
        }
    }

    public void PlayerRegister(GameObject player)
    {
        players.Add(player);
    }


    //public void OnPlayerJoined(PlayerInput playerInput)
    //{
    //    playerInput.GetComponent<Player>().SetupPlayer(playerInput.playerIndex);
    //}

    

}

enum GameState
{
    BEGIN,
    RUNNING,
    END,
    OFF
}