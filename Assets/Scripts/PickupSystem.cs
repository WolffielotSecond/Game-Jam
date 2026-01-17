using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _backpackPrefab;
    [SerializeField] private GameObject _backpackUI;
    [SerializeField] private TMP_Text _backpackSlotText;
    [SerializeField] private TMP_Text _backpackCapacityText;
    [SerializeField] private TMP_Text _backpackScoreText;
    [Space]
    [SerializeField] private List<GameObject> _collectableList = new List<GameObject>();

    private int _currentSlotUsed;
    private int _currentCapacityUsed;
    private int _currentScore;
    private void Start()
    {
        UpdateBackpackUI(0, 0, 0);
    }
    private void Update()
    {
        transform.position = _player.transform.position;
        if (Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            _collectableList.Add(collision.gameObject);
            Edge();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            _collectableList.Remove(collision.gameObject);
            Edge();
            collision.gameObject.GetComponent<Collectable>().HideEdge();
        }
    }
    public void Edge()
    {
        if (_collectableList.Count > 0)
        {
            for (int i = 0; i < _collectableList.Count; i++)
            {
                _collectableList[i].GetComponent<Collectable>().HideEdge();
            }
            _collectableList[_collectableList.Count - 1].GetComponent<Collectable>().ShowEdge();
        }
    }
    public void Pickup()
    {
        if (_collectableList.Count > 0)
        {
            if (_currentCapacityUsed + _collectableList[_collectableList.Count - 1].GetComponent<Collectable>().GetWeight() > _player.GetComponent<Player>()._backpackCapacity) return;
            if (_currentSlotUsed + 1 > _player.GetComponent<Player>()._backpackSlots) return;
            GameObject pickedItem = _collectableList[_collectableList.Count - 1];
            pickedItem.GetComponent<Collectable>().HideEdge();
            pickedItem.GetComponent<Collectable>().Picked();
            pickedItem.SetActive(false);
            GameObject gameObject = Instantiate(_backpackPrefab);
            pickedItem.GetComponent<Collectable>().SetBackpackUI(gameObject);
            gameObject.transform.SetParent(_backpackUI.transform, false);
            BackpackText backpackText = gameObject.GetComponent<BackpackText>();
            backpackText.itemName = pickedItem.GetComponent<Collectable>().GetName();
            backpackText.itemValue = pickedItem.GetComponent<Collectable>().GetValue();
            backpackText.itemWeight = pickedItem.GetComponent<Collectable>().GetWeight();
            backpackText.itemObject = pickedItem;
            backpackText.SetText();
            Player playerScript = _player.GetComponent<Player>();
            playerScript.AddToBackpack(pickedItem.GetComponent<Collectable>());
            _currentCapacityUsed += pickedItem.GetComponent<Collectable>().GetWeight();
            _currentSlotUsed += 1;
            _currentScore += pickedItem.GetComponent<Collectable>().GetValue();
            UpdateBackpackUI(_currentSlotUsed, _currentCapacityUsed, _currentScore);
        }
    }
    public void RemoveFromBackpack(int weight, int value)
    {
        _currentCapacityUsed -= weight;
        _currentSlotUsed -= 1;
        _currentScore -= value;
        UpdateBackpackUI(_currentSlotUsed, _currentCapacityUsed, _currentScore);
    }
    public void UpdateBackpackUI(int slotUsed, int capacity, int score)
    {
        _backpackCapacityText.text = "Weight: " + capacity + "/" + _player.GetComponent<Player>()._backpackCapacity;
        _backpackSlotText.text = "Slots: " + slotUsed + "/" + _player.GetComponent<Player>()._backpackSlots;
        _backpackScoreText.text = "Score: " + score;
    }
}