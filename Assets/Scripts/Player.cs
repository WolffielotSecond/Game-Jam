using System.Collections.Generic;
using TMPro;
using UnityEngine;
struct CollectableData
{
    public string itemName;
    public float weight;
}
public class Player : MonoBehaviour
{

    [Header("引用")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private GameObject _dayMap;
    [SerializeField] private GameObject _nightMap;
    [SerializeField] private RectTransform _lifeBar;
    [SerializeField] private RectTransform _colddownBar;
    [SerializeField] private RectTransform _warningBar;
    [SerializeField] private TMP_Text _lifeText;
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private GameObject _evacIndi;
    [SerializeField] private TMP_Text _evacText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private GameObject _soundSystem;
    [SerializeField] private GameObject _reachSystem;
    [SerializeField] private GameObject _evacZone;
    [Space]
    [Header("数据")]
    public int _speed;
    public int _maxlife;
    public float _soundMultiplier;
    public float _reachRange;
    public float _evacTime;
    public float _maxSessionTime;
    public int _backpackCapacity;
    public int _backpackSlots;

    private int _moveSpeed = 40;
    private float max = 150;
    private bool isDay = true;
    private float _warningLevel;
    private float _cooldown;
    private int life = 7;
    private float _damageCooldown;
    private bool _chasePositionSeted;
    private Vector3 _chasePosition;
    private bool _isEvacing;
    private float _currentEvacTime;
    private float _sessionTime;
    [Space]
    [SerializeField] private float _maxWarningLevel;
    [Space]
    [SerializeField] private List<GameObject> _enemys;
    [SerializeField] private List<GameObject> _collectables;
    [Space]
    public List<GameObject> _Backback;
    public int _backpackScore;
    void Start()
    {
        _sessionTime = _maxSessionTime;
        _reachSystem.transform.localScale = new Vector3(_reachRange, _reachRange, _reachRange);
        _moveSpeed = _speed * 10;
        life = _maxlife;
        _sessionTime = _maxSessionTime;
        _enemys = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        _collectables = new List<GameObject>(GameObject.FindGameObjectsWithTag("Collectable"));
        foreach (var enemy in _enemys)
        {
            enemy.SetActive(false);
        }
        foreach (var collectable in _collectables)
        {
            collectable.SetActive(false);
        }
        _cooldown = 30;
    }
    void Update()
    {
        //玩家移动速度调节
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
        //玩家移动
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xspeed = x * _moveSpeed / 10f;
        float yspeed = y * _moveSpeed / 10f;
        //限制最大速度
        float magnitude = (float)System.Math.Sqrt(xspeed * xspeed + yspeed * yspeed);
        if (magnitude > _moveSpeed / 10f)
        {
            xspeed = xspeed / magnitude * _moveSpeed / 10f;
            yspeed = yspeed / magnitude * _moveSpeed / 10f;
        }
        magnitude = (float)System.Math.Sqrt(xspeed * xspeed + yspeed * yspeed);
        _soundSystem.transform.localScale = new Vector3(magnitude * _soundMultiplier, magnitude * _soundMultiplier, magnitude * _soundMultiplier);
        _rb.velocity = new Vector2(xspeed, yspeed);
        if(!isDay) _warningLevel += magnitude * 0.5f *Time.deltaTime;
        //倒计时与状态更新
        _warningLevel -= Time.deltaTime;
        _cooldown -= Time.deltaTime;
        _damageCooldown -= Time.deltaTime;
        _sessionTime -= Time.deltaTime;
        _timeText.text = "Time Left: " + _sessionTime.ToString("0.0") + "s";
        if (_isEvacing)
        {
            _currentEvacTime -= Time.deltaTime;
            _evacText.text = "Extraction in " + _currentEvacTime.ToString("0.0") + "s";
        }
        else
        {
            _currentEvacTime = _evacTime;
        }
        //眨眼切换
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
        //UI更新
        if (_warningLevel <= 0)
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
            SetColddownBarNight(_cooldown);
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
        //游戏结束判定
        if (life <= 0 || _sessionTime <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
            PlayerPrefs.SetInt("LifeLeft", life);
            PlayerPrefs.SetInt("SessionTime", (int)(_maxSessionTime - _sessionTime));
        }
        if(_isEvacing && _currentEvacTime <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Victory");
            PlayerPrefs.SetInt("LifeLeft", life);
            PlayerPrefs.SetInt("SessionTime", (int)(_maxSessionTime - _sessionTime));
            int score = 0;
            for (int i = 0; i < _Backback.Count; i++)
            {
                score += _Backback[i].GetComponent<Collectable>().GetValue();
            }
            PlayerPrefs.SetInt("Score", score);
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
            _warningLevel = 0;
        }
        if (collision.gameObject.CompareTag("RoomSwitcher"))
        {
            foreach (var enemy in _enemys)
            {
                enemy.GetComponent<Enemy>().StopChasing();
            }
            _warningLevel = 0;
        }
        if (collision.gameObject.CompareTag("Evac"))
        {
            _isEvacing = true;
            _evacIndi.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Evac"))
        {
            _isEvacing = false;
            _evacIndi.SetActive(false);
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
        foreach (var collectable in _collectables)
        {
            if(!collectable.GetComponent<Collectable>().IsPicked())
                collectable.SetActive(true);
        }
        _evacZone.SetActive(false);
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
        foreach (var collectable in _collectables)
        {
            if (!collectable.GetComponent<Collectable>().IsPicked())
                collectable.SetActive(false);
        }
        _cooldown = 30;
        _evacZone.SetActive(true);
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
    public void AddToBackpack(Collectable collectable)
    {
        GameObject item = collectable.gameObject;
        _Backback.Add(item);
    }
}
