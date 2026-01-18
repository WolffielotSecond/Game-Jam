using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Health_Heart_Regulation : MonoBehaviour
{
    [SerializeField] GameObject _player;
    public SpriteRenderer[] full_hearts;
    private void Update()
    {
        for (int i = 0; i < 7; i++)
        {
            if (i < _player.GetComponent<Player>().life)
            {
                full_hearts[i].enabled = true;
            }
            else
            {
                full_hearts[i].enabled=false;
            }
        }
    }
}
