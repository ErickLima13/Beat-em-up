using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Gameplay,
    Cutscene,
    Battleboss
}

public class SpawnManager : MonoBehaviour
{
    public GameState _currentGame;

    public Camera _camera;
    public PlayerController _playerController;
    private SceneryManager _sceneryManager;

    public List<GameObject> _enemiesList;
    public List<GameObject> _enemiesActive;
    public int _idEnemy;
    public float _interval;

    public float _outCam;

    public float minTime, maxTime;
    private float tempTime, routineTime;
    private bool isActive;

    public Transform bossPoint;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _sceneryManager = GetComponent<SceneryManager>();
        _camera = Camera.main;
    }

    private void Update()
    {
        ControlEnemies();
    }

    private void ControlEnemies()
    {
        if (_currentGame == GameState.Gameplay)
        {
            if (_enemiesActive.Count > 0)
            {
                if (!isActive)
                {
                    routineTime = Random.Range(minTime, maxTime);
                    isActive = true;
                }

                tempTime += Time.deltaTime;

                if (tempTime >= routineTime)
                {
                    int id = Random.Range(0, _enemiesActive.Count);

                    foreach (GameObject enemy in _enemiesActive)
                    {
                        int percMov = Random.Range(0, 99);

                        if (percMov < 50)
                        {
                            enemy.SendMessage("ChangeState", EnemyState.Escape, SendMessageOptions.DontRequireReceiver);
                        }
                        else
                        {
                            enemy.SendMessage("ChangeState", EnemyState.Patrol, SendMessageOptions.DontRequireReceiver);
                        }
                    }

                    _enemiesActive[id].SendMessage("ChangeState", EnemyState.Chase, SendMessageOptions.DontRequireReceiver);
                    tempTime = 0;
                    isActive = false;
                }
            }
        }

        
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

        if (_idEnemy < _enemiesList.Count)
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

    public void BattleBoss()
    {
        _currentGame = GameState.Cutscene;
    }
}
