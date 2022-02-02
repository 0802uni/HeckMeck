using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

//サイコロを並べるパネル。
//サイコロに関する外交的なイベントを扱う
public class DiceManager : MonoBehaviour
{
    [SerializeField]
    public List<DiceData> diceDatas;
    [SerializeField]
    public List<Dice> dices;

    [SerializeField]
    public Button resultButton;

    [SerializeField]
    GameObject selectedPanel;

    public int sum = 0;
    [SerializeField]
    Text sumCalc;
    [SerializeField]
    TileManager tileManager;
    [SerializeField]
    GameDirector gameDirector;

    RectTransform rect;
    bool isSet;

    private void Awake()
    {
        //gameObject.SetActive(false);
        resultButton.gameObject.SetActive(false);
        resultButton.onClick.AddListener(Result);

        rect = GetComponent<RectTransform>();
        isSet = false;
    }

    public void Slide()
    {
        var pos = rect.localPosition;

        pos.x = (isSet) ? -rect.sizeDelta.x : 0;
        isSet = !isSet;

        rect.DOLocalMove(pos, 0.1f);
    }

    public void Reload()
    {
        dices.ForEach(n => n.transform.SetParent(selectedPanel.transform));

        foreach (var d in dices)
        {
            d.transform.SetParent(this.transform);
            d.diceData = diceDatas[0];
            d.isRollable = true;
            d.isSelected = false;
            d.isSlectable = true;
            d.buttonComponent.interactable = false;
        }
        diceDatas.ForEach(n => n.isSelectAlready = false);

        sum = 0;
        sumCalc.text = sum.ToString();
    }

    public void Roll()
    {
        foreach (var d in dices.Where(d => d.isRollable))
        {
            d.Roll();
        }

        if (dices.All(n => n.diceData.isSelectAlready))
        {
            Debug.Log("Dobon");
            StartCoroutine(gameDirector.Dobon());
            return;
        }

        if (dices.Where(n => n.isSlectable).Distinct().Count() == 1
            && dices.First().diceData.typeData != 6 && !diceDatas.Last().isSelectAlready)
        {
            Debug.Log("Dobon");
            StartCoroutine(gameDirector.Dobon());
            return;
        }
    }

    public void DiceClick(Dice dice)
    {
        //クリックしたサイコロと同数値の物が青く表示される
        //決定ボタンが出現する
        dices.ForEach(n => n.GetComponent<Image>().color = Color.white);
        dices.ForEach(n => n.isSelected = false);
        //場にあるサイコロ（Selectable）
        foreach (var d in dices.Where(n => n.isSlectable))
        {
            if (d.diceData == dice.diceData)
            {
                d.GetComponent<Image>().color = Color.cyan;
                d.isSelected = true;
            }
        }
        resultButton.gameObject.SetActive(true);
    }

    private void Result()
    {
        //既に選んだ目は帰される
        if (dices.Find(n => n.isSelected && n.diceData.isSelectAlready)) return;
        //選んだ目の種類を選択済みにする
        dices.First(n => n.isSelected&&n.isSlectable).diceData.isSelectAlready = true;
        //出現したボタンを押すと下部のパネルにサイコロが移動する
        foreach (var d in dices.Where(n => n.isSelected && n.isSlectable))
        {
            d.transform.SetParent(selectedPanel.transform);
            d.GetComponent<Image>().color = Color.white;
            sum += d.diceData.numData;
            d.buttonComponent.interactable = false;
            d.isSlectable = false;//場から除外
        }
        foreach (var d in dices.Where(n => n.isSlectable))
        {
            d.isRollable = true;
            d.buttonComponent.interactable = false;
        }
        sumCalc.text = sum.ToString();

        resultButton.gameObject.SetActive(false);

        Slide();

        tileManager.PermitToGet(sum);
    }
}

[Serializable]
public class DiceData
{
    public Sprite pipData;
    public int numData;
    public int typeData;
    public bool isSelectAlready;
}
