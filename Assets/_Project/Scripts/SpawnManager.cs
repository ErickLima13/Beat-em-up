using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Camera _camera;
    private PlayerController _playerController;
    private SceneryManager _sceneryManager;

    public List<GameObject> _enemiesList;
    public List<GameObject> _enemiesActive;
    public int _idEnemy;
    public float _interval;

    [SerializeField] private float _outCam;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _sceneryManager = GetComponent<SceneryManager>();
        _camera = Camera.main;
    }

    public void StartSpawn()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        int rand = Random.Range(0, 99);
        Vector3 posIns = Vector3.zero;

        if (rand < 50)
        {
            posIns = new Vector3(_camera.transform.position.x + _outCam * -1, 0, _playerController.transform.position.z);
        }
        else
        {
            posIns = new Vector3(_camera.transform.position.x + _outCam, 0, _playerController.transform.position.z);
        }

        GameObject temp = Instantiate(_enemiesList[_idEnemy], posIns, transform.localRotation);
        _enemiesActive.Add(temp);
        _idEnemy++;

        yield return new WaitForSeconds(_interval);

        if(_idEnemy < _enemiesList.Count)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    public void RemoveEnemies(GameObject enemy)
    {
       _enemiesActive.Remove(enemy);

        if (_enemiesActive.Count == 0 && _idEnemy >= _enemiesList.Count)
        {
            _sceneryManager.SetFollow(true);
        }
    }
}
