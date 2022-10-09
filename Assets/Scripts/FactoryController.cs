using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class FactoryController : MonoBehaviour
{
    [SerializeField] private RedEnemyFactory _redEnemyFactory;
    [SerializeField] private BlueEnemyFactory _blueEnemyFactory;
    private int _rand;
    [SerializeField] private float _highTime;
    [SerializeField] private float _lowTime;
    private List<GameObject> _enemies;
    private float timeSpawn;
    private MainCharacterController _characterController;

    [Inject]
    private void Construct([Inject(Id = "characterController")] MainCharacterController characterController)
    {
        _characterController = characterController;
    }
    private void Start()
    {
        _enemies = new List<GameObject>();
    }

    private void SpawnRed()
    {
        _redEnemyFactory.GetNewInstance();
        _enemies = GameObject.FindGameObjectsWithTag("enemy").ToList();
    }
    public void DestroyEnemies()
    {
        _characterController.MakeKills(_enemies.Count);
        for (int i = 0; i < _enemies.Count; i++)
        {
            Destroy(_enemies[i]);
        }
    }
    private void SpawnBlue()
    {
        _blueEnemyFactory.GetNewInstance();
    }
    private void Update()
    {
        if (timeSpawn <= 0)
        {
            if (_rand == 4)
            {
                SpawnBlue();
                _rand = 0;
            }
            else if (_rand>=0&& _rand<=3)
            {
                SpawnRed();
                _rand += 1;
            }
            if(_highTime > _lowTime) _highTime -= 1;
            timeSpawn = _highTime;
        }
        else
        {
            timeSpawn -= Time.deltaTime;
        }
    }
}
