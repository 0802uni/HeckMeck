using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerGenerater : MonoBehaviour
{
    int playerNum;

    [SerializeField]
    GameObject playerNameInput;

    GameObject parentObject;

    GameObject playerPrefab;

    List<string> playerNames;

    [SerializeField]
    GameObject playerNumInput;

    [SerializeField]
    List<InputField> playerNameInputs;

    [SerializeField]
    Button startButton;

    public void DecideNum()
    {
        var num = playerNumInput.GetComponentInChildren<InputField>().text;
        parentObject = playerNumInput.transform.parent.gameObject;

        playerNum = int.Parse(num);

        playerNumInput.SetActive(false);

        for (int i = 0; i < playerNum; i++)
        {
            var input=Instantiate(playerNameInput, parentObject.transform);
            input.GetComponentInChildren<Text>().text = "player" + (i+1) + ":";
            playerNameInputs.Add(input.GetComponentInChildren<InputField>());
        }

        startButton.interactable = true;
    }

    public void DecideName()
    {
        foreach (var n in playerNameInputs)
        {
            playerNames.Add(n.text);
        }

        DontDestroyOnLoad(this);
        SceneManager.LoadScene("MainScene");

        GameObject playerBoard = GameObject.Find("PlayerBoard");

        foreach (var n in playerNames)
        {
            var p = Instantiate(playerPrefab);
            p.GetComponent<Player>().myName = n;
        }
    }
}
