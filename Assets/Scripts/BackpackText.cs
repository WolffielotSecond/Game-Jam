using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackpackText : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _value;
    [SerializeField] private TMP_Text _weight;
    [SerializeField] private Button _dropButton;
    [Space]
    public string itemName;
    public int itemValue;
    public float itemWeight;
    public GameObject itemObject;
    public void Start()
    {
        _dropButton.onClick.AddListener(DropItem);
    }
    public void SetText()
    {
        _name.text = itemName;
        _value.text = "Value: " + itemValue;
        _weight.text = "Weight: " + itemWeight;
    }
    public void DropItem()
    {
        itemObject.SetActive(true);
        itemObject.GetComponent<Collectable>().Dropped();
        Destroy(gameObject);
    }
}
