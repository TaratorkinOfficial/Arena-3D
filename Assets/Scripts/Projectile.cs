using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Projectile : MonoBehaviour
{
    private Transform _player;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private CharacterData _blueCharacterData;
    private float _damageForce;
    private MainCharacterController _characterController;
    private bool _isTeleported;

    [Inject] 
    private void Construct(
        [Inject(Id ="characterController")] MainCharacterController characterController,
        [Inject(Id = "Player")] Transform player)
    {
        _characterController = characterController;
        _player = player;
    }

    private void Start()
    {
        _damageForce = _blueCharacterData.damageForce;
        _characterController.TeleportEvent += PlayerTeleports;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            Destroy(gameObject);
        }
    }
    private void PlayerTeleports()
    {
        _isTeleported = true;
    }
    private void FlyToPlayer()
    {
        _agent.SetDestination(_player.position);
        if (_agent.remainingDistance > 0 && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _characterController.TakeForceDamage(_damageForce);
            Destroy(gameObject);
        }
    }
    private void FlyToPoint()
    {
        if (_agent.remainingDistance > 0 && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        _characterController.TeleportEvent -= PlayerTeleports;
    }

    private void Update()
    {
        if (_isTeleported)
        {
            FlyToPoint();
        }
        else
        {
            FlyToPlayer();
        }
    }
}
