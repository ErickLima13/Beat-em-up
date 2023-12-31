﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Status : MonoBehaviour
{
    public event Action OnDie;

    public event Action OnEnemyTakeHit;
    public event Action OnPlayerTakeHit;

    private UIManager _uIManager;
    private SpawnManager _spawnManager;

    [SerializeField] private GameObject hitPrefab;

    [SerializeField] private int maxLife;
    [SerializeField] private string charName;
    [SerializeField] private string[] randomNames;
    [SerializeField] private Sprite charPicture;
    [SerializeField] private bool isEnemy;
    [SerializeField] private bool isLackey;
    private int currentLife;

    private GameObject _boss;

    private void Start()
    {
        _uIManager = FindObjectOfType<UIManager>();
        _spawnManager = FindObjectOfType<SpawnManager>();
        currentLife = maxLife;

        if (isEnemy)
        {
            charName = randomNames[Random.Range(0, randomNames.Length)];

        }

        if (GetComponent<EnemyA>() != null)
        {
            _boss = GetComponent<EnemyA>()._bossScript;
        }
    }

    public void HealthChange(int value)
    {
        currentLife -= value;

        float perc = currentLife / (float)maxLife;

        if (perc < 0)
        {
            perc = 0;
        }

        if (isEnemy)
        {
            _uIManager.UpdateHpBar(perc, charName, charPicture);
            OnEnemyTakeHit?.Invoke();
        }
        else
        {
            _uIManager.UpdateHpBarPlayer(perc);
            OnPlayerTakeHit?.Invoke();
        }

        CheckDeath();

    }

    private void CheckDeath()
    {
        if (currentLife <= 0)
        {
            if (isEnemy)
            {
                if (isLackey)
                {
                    _boss.GetComponent<BossController>().RemoveLackeys(gameObject);
                }
                else
                {
                    _spawnManager.RemoveEnemies(gameObject);
                }

                Destroy(gameObject, 0.3f);
            }
            else
            {
                OnDie?.Invoke();
            }
        }
    }
}