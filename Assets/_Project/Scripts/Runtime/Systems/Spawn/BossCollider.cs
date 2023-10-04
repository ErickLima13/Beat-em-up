using UnityEngine;

public class BossCollider : MonoBehaviour
{
    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = FindObjectOfType<SpawnManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_spawnManager._currentGame != GameState.Gameplay)
        {
            return;
        }

        if (other.gameObject.TryGetComponent(out PlayerController controller))
        {
            ColliderWithPlayer();
        }
    }

    private void ColliderWithPlayer()
    {
        print("COLID");
        _spawnManager.BattleBoss();
    }
}
