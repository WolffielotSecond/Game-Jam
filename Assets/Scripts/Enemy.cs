using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _slowedMoveSpeed;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject player;
    private float _colddown;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = _moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Slower"))
        {
            agent.speed = _slowedMoveSpeed;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slower"))
        {
            agent.speed = _moveSpeed;
        }
    }
    private void Update()
    {
        _colddown -= Time.deltaTime;
        if (_colddown < 0)
        {
            gameObject.GetComponent<Collider2D>().enabled = true;//cd完了重新enable碰撞
        }
    }
    public void ChasePlayer(Vector3 location)
    {
        if (_colddown >= 0) return;
        agent.SetDestination(location);
    }
    public void StopChasing()
    {
        agent.SetDestination(transform.position);
        _colddown = 2f;
        gameObject.GetComponent<Collider2D>().enabled = false;//确保你扣血以后敌怪不会堵你
    }
}
