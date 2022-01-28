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

    [SerializeField]
    GameStart gameStart;

    [SerializeField]
    GameObject playerNumInput;

    [SerializeField]
    List<InputField> playerNameInputs;

    [SerializeField]
    Button startButton;

    public void DecideNum(InputField num)
    {
        parentObject = playerNumInput.transform.parent.gameObject;

        playerNum = int.Parse(num.text);

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
            gameStart.playerNames.Add(n.text);
        }

        gameStart.OffLineStart();
    }
}
