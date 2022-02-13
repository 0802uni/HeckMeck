using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;


//ゲームを進行する役割
//ゲームが終わっているか否か、タイル・サイコロが引けるか否かの情報を渡す
public class GameDirector : MonoBehaviour
{
    [SerializeField]
    public List<Player> players;

    [SerializeField]
    public int playerTurn;

    [SerializeField]
    TileManager tileManager;
    [SerializeField]
    DiceManager diceManager;
    [SerializeField]
    GameObject dobonText;

    [SerializeField]
    Button rerollButton;
    Text rerollButText;
    string defaultString;

    [SerializeField]
    GameSet gameSet;

    [SerializeField]
    GameObject gameSetText;

    [SerializeField]
    Sprite dobonSprite;

    public float flipSpeed;

    [SerializeField]
    Text message;

    Color defaultColor;

    private void Awake()
    {
        rerollButText = rerollButton.GetComponentInChildren<Text>();
        defaultString = rerollButText.text;
        playerTurn = 0;
    }

    private void Start()
    {
        StartCoroutine(GameStartDelay());
    }

    IEnumerator GameStartDelay(){
        yield return new WaitForSeconds(1.5f);

defaultColor=players[playerTurn].GetComponent<Image>().color;
        players[playerTurn].GetComponent<Image>().color=Color.yellow;
        message.text=players[playerTurn].playerName+"のターン";
    }

    public IEnumerator Dobon()
    {
        yield return new WaitForSeconds(1.5f);

        dobonText.SetActive(true);

        StartCoroutine(FlipMissingTile());

        yield return new WaitForSeconds(2);

        dobonText.SetActive(false);
        diceManager.resultButton.gameObject.SetActive(false);
        diceManager.Slide();

        NextTurn();
    }

    //非アクティブにするタイルを裏返す
    public IEnumerator FlipMissingTile()
    {
        GameObject missingTile = tileManager.tiles.Where(n => !n.OwnedTile && n.FieldTile).Last().gameObject;

        Debug.Log(missingTile.transform.eulerAngles);

        float angle = 0;

        while (angle < 90)
        {
            angle += flipSpeed * Time.deltaTime;
            missingTile.transform.eulerAngles = new Vector3(0, angle, 0);
            yield return null;
        }

        missingTile.GetComponent<Image>().sprite = dobonSprite;
        missingTile.GetComponent<Tile>().FieldTile = false;
        
        while (angle < 0)
        {
            angle -= flipSpeed * Time.deltaTime;
            missingTile.transform.eulerAngles = new Vector3(0, angle, 0);
            yield return null;
        }

        missingTile.transform.eulerAngles = Vector3.zero;
    }

    public void NextTurn()
    {
        players[playerTurn].GetComponent<Image>().color=defaultColor;

        //場に取得可能なタイルがなかった場合ゲームを終了させる
        if (!tileManager.tiles.Any(n => !n.OwnedTile && n.FieldTile))
        {
            GameSet();
            return;
        }

        Debug.Log("TurnEnd,NextTurn");

        playerTurn++;

        if (playerTurn > players.Count - 1)
        {
            playerTurn = 0;
        }

        players[playerTurn].GetComponent<Image>().color=Color.yellow;
        message.text=players[playerTurn].playerName+"のターン";

        tileManager.tiles.ForEach(n => n.buttonCompornent.interactable = false);
        tileManager.tiles.ForEach(n => n.SelectableTile = false);

        diceManager.Reload();
        rerollButText.text = defaultString;
    }

    private void GameSet()
    {
        foreach (var p in players)
        {
            gameSet.playerDatas.Add(new GameSet.PlayerData());

            gameSet.playerDatas.Last().image = p.characterSprite;
            gameSet.playerDatas.Last().name = p.playerName;
            gameSet.playerDatas.Last().tileCount = p.playerTile.Count;
            gameSet.playerDatas.Last().tilePoint = p.playerTile.Sum(n => n.point);
        }

        gameSetText.transform.DOLocalMoveX(0, 0.5f);

        StartCoroutine(DataLoadWaiting());
    }

    public IEnumerator DataLoadWaiting()
    {
        yield return new WaitForSeconds(2);

        gameSet.Record();
    }
}