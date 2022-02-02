using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject playerPrefab;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0,LoadSceneMode arg1)
    {
        if (arg0.name=="MainScene")
        {
            var playerBoard = GameObject.Find("PlayerBoard");
            var player = Instantiate(playerPrefab,playerBoard.transform);
        }
    }
}
