using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerGenerater : MonoBehaviour
{
    int playerNum;
    [SerializeField]
    GameObject playerNumInput;
    GameObject instance;

    [SerializeField]
    GameObject playerNameInput;

    [SerializeField]
    GameObject parentObject;

    [SerializeField]
    GameStart gameStart;


    [SerializeField]
    List<GameObject> playerNameInputs;

    [SerializeField]
    Button startButton;

    public void StandUp()
    {
        gameObject.SetActive(true);
        playerNameInputs.Clear();
        startButton.interactable = false;
        instance=Instantiate(playerNumInput, parentObject.transform);
        instance.GetComponentInChildren<Button>().onClick.AddListener(DecideNum);
    }

    public void Close()
    {
        if (instance != null) Destroy(instance);
        instance = null;
        if (playerNameInputs.Count>0)
        {
            playerNameInputs.ForEach(Destroy);
        }
        playerNameInputs.Clear();
        gameObject.SetActive(false);
    }

    public void DecideNum()
    {
        var num = instance.GetComponentInChildren<InputField>();
        playerNum = int.Parse(num.text);

        for (int i = 0; i < playerNum; i++)
        {
            var input = Instantiate(playerNameInput, parentObject.transform);
            input.GetComponentInChildren<Text>().text = "player" + (i + 1) + ":";
            playerNameInputs.Add(input);
        }

        Destroy(instance);
        startButton.interactable = true;
    }

    public void DecideName()
    {
        foreach (var n in playerNameInputs)
        {
            gameStart.playerNames.Add(n.GetComponent<InputField>().text);
        }

        gameStart.OffLineStart();
    }
}
