using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using System.Collections;

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

    [SerializeField, Range(1, 9)]
    public int pipBugRate;
    [SerializeField, Range(1, 9)]
    public int Pip5Rate;
    [SerializeField, Range(1, 9)]
    public int PipOtherRate;

    const int bugDice = 6;

    private void Awake()
    {
        resultButton.gameObject.SetActive(false);
        resultButton.onClick.AddListener(Result);

        rect = GetComponent<RectTransform>();
        isSet = false;
    }

    private void Start()
    {
        Debug.Log("難易度：" + string.Join(",", dices.First().rollList.Select(n => n.ToString())));
    }

    public void Slide()
    {
        if (dices.Where(n => n.isSlectable).Count() == 0 && !isSet)
        {
            return;
        }

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
            d.diceImage.color = Color.white;
        }
        diceDatas.ForEach(n => n.isSelectAlready = false);

        sum = 0;
        sumCalc.text = sum.ToString();
    }

    public void RollDices()
    {
        //初期化
        foreach (var t in tileManager.tiles)
        {
            t.buttonCompornent.interactable = false;
            t.SelectableTile = false;
        }

        foreach (var d in dices.Where(d => d.isRollable))
        {
            d.Roll();
        }

        DobonJudge();
    }

    public void DobonJudge()
    {
        if (dices.All(n => n.diceData.isSelectAlready))
        {
            Debug.Log("Dobon：取得済みの数しかない");
            StartCoroutine(gameDirector.Dobon());
            return;
        }

        //Distinctの拡張メソッド出力テスト
        //typeDataを元に一意の配列数を取得
        var dist = dices.Where(n => n.isSlectable).Distinct(n => n.diceData.typeData).Count();
        Debug.Log(dist);


        if (dices.Where(n => n.isSlectable).Distinct(n => n.diceData.typeData).Count() == 1)
        {
            Debug.Log(dices.Where(n => n.isSlectable).First().diceData.typeData);

            if (dices.Where(n => n.isSlectable).First().diceData.typeData != bugDice
                && !diceDatas.First(n => n.typeData == bugDice).isSelectAlready)
            {
                Debug.Log("Dobon：最後の数が虫じゃない");
                StartCoroutine(gameDirector.Dobon());
                return;
            }
        }
    }


    public void DiceClick(Dice dice)
    {
        //クリックしたサイコロと同数値の物が青く表示される
        //決定ボタンが出現する
        dices.ForEach(n => n.diceImage.color = Color.white);
        dices.ForEach(n => n.isSelected = false);
        //場にあるサイコロ（Selectable）
        foreach (var d in dices.Where(n => n.isSlectable))
        {
            if (d.diceData == dice.diceData)
            {
                d.diceImage.color = Color.cyan;
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
        dices.First(n => n.isSelected && n.isSlectable).diceData.isSelectAlready = true;
        //出現したボタンを押すと下部のパネルにサイコロが移動する
        foreach (var d in dices.Where(n => n.isSelected && n.isSlectable))
        {
            d.transform.SetParent(selectedPanel.transform);
            d.diceImage.color = Color.white;
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

        //サイコロを全部使っても取れるタイルがなかった場合
        if (dices.Where(n => n.isSlectable).Count() == 0)
        {
            if (tileManager.tiles.Where(n => n.SelectableTile).Count() == 0)
            {
                StartCoroutine(gameDirector.Dobon());
            }
        }
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
