using System;
using UnityEngine;

public class SceneryManager : MonoBehaviour
{
    public event Action OnBattleBoss;

    private SpawnManager _spawnManager;

    private PlayerController _player;
    private Camera _camera;

    [SerializeField] private bool _canFollow;
    [SerializeField] private float _speedCam = 1;

    private void Start()
    {
        _spawnManager = GetComponent<SpawnManager>();
        _player = FindObjectOfType<PlayerController>();
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        MoveCam();
    }

    private void MoveCam()
    {
        Vector3 posCam = _camera.transform.position;

        if (_canFollow && _camera.transform.position.x < _player.transform.position.x && _spawnManager._currentGame == GameState.Gameplay)
        {
           
            Vector3 posTarget = new Vector3(_player.transform.position.x,posCam.y,posCam.z);   
            
            _camera.transform.position =  Vector3.Lerp(posCam, posTarget, _speedCam * Time.deltaTime);
        }

        if (_spawnManager._currentGame == GameState.Cutscene)
        {
            Vector3 posTarget = new Vector3(_spawnManager.bossPoint.transform.position.x, posCam.y, posCam.z);
            _camera.transform.position = Vector3.MoveTowards(posCam, posTarget, 0.3f * Time.deltaTime);

            if(posCam ==  posTarget)
            {
                _spawnManager._currentGame = GameState.Battleboss;
                OnBattleBoss?.Invoke();
            }
        }
    }

    public void SetFollow(bool follow)
    {
        _canFollow = follow;
    }

}
