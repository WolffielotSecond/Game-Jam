using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private GameObject _edge;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _pickupSystem;
    [Space]
    [SerializeField] private string _itemName;
    [SerializeField] private int _weight;
    [SerializeField] private int _value;
    [Space]
    [SerializeField] private bool _isPicked;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _pickupSystem = GameObject.FindGameObjectWithTag("PickupSystem");
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
