using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


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

        //playerに所有されていない中で最大のタイル
        tileManager.tiles.Where(n => !n.isOwned && n.isSlectable).
            Last().GetComponent<Image>().sprite = null;
        tileManager.tiles.Where(n => !n.isOwned && n.isSlectable).
            Last().isSlectable = false;

        yield return new WaitForSeconds(2);

        dobonText.SetActive(false);
        diceManager.resultButton.gameObject.SetActive(false);
        diceManager.Slide();

        NextTurn();
    }

    public void NextTurn()
    {
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
}
