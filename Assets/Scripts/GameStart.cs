using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

public class GameStart : MonoBehaviour
{
    public List<string> playerNames = new List<string>();

    [SerializeField]
    GameObject playerPrefab;

    public List<chara> charas;

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
            p.GetComponent<Player>().playerName = n;

            var sortedChara = charas.Where(n => !n.yet).ToList();
            var c = sortedChara[Random.Range(0, sortedChara.Count)];
            p.GetComponent<Player>().characterSprite = c.image;
            c.yet = true;
        }
    }
    [Serializable]
    public class chara
    {
        public Sprite image;
        public bool yet;
    }
}
