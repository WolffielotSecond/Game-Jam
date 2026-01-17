using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject player;
    private float _colddown;
    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = _moveSpeed;
    }
    private void Update()
    {
        _colddown -= Time.deltaTime;
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
    }
}
