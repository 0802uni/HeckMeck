using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;


//playerの持つタイルの情報を保持する
public class Player : MonoBehaviour
{
    [SerializeField]
    Text playerName;

    [SerializeField]
    public string myName;
    [SerializeField]
    public List<Tile> ownedTiles;

    GameDirector gameDirector;
    GameObject playerBoard;

    public Sprite chara;
    [SerializeField]
    Image image;

    private void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        playerBoard = GameObject.Find("PlayerBoard");
        gameDirector.players.Add(this);
        playerName.text = myName;
        image.sprite = chara;
        this.transform.SetParent(playerBoard.transform, false);
    }

    public void Owning(Tile tile)
    {
        if (ownedTiles.Count > 0)
        {
            ownedTiles.Last().GetComponent<Button>().interactable = false;
            ownedTiles.Last().isSlectable = false;
        }

        ownedTiles.Add(tile);
        tile.transform.SetParent(transform);
        tile.transform.position = transform.position;

        var scoreSum = ownedTiles.Sum(n => n.point);
        Debug.Log(myName + "　"
            + "所持タイル数：" + ownedTiles.Count + "　"
            + "ポイント合計：" + scoreSum);

        StartCoroutine(NextTurnDelay());
    }

    public IEnumerator NextTurnDelay()
    {
        yield return null;

        gameDirector.NextTurn();
    }
}
