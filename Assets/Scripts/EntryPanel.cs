using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryPanel : MonoBehaviour
{
    [SerializeField]
    OnLineStart onLineStart;

    [SerializeField]
    GameObject entryName;
    GameObject eN;

    [SerializeField]
    GameObject entryRoom;
    GameObject eR;

    public void WindowOpen()
    {
        gameObject.SetActive(true);
        eN = Instantiate(entryName, this.transform);
        eN.GetComponentInChildren<Button>().onClick.AddListener(NameInput);
    }

    public void NameInput()
    {
        onLineStart.playerName = eN.GetComponentInChildren<InputField>().
            gameObject.GetComponentInChildren<Text>().text;
        Destroy(eN);
        eR = Instantiate(entryRoom, this.transform);
        eR.GetComponentInChildren<Button>().onClick.AddListener(RoomInput);
    }

    public void RoomInput()
    {
        onLineStart.roomName = eR.GetComponentInChildren<InputField>().
            gameObject.GetComponentInChildren<Text>().text;
        onLineStart.GameStart();
    }

    public void WindowClose()
    {
        foreach (Transform n in gameObject.transform)
        {
            Destroy(n.gameObject);
        }
        gameObject.SetActive(false);
    }
}
