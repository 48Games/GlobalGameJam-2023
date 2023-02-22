using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Options : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI playerName;
    [SerializeField]
    public TMP_InputField input;


    public static string GetPlayerName()
    {
        string playerName = PlayerPrefs.GetString("playerName", "Joueur");
        return playerName;
    }

    public static void SetPlayerName(string playerName)
    {
        PlayerPrefs.SetString("playerName", playerName);
    }


    public void UpdateName()
    {
        SetPlayerName(input.text);
        playerName.text = input.text;
        input.text = "";
    }

    private void Start()
    {
        input.characterLimit = 8;
        playerName.text = GetPlayerName();
    }
}
