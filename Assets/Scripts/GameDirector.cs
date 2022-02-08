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
    Sprite dobonImage;

    public float flipSpeed;

    private void Awake()
    {
        rerollButText = rerollButton.GetComponentInChildren<Text>();
        defaultString = rerollButText.text;
        playerTurn = 0;
    }

    public IEnumerator Dobon()
    {
        yield return new WaitForSeconds(1);

        dobonText.SetActive(true);

        StartCoroutine(FlipTile());

        yield return new WaitForSeconds(2);

        dobonText.SetActive(false);
        diceManager.resultButton.gameObject.SetActive(false);
        diceManager.Slide();

        NextTurn();
    }

    public IEnumerator FlipTile()
    {
        GameObject missingTile = tileManager.tiles.Where(n => !n.isOwned && n.isSlectable).Last().gameObject;

        Debug.Log(missingTile.transform.eulerAngles);

        float angle = 0;

        while (angle < 90)
        {
            angle += flipSpeed * Time.deltaTime;
            missingTile.transform.eulerAngles = new Vector3(0, angle, 0);
            yield return null;
        }

        missingTile.GetComponent<Image>().sprite = dobonImage;
        missingTile.GetComponent<Tile>().isSlectable = false;

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
        //場に取得可能なタイルがなかった場合ゲームを終了させる
        if (!tileManager.tiles.Any(n => !n.isOwned && n.isSlectable))
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

        tileManager.tiles.ForEach(n => n.buttonCompornent.interactable = false);

        diceManager.Reload();
        rerollButText.text = defaultString;
    }

    private void GameSet()
    {
        foreach (var p in players)
        {
            gameSet.playerDatas.Add(new GameSet.PlayerData());

            gameSet.playerDatas.Last().image = p.chara;
            gameSet.playerDatas.Last().name = p.myName;
            gameSet.playerDatas.Last().tileCount = p.ownedTiles.Count;
            gameSet.playerDatas.Last().tilePoint = p.ownedTiles.Sum(n => n.point);
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