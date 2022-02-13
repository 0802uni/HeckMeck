using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

//タイルを内容する
//タイルに関するイベントを行う
//タイルは画像を差し替えたりしないのでSerializableで扱うことはしない
//単純に場に存在するタイルを配列でまとめ、取得されれば消していく
public class TileManager : MonoBehaviour
{
    [SerializeField]
    public List<Tile> tiles;

    [SerializeField]
    GameDirector gameDirector;
    [SerializeField]
    DiceManager diceManager;

    Player currentPlayer;

    [SerializeField]
    Color color;

    public AudioSource audioSource;


    private void Update()
    {
        if (gameDirector.players.Count > 0)
        {
            currentPlayer = gameDirector.players[gameDirector.playerTurn];
        }
    }

    internal void PermitToGet(int sum)
    {
        if (!diceManager.diceDatas.Last().isSelectAlready)
        {
            return;
        }
        foreach (var t in tiles.Where(t => t.num <= sum && t.FieldTile && !t.OwnedTile))
        {
            t.buttonCompornent.interactable = true;
            t.SelectableTile = true;
        }
        //player側のタイルもsumと等しいものは取得可能になる
        foreach (var ot in tiles.Where(t => t.num == sum && t.FieldTile && t.OwnedTile))
        {
            if (ot.transform.parent.GetComponent<Player>()!=currentPlayer)
            {
            ot.buttonCompornent.interactable = true;
            ot.SelectableTile = true;
            }
        }
    }

    public void TileClick(Tile tile)
    {
audioSource.Play();

        if (tile.OwnedTile)
        {
            tile = Steel(tile);
        }

        currentPlayer.OwningTile(tile);
    }

    private Tile Steel(Tile tile)
    {
        var steelTile = tiles.Find(n => n.num == tile.num);
        var victim = tile.transform.parent.GetComponent<Player>();
        if (victim.playerTile.Count > 1)
        {
            victim.playerTile.Where(n => !n.SelectableTile).Last().SelectableTile = true;
        }
        victim.playerTile.Remove(tile);
        return steelTile;
    }
}