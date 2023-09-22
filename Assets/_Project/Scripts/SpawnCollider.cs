using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollider : MonoBehaviour
{
    private SceneryManager _sceneryManager;
    private SpawnManager _spawnManager;

    private bool _isWorking;

    [SerializeField] private GameObject[] _enemiesPrefab;
    [SerializeField] private int _qtdEnemies;
    [SerializeField] private float _spawnInterval;

    private void Start()
    {
        _sceneryManager = FindObjectOfType<SceneryManager>();
        _spawnManager = FindObjectOfType<SpawnManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController controller) && !_isWorking)
        {
            ColliderWithPlayer();
        }
    }

    private void ColliderWithPlayer()
    {
        _isWorking = true;
        _sceneryManager.SetFollow(false);
        _spawnManager._enemiesList.Clear();
        _spawnManager._idEnemy = 0;
        _spawnManager._interval = _spawnInterval;

        for (int i = 0; i < _qtdEnemies; i++)
        {
            _spawnManager._enemiesList.Add(_enemiesPrefab[Random.Range(0, _enemiesPrefab.Length)]);
        }

        _spawnManager.StartSpawn();
    }
}
