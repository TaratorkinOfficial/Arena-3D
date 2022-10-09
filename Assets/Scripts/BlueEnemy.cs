using UnityEngine;
using Zenject;

public class BlueEnemy : MonoBehaviour
{
    [SerializeField] private CharacterData _characterData;
    private Animator _animator;
    private MainCharacterController _characterController;
    private float _health;
    private float _maxHealth;
    private int _chance;
    private Bullet _bulletScript;

    [Inject]
    private void Construct([Inject(Id = "characterController")] MainCharacterController characterController)
    {
        _characterController = characterController;
    }
    private void Start()
    {
        _characterController = FindObjectOfType<MainCharacterController>();
        _health = _characterData.health;
        _maxHealth = _characterData.maxHealth;
        _animator = GetComponent<Animator>();
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
    private void Die()
    {
        _characterController.MakeKills(1);
        _characterController.MakeForce(50);
        _animator.SetBool("isDead", true);
    }
    private void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
            Die();
    }
}