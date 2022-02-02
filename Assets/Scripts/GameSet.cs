using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSet : MonoBehaviour
{
    public List<PlayerData> playerDatas;

    [SerializeField]
    GameObject playerRecord;

    [Serializable]
    public class PlayerData
    {
        public Sprite image;
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

        StartCoroutine(SceneChangeWaiting());
    }

    List<PlayerData> rankList;
    GameObject parentObject;

    public IEnumerator SceneChangeWaiting()
    {
        yield return null;

        parentObject = GameObject.Find("RecordView");
        rankList = playerDatas.OrderByDescending(n => n.tilePoint).ToList();

        StartCoroutine(RecordUp());
    }

    public IEnumerator RecordUp()
    {
        yield return null;

        foreach (var p in rankList)
        {
            var r = Instantiate(playerRecord, parentObject.transform).GetComponent<PlayerRecord>();

            r.image.sprite = p.image;
            r.name.text = p.name;
            r.tileCount.text = p.tileCount.ToString();
            r.tilePoint.text = p.tilePoint.ToString();

            r.rank.text = (rankList.IndexOf(p) + 1).ToString();

            if (r.rank.text=="1")
            {
                r.GetComponent<Image>().sprite = r.ri[0];
            }
            else if(r.rank.text=="2")
            {
                r.GetComponent<Image>().sprite = r.ri[1];
            }
            else if (r.rank.text=="3")
            {
                r.GetComponent<Image>().sprite = r.ri[2];
            }
            else
            {
                r.GetComponent<Image>().sprite = r.ri[3];
            }
        }
    }
}
