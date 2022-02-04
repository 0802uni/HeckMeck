using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnLineStart : MonoBehaviourPunCallbacks
{
    public string playerName;

    public string roomName;

    public void GameStart()
    {
        DontDestroyOnLoad(this);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("OnlineScene");

        StartCoroutine(PlayerInstantiateDelay());
    }

    public IEnumerator PlayerInstantiateDelay()
    {
        yield return null;

        var player = PhotonNetwork.Instantiate("PlayerUI",new Vector3(0, 0, 0), Quaternion.identity)
            .GetComponent<OnlinePlayer>();

        Debug.Log("player参上");

        player.myName = playerName;
    }
}
