using UnityEngine;

public class SceneryManager : MonoBehaviour
{
    private PlayerController _player;
    private Camera _camera;

    [SerializeField] private bool _canFollow;
    [SerializeField] private float _speedCam = 1;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        MoveCam();
    }

    private void MoveCam()
    {
        if (_canFollow && _camera.transform.position.x < _player.transform.position.x)
        {
            Vector3 posCam = _camera.transform.position;
            Vector3 posTarget = new Vector3(_player.transform.position.x,posCam.y,posCam.z);   
            
            _camera.transform.position =  Vector3.Lerp(posCam, posTarget, _speedCam * Time.deltaTime);
        }
    }

    public void SetFollow(bool follow)
    {
        _canFollow = follow;
    }

}
