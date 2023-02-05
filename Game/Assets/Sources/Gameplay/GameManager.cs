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
    GameState state = GameState.BEGIN;
    private bool spawningAnvil = false;
    private PlayerInputManager playerInputManager = null;

    // Start is called before the first frame update
    void Start()
    {
        InputDevice[] list = Lobby.players;
        playerInputManager = GameObject.FindGameObjectWithTag("inputManager").GetComponent<PlayerInputManager>();
        for(int i = 0; i < list.Length; i++)
        {
            if(list[i] != null)
            {
                GameObject player = playerInputManager.JoinPlayer(i, -1, "Gamepad", list[i]).gameObject;
                player.GetComponent<Player>().SetupPlayer(i);
                player.GetComponent<Player>().Spawn = new Vector3(spawns[i].transform.position.x, player.transform.position.y, spawns[i].transform.position.z);
                player.transform.position = player.GetComponent<Player>().Spawn;
            }
        }
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
        int count = 0;
        players.ForEach((player) =>
        {
            if(player.active)
            {
                count++;
            }
        });
        if (count <= 1)
        {
            if (state == GameState.RUNNING)
                state = GameState.END;
        }

    }

    public void NewRound()
    {
        int winner = 0;
        state = GameState.BEGIN;
        players.ForEach((player) =>
        {
            if (player.active)
            {
                winner = player.GetComponent<Player>().PlayerID + 1;
            }
            player.gameObject.SetActive(true);
            player.transform.position = player.GetComponent<Player>().Spawn;
        });
        if (winner == 0)
        {
            Debug.Log("Draw");
        }
        else
        {
            Debug.Log("A player won: " + winner);
        }
    }

    public void PlayerRegister(GameObject player)
    {
        players.Add(player);
    }

}

enum GameState
{
    BEGIN,
    RUNNING,
    END,
    OFF
}