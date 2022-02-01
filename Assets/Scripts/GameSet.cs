using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSet : MonoBehaviour
{
    public List<PlayerData> playerDatas;

    [Serializable]
    public class PlayerData
    {
        public string name;
        public int tileCount;
        public int tilePoint;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    internal void Record()
    {
        SceneManager.LoadScene("GameSet");


    }
}
