using UnityEngine;
using Zenject;

public class RedEnemy : MonoBehaviour
{
    [SerializeField] private CharacterData characterData;
    private float _health;
    private float _maxHealth;
    private Animator _animator;
    private MainCharacterController _characterController;
    private Bullet _bulletScript;
    private int _chance;

    [Inject]
    private void Construct([Inject(Id = "characterController")] MainCharacterController characterController)
    {
        _characterController = characterController;
    }

    private void Start()
    {
        _health = characterData.health;
        _maxHealth = characterData.maxHealth;
        _animator = GetComponent<Animator>();
    }

    private void Die()
    {
        _characterController.MakeKills(1);
        _characterController.MakeForce(characterData.force);
        _animator.SetBool("isDead", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            gameObject.tag = "Untagged";
            _chance = 100 - (int) _characterController.Health;
            TakeDamage(_maxHealth);
            _bulletScript = other.GetComponent<Bullet>();
            _bulletScript.DestroyChance(_chance);
        }
    }

    private void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
            Die();
    }
}