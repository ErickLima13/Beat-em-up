using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Status : MonoBehaviour
{
    public event Action OnDie;
  

    private UIManager _uIManager;
    private SpawnManager _spawnManager;

    [SerializeField] private GameObject hitPrefab;

    [SerializeField] private int maxLife;
    [SerializeField] private string charName;
    [SerializeField] private string[] randomNames;
    [SerializeField] private Sprite charPicture;
    [SerializeField] private bool isEnemy;
    private int currentLife;


    private void Start()
    {
        _uIManager = FindObjectOfType<UIManager>();
        _spawnManager = FindObjectOfType<SpawnManager>();
        currentLife = maxLife;

        if (isEnemy)
        {
            charName = randomNames[Random.Range(0, randomNames.Length)];
        }
       
    }

    public void HealthChange(int value)
    {
        //GameObject temp = Instantiate(hitPrefab, transform.position, Quaternion.identity);
        //Destroy(temp, 0.5f);
        currentLife -= value;

        float perc = currentLife / (float)maxLife;

        if (perc < 0)
        {
            perc = 0;
        }

        if(isEnemy)
        {
            _uIManager.UpdateHpBar(perc, charName);
        }

        if (currentLife <= 0)
        {
            if (isEnemy)
            {
                _spawnManager.RemoveEnemies(gameObject);
                Destroy(gameObject, 0.3f);
            }
            else
            {
                OnDie?.Invoke();
            } 
        }
    }
}