using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private AudioSource _bat;
    private void Update()
    {
        transform.position = _player.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !_bat.isPlaying) 
        {
            _bat.Play();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().ChasePlayer(_player.transform.position);
        }
    }
}