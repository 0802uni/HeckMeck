using UnityEngine;
using UnityEngine.UI;


//タイルのオブジェクトにアタッチ
//TileManagerのイベントを呼び出す
[RequireComponent(typeof(Button))]
public class Tile : MonoBehaviour
{
    [SerializeField]
    public int num;
    [SerializeField]
    public int point;
    
    [SerializeField]
    public bool FieldTile;//Dobon時に場から除外するため必要
    [SerializeField]
    public bool OwnedTile;//playerが取得した時に使用
    [SerializeField]
    public bool SelectableTile;//取得可能かどうかの判定に使用


    TileManager tileManager;
    
    public Button buttonCompornent;

    private void Awake()
    {
        tileManager = gameObject.transform.parent.GetComponent<TileManager>();
        buttonCompornent = gameObject.GetComponent<Button>();
        FieldTile = true;
    }

    private void Start()
    {
        buttonCompornent.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        tileManager.TileClick(this);
    }
}
