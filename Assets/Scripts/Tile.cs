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
    public bool isSlectable;
    [SerializeField]
    public bool isOwned;

    TileManager tileManager;
    
    public Button buttonCompornent;

    private void Awake()
    {
        tileManager = gameObject.transform.parent.GetComponent<TileManager>();
        buttonCompornent = gameObject.GetComponent<Button>();
        isSlectable = true;
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
