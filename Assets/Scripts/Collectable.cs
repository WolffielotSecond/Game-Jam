using UnityEngine;

public class Collectable : MonoBehaviour
{
    private enum ItemType//物品类型
    {
        Med_Pack, //回血x1，多一个使用按键可以回血，回完消耗
        Treasure,//主要卖钱的
        Tear_Gas,//催泪弹，强制眨眼
        Accelerator//加速，短时间(1秒)速度上限提升到eg5.0, 用完消耗
    }
    [SerializeField] private GameObject _edge;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _pickupSystem;
    [SerializeField] private Sprite _icon;
    [Space]
    [Header("Item属性")]
    [SerializeField] private string _itemName;
    [SerializeField] private int _weight;
    [SerializeField] private int _value;
    [SerializeField] private ItemType _itemType;
    [Space]
    [SerializeField] private bool _isPicked;
    [Space]
    [Header("跨script调用，不用乱动里面的值")]
    public bool _dropped_during_day = false;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _pickupSystem = GameObject.FindGameObjectWithTag("PickupSystem");
        _icon = GetComponent<SpriteRenderer>().sprite;
    }
    public void ShowEdge()
    {
        _edge.SetActive(true);
    }
    public void HideEdge()
    {
        _edge.SetActive(false);
    }
    public void Picked()
    {
        _isPicked = true;
        
    }
    public void Dropped()
    {
        if (_player.gameObject.GetComponent<Player>().getIsDay())//防止白天drop出bug
        {
            _dropped_during_day = true;
        }
        else
        {
            _dropped_during_day = false;
        }
        _isPicked = false;
        transform.position = _player.transform.position + new Vector3(1f, 0f, 0f);
        _pickupSystem.GetComponent<PickupSystem>().RemoveFromBackpack(_weight, _value);
    }
    public string GetName()
    {
        return _itemName;
    }
    public int GetValue()
    {
        return _value;
    }
    public int GetWeight()
    {
        return _weight;
    }
    public bool IsPicked()
    {
        return _isPicked;
    }
}
