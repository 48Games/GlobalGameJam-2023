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
    public GameObject playerObject;
    private bool inEnd = false;
    List<int> score = new();

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



        // For debug --------
        if (list[0] == null)
        {
            GameObject player = Instantiate(playerObject);
            int i = 0;
            player.GetComponent<Player>().SetupPlayer(i);
            player.GetComponent<Player>().Spawn = new Vector3(spawns[i].transform.position.x, player.transform.position.y, spawns[i].transform.position.z);
            player.transform.position = player.GetComponent<Player>().Spawn;
            player = Instantiate(playerObject);
            i = 1;
            player.GetComponent<Player>().SetupPlayer(i);
            player.GetComponent<Player>().Spawn = new Vector3(spawns[i].transform.position.x, player.transform.position.y, spawns[i].transform.position.z);
            player.transform.position = player.GetComponent<Player>().Spawn;
        }
        // ------------


    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.BEGIN:
                state = GameState.RUNNING;
                spawningAnvil = false;
                break;
            case GameState.RUNNING:
                // Anvils
                if(!spawningAnvil)
                {
                    StartCoroutine(SpawnAnvilCoroutine());
                }
                break;
            case GameState.END:
                if(!inEnd)
                {
                    inEnd = true;
                    StartCoroutine(EndRoundCoroutine());
                }
                break;
            case GameState.POSTGAME:
                StartCoroutine(PostGameCoroutine());
                break;
            case GameState.OFF:
                if (Input.anyKeyDown)
                {
                    state = GameState.BEGIN;
                    NewRound();
                }
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
        float timeblocked = 1f;
        while (timeblocked > 0)
        {
            yield return null;
            timeblocked -= Time.deltaTime;
        }

        if (score.Count == 0)
            for (int i = 0; i < players.Count; i++)
            {
                score.Add(0);
            }

        int winner = 0;
        players.ForEach((player) =>
        {
            if (player.activeInHierarchy)
            {
                winner = player.GetComponent<Player>().PlayerID + 1;
            }
        });
        bool end = false;
        string text = "";
        if (winner == 0)
        {
            text += "Draw\n\n";
        }
        
        else
        {
            score[winner - 1] += 1;
            if (checkWin(score)) // Check end
            {
                end = true;
                text += "Player " + (char)('A' + winner - 1) + " won the Game !\n";
            }
            else
            {
                text += "Player " + (char)('A' + winner - 1) + " won the round\n";
            }
        }
        text += "Player A : " + new string('*', score[0]) + "\n";
        text += "Player B : " + new string('*', score[1]) + "\n";
        if (players.Count > 2) text += "Player C : " + new string('*', score[2]) + "\n";
        if (players.Count > 3) text += "Player D : " + new string('*', score[3]) + "\n";

        GameObject.FindGameObjectWithTag("ScoreTxt").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.FindGameObjectWithTag("ScoreTxt").GetComponentInChildren<TextMeshProUGUI>().text = text;
        
        

        if(!end)
        {
            state = GameState.POSTGAME;
            inEnd = false;
        }
        else
        {
            state = GameState.QUIT;
        }
    }

    private bool checkWin(List<int> score)
    {
        for(int i = 0; i < score.Count; i++)
        {
            if (score[i] >= 5)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator PostGameCoroutine()
    {
        float timeblocked = 2f;
        while (timeblocked > 0)
        {
            yield return null;
            timeblocked -= Time.deltaTime;
        }
        if(state == GameState.POSTGAME)
            state = GameState.OFF;
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
            {
                state = GameState.END;
                players.ForEach((player) =>
                {
                    if (player.active)
                    {
                        player.transform.position = player.GetComponent<Player>().Spawn;
                    }
                });
            }
        }

        
    }

    public void NewRound()
    {
        Debug.Log("New Round Start");
        GameObject.FindGameObjectWithTag("ScoreTxt").transform.GetChild(0).gameObject.SetActive(false);

        players.ForEach((player) =>
        {
            player.gameObject.SetActive(true);
            player.transform.position = player.GetComponent<Player>().Spawn;
        });
        Debug.Log("New Round");

    }

    public void PlayerRegister(GameObject player)
    {
        players.Add(player);
    }

    public bool IsAbleToMove()
    {
        return state == GameState.RUNNING;
    }
}

enum GameState
{
    BEGIN,
    RUNNING,
    END,
    POSTGAME,
    OFF,
    QUIT
}