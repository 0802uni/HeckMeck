using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System;


//playerの持つタイルの情報を保持する
public class Player : MonoBehaviour
{
    [SerializeField]
    Text nameLabel;

    [SerializeField]
    public string playerName;
    [SerializeField]
    public List<Tile> playerTile;

    GameDirector gameDirector;
    GameObject playerBoard;

    public Sprite characterSprite;
    [SerializeField]
    Image characterImage;

    private void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        playerBoard = GameObject.Find("PlayerBoard");
        nameLabel.text = playerName;
        characterImage.sprite = characterSprite;
        this.transform.SetParent(playerBoard.transform, false);
    }

    public void OwningTile(Tile tile)
    {
        if (playerTile.Count > 0)
        {
            playerTile.Last().GetComponent<Button>().interactable = false;
            playerTile.Last().FieldTile = false;
        }

        tile.OwnedTile = true;
        playerTile.Add(tile);
        tile.transform.SetParent(transform);
        tile.transform.position = transform.position;

        var scoreSum = playerTile.Sum(n => n.point);
        Debug.Log(playerName + "\n"
            + "所持タイル数：" + playerTile.Count + "\n"
            + "ポイント合計：" + scoreSum);

        StartCoroutine(NextTurnDelay());
    }

    public IEnumerator NextTurnDelay()
    {
        yield return null;

        gameDirector.NextTurn();
    }
}
