﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSpawner : MonoBehaviour
{
    public void StartHost()
    {
        Unity.Netcode.NetworkManager.Singleton.StartHost();
    }
    public void StartClient()
    {
        Unity.Netcode.NetworkManager.Singleton.StartClient();
    }
}
