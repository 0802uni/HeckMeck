using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        foreach (var t in tiles.Where(t => t.num <= sum && t.isSlectable))
        {
            t.buttonCompornent.interactable = true;
        }
    }

    public void TileClick(Tile tile)
    {
        currentPlayer.Owning(tile);
        tile.isOwned = true;
    }


}