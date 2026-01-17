using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private int _moveSpeed = 40;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private GameObject _dayMap;
    [SerializeField] private GameObject _nightMap;
    [SerializeField] private RectTransform _lifeBar;
    [SerializeField] private RectTransform _colddownBar;
    [SerializeField] private RectTransform _warningBar;
    [SerializeField] private TMP_Text _lifeText;
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private GameObject _soundSystem;
    private float max = 150;
    private bool isDay = true;
    private float _warningLevel;
    private float _cooldown;
    private int life = 7;
    private float _damageCooldown;
    private bool _chasePositionSeted;
    private Vector3 _chasePosition;
    [SerializeField] private float _maxWarningLevel;
    [SerializeField] private List<GameObject> _enemys;
    void Start()
    {
        _enemys = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        foreach (var enemy in _enemys)
        {
            enemy.SetActive(false);
        }
        _cooldown = 30;
    }
    void Update()
    {
        float judge = Input.GetAxis("Mouse ScrollWheel");
        {     if (judge > 0f)
            {
                _moveSpeed += 1;
            }
            else if (judge < 0f)
            {
                _moveSpeed -= 1;
            }
        }
        if (_moveSpeed < 10)
        {
            _moveSpeed = 10;
        }
        if (_moveSpeed > 40)
        {
            _moveSpeed = 40;
        }
        _speedText.text = "Max Speed: " + _moveSpeed / 10f ;
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xspeed = x * _moveSpeed / 10f;
        float yspeed = y * _moveSpeed / 10f;
        float magnitude = (float)System.Math.Sqrt(xspeed * xspeed + yspeed * yspeed);
        if (magnitude > _moveSpeed / 10f)
        {
            xspeed = xspeed / magnitude * _moveSpeed / 10f;
            yspeed = yspeed / magnitude * _moveSpeed / 10f;
        }
        magnitude = (float)System.Math.Sqrt(xspeed * xspeed + yspeed * yspeed);
        _soundSystem.transform.localScale = new Vector3(magnitude * 3, magnitude * 3, magnitude * 3);
        _rb.velocity = new Vector2(xspeed, yspeed);
        _warningLevel -= Time.deltaTime;
        _cooldown -= Time.deltaTime;
        _damageCooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isDay)
            {
                SwichtoNight();
            }
            else if(!isDay && _cooldown <= 15)
            {
                SwichtoDay();
            }
        }
        if(_warningLevel <= 0)
        {
            _warningLevel = 0;
        }
        if (_warningLevel>= _maxWarningLevel)
        {
            if(_chasePositionSeted == false)
            {
                _chasePositionSeted = true;
                _chasePosition = transform.position;
            }
            foreach (var enemy in _enemys)
            {
                enemy.GetComponent<Enemy>().ChasePlayer(_chasePosition);
            }
            SetLifeBar(1f);
        }
        else
        {
            SetLifeBar(_warningLevel / _maxWarningLevel);
        }
        if (_warningLevel < _maxWarningLevel)
        {
            _chasePositionSeted = false;
        }
        if (isDay)
        {
            SetColddownBarDay(_cooldown);
            if (_cooldown <= 0)
            {
                SwichtoNight();
            }
        }
        else
        {
            SetColddownBarNight(_cooldown);
            if (_cooldown <= 0)
            {
                SwichtoDay();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Traps"))
        {
            _warningLevel += 5f;
        }
        if (collision.gameObject.CompareTag("Enemy") && _damageCooldown<=0)
        {
            life -= 1;
            _lifeText.text = "Life: " + life.ToString();
            collision.gameObject.GetComponent<Enemy>().StopChasing();
            _damageCooldown = 2f;
        }
    }
    void SwichtoNight()
    {
        _dayMap.SetActive(false);
        _nightMap.SetActive(true);
        isDay = false;
        _warningLevel += 7.5f;
        _cooldown = 30;
        foreach (var enemy in _enemys)
        {
            enemy.SetActive(true);
        }
    }
    void SwichtoDay()
    {
        _dayMap.SetActive(true);
        _nightMap.SetActive(false);
        isDay = true;
        foreach (var enemy in _enemys)
        {
            enemy.SetActive(false);
        }
        _cooldown = 30;
    }
    void SetLifeBar(float val)
    {
        Vector2 cur = _lifeBar.sizeDelta;
        cur.y = val * max;
        _lifeBar.sizeDelta = cur;
    }
    void SetColddownBarDay(float val)
    {
        Vector2 cur = _colddownBar.sizeDelta;
        cur.y = (val/30) * max;
        _colddownBar.sizeDelta = cur;
        cur = _warningBar.sizeDelta;
        cur.y = 0;
        _warningBar.sizeDelta = cur;
    }
    void SetColddownBarNight(float time)
    {
        if (time >= 15)
        {
            Vector2 cur = _colddownBar.sizeDelta;
            cur.y = System.Math.Abs(time - 15) / 15 * max;
            _colddownBar.sizeDelta = cur;
            cur = _warningBar.sizeDelta;
            cur.y = 0;
            _warningBar.sizeDelta = cur;
        }
        else if(time <= 0)
        {
            Vector2 cur = _colddownBar.sizeDelta;
            cur.y = 0;
            _colddownBar.sizeDelta = cur;
            cur = _warningBar.sizeDelta;
            cur.y = 0;
            _warningBar.sizeDelta = cur;
        }
        else { 
            Vector2 cur = _colddownBar.sizeDelta;
            cur.y = 0;
            _colddownBar.sizeDelta = cur;
            cur = _warningBar.sizeDelta;
            cur.y = System.Math.Abs(time - 15) / 15 * max;
            _warningBar.sizeDelta = cur;
        }
    }
}
