using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameStart : MonoBehaviour
{
    public List<string> playerNames = new List<string>();

    [SerializeField]
    GameObject playerPrefab;

    public List<chara> charas;

    public GameDirector gameDirector;

void Update()
{
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        CloseWindow();
    }
}
    public void CloseWindow(){
            #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
    #elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
    #endif
    }

    public void OffLineStart()
    {
        DontDestroyOnLoad(this);

        SceneManager.LoadScene("MainScene");

        StartCoroutine(PlayerInstanceDelay());
    }

    IEnumerator PlayerInstanceDelay()
    {
        yield return null;

        gameDirector=GameObject.Find("GameDirector").GetComponent<GameDirector>();

        foreach (var n in playerNames)
        {
            var p = Instantiate(playerPrefab);
            p.GetComponent<Player>().playerName = n;

            var sortedChara = charas.Where(n => !n.yet).ToList();
            var c = sortedChara[Random.Range(0, sortedChara.Count)];
            p.GetComponent<Player>().characterSprite = c.image;
            c.yet = true;
        gameDirector.players.Add(p.GetComponent<Player>());

        }
    }
    [Serializable]
    public class chara
    {
        public Sprite image;
        public bool yet;
    }

}
