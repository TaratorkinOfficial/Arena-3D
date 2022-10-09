using StarterAssets;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Zenject;

public class MainCharacterController : MonoBehaviour
{
    [SerializeField] private CharacterData characterData;
    [SerializeField] private GameObject bullet;
    private Camera _cam;
    private UiController _uiController;
    private NavMeshHit _hit;
    private float _walkRadius;
    private FirstPersonController _firstPerson;
    private List<GameObject> _enemies;
    private List<float> _enemiesDistance;
    private List<float> _enemiesDistanceSafe;
    private List<Vector3> _safePoint;
    private int _maxIndex;
    private int _minIndex;
    [NonSerialized] public float Health;
    private float _maxHealth;
    private float _damage;
    private float _force;
    private float _maxForce;
    public UnityAction TeleportEvent;

    [Inject]
    private void Construct([Inject(Id = "uiController")] UiController uiController)
    {
        _uiController = uiController;
    }

    private void Start()
    {
        _enemies= new List<GameObject>();
        _enemiesDistanceSafe = new List<float>();
        _safePoint = new List<Vector3>();
        _enemiesDistance = new List<float>();
        _walkRadius = 20;
        _cam = Camera.main;
        _firstPerson = GetComponent<FirstPersonController>();
        Health = characterData.health;
        _maxHealth = characterData.maxHealth;
        _force = characterData.force;
        _maxForce = characterData.maxForce;
    }
    private void ListUpdate()
    {
        _enemies.Clear();
        _safePoint.Clear();
        _enemiesDistanceSafe.Clear();
        _enemiesDistance.Clear();
        _enemies = GameObject.FindGameObjectsWithTag("enemy").ToList();
        for (int i = 0; i < 50; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * _walkRadius;
            NavMesh.SamplePosition(randomDirection, out _hit, _walkRadius, 1);
            for (int j = 0; j < _enemies.Count; j++)
            {
                _enemiesDistance.Add(Vector3.Distance(_hit.position, _enemies[j].transform.position));
                if (_enemiesDistance[j] == _enemiesDistance.Max<float>())
                {
                    _maxIndex = j;
                }
                if (_enemiesDistance[j] == _enemiesDistance.Min<float>())
                {
                    _minIndex = j;
                }
            }
            _safePoint.Add(_hit.position);
        }
        for (int j = 0; j < _safePoint.Count; j++)
        {
            _enemiesDistanceSafe.Add(Vector3.Distance(_safePoint[j], _enemies[_minIndex].transform.position));
            if (_enemiesDistanceSafe[j] == _enemiesDistanceSafe.Max<float>())
            {
                _maxIndex = j;
            }
        }
    }
    private void TeleportPlayer() 
    {
        TeleportEvent?.Invoke();
        _firstPerson.enabled = false;
        ListUpdate();
        transform.position = _safePoint[_maxIndex];
    }
    private void OnTriggerExit(Collider other)
    {
        string tag = other.tag;
        switch (tag)
        {
            case "barrier":
                TeleportPlayer();
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        _firstPerson.enabled = true;
    }

    private void Die()
    {
        _uiController.LoseUI();
    }
    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health < 0)
            Die();
        Health = Mathf.Clamp(Health, 0, _maxHealth);
        _uiController.HealthUpdate(Health);
    }
    public void MakeHealth(float rehealth)
    {
        Health += rehealth;
        Health = Mathf.Clamp(Health, 0, _maxHealth);
        _uiController.HealthUpdate(Health);
    }
    public void Shoot()
    {
        Instantiate(bullet, _cam.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height/2,_cam.nearClipPlane)), _cam.transform.rotation);
    }
    public void MakeKills(int kills)
    {
        _uiController.KillsUpdate(kills);
    }
    public void MakeForce(float strenght)
    {
        _force += strenght;
        _force = Mathf.Clamp(_force, 0, _maxForce);
        _uiController.ForceUpdate(_force);
    }

    public void TakeForceDamage(float damage)
    {
        _force -= damage;
        _force = Mathf.Clamp(_force, 0, _maxForce);
        _uiController.ForceUpdate(_force);
    }
}
