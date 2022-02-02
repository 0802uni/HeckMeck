using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="OffLineGameData")]
public class OffLineGameData : ScriptableObject
{
    public List<PlayerData> playerDatas;

    [Serializable]
    public class PlayerData
    {
        public string name;
        public int tileCount;
        public int tilePoint;
    }
}
