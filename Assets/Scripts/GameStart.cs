using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public List<string> playerNames = new List<string>();

    [SerializeField]
    GameObject playerPrefab;

    public void OffLineStart()
    {
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("MainScene");

        StartCoroutine(PlayerInstanceDelay());
    }

    IEnumerator PlayerInstanceDelay()
    {
        yield return null;

        foreach (var n in playerNames)
        {
            var p = Instantiate(playerPrefab);
            p.GetComponent<Player>().myName = n;
        }
    }
}
