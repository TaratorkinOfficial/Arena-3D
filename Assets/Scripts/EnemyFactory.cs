using UnityEngine;
using UnityEngine.AI;

public class EnemyFactory<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    private Vector3 _pointSpawn;
    private float _walkRadius = 20;
    private NavMeshHit _hit;

    public T GetNewInstance()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _walkRadius;
        NavMesh.SamplePosition(randomDirection, out _hit, _walkRadius, 1);
        _pointSpawn = _hit.position;
        return Instantiate(_prefab, _pointSpawn, Quaternion.identity);
    }
}
