using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


//サイコロのオブジェクトにアタッチ
//サイコロの内部的なイベントはここで行う
//ここからDiceManagerのイベントを呼び出す
[RequireComponent(typeof(Button))]
public class Dice : MonoBehaviour
{
    [SerializeField]
    public DiceData diceData;

    [SerializeField]
    public bool isRollable;
    [SerializeField]
    public bool isSelected;
    [SerializeField]
    public bool isSlectable;

    DiceManager diceManager;
    public Button buttonComponent;
    public Image diceImage;

    [HideInInspector]
    public List<int> rollList = new List<int>();

    const int bugs = 5;
    const int five = 4;
    const int four = 3;
    const int three = 2;
    const int two = 1;
    const int one = 0;

    private void Awake()
    {
        diceManager = gameObject.transform.parent.GetComponent<DiceManager>();
        
        buttonComponent = gameObject.GetComponent<Button>();
        diceImage = gameObject.GetComponent<Image>();

        isRollable = true;
        isSelected = false;
        isSlectable = true;

        for (int i = 0; i < diceManager.pipBugRate; i++)
        {
            rollList.Add(bugs);
        }
        for (int i = 0; i < diceManager.Pip5Rate; i++)
        {
            rollList.Add(five);
        }
        for (int i = 0; i < diceManager.PipOtherRate; i++)
        {
            rollList.Add(four);
            rollList.Add(three);
            rollList.Add(two);
            rollList.Add(one);
        }
    }

    private void Start()
    {
        //Awake内だと代入されないので
        buttonComponent.onClick.AddListener(OnClick);
        buttonComponent.interactable = false;
        //初期値＝１
        diceData = diceManager.diceDatas[0];
    }

    private void Update()
    {
        //サイコロの画像を常に更新
        gameObject.GetComponent<Image>().sprite = diceData.pipData;
    }

    public void Roll()
    {
        //ランダムなインデックスを作成し、マネージャーのサイコロの大元の情報配列から引き出す
        int randomIndex = Random.Range(0, rollList.Count);
        diceData = diceManager.diceDatas[rollList[randomIndex]];
        isRollable = false;
        buttonComponent.interactable = true;
    }

    private void OnClick()
    {
        //ボタンコンポーネントによりマネージャーのDiceClickメソッドが呼び出される
        diceManager.DiceClick(this);
    }
}
