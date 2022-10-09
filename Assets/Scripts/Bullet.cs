using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    private Transform _bullet;
    private int _random;
    private FactoryController _factoryController;
    private List<GameObject> _enemies;
    private List<float> _enemiesDistance;
    private int _minIndex;
    private bool _rikoshet;
    private MainCharacterController _characterController;
    private int _hitCount;
    [SerializeField] private CharacterData characterData;
    
    [Inject]
    private void Construct([Inject(Id = "characterController")] MainCharacterController characterController)
    {
        _characterController = characterController;
    }

    private void Start()
    {
        _enemies = new List<GameObject>();
        _enemiesDistance = new List<float>();
        _bullet = gameObject.transform;
    }
    public void DestroyChance(int chance)
    {
        if ( _hitCount ==1)
        {
            _characterController.MakeHealth(characterData.maxHealth/2);
            _characterController.MakeForce(Random.Range(10,50));
            Destroy(gameObject);
            return;
        }
        _random = chance;
        int range1 = Random.Range(0, _random+1);
        if (range1 == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            int range = Random.Range(0, 2);
            if (range == 0)
            {
                _enemies = GameObject.FindGameObjectsWithTag("enemy").ToList();
                _enemiesDistance = new List<float>();
                for (int i = 0; i < _enemies.Count; i++)
                {
                    _enemiesDistance.Add(Vector3.Distance(_bullet.position, _enemies[i].transform.position));
                    
                    if (_enemiesDistance[i] == _enemiesDistance.Min<float>())
                    {
                        _minIndex = i;
                    }
                }
                _rikoshet = true;
                _hitCount += 1;
            }
        }  
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("barrier"))
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (_rikoshet)
        {
            _bullet.position = 
                Vector3.MoveTowards(_bullet.position, _enemies[_minIndex].transform.position, .3f);
        }
        else
        {
            _bullet.position += transform.forward* .3f;
        }
    }
}
