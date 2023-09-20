using System;
using UnityEngine;

public class Status : MonoBehaviour
{
    public event Action OnDie;

    private UIManager _uIManager;

    [SerializeField] private GameObject hitPrefab;

    [SerializeField] private int maxLife;
    [SerializeField] private string charName;
    private int currentLife;

    private void Start()
    {
        _uIManager = FindObjectOfType<UIManager>();
        currentLife = maxLife;
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

        _uIManager.UpdateHpBar(perc,charName);

        if (currentLife <= 0)
        {
            OnDie?.Invoke();
            Destroy(gameObject, 0.1f);
        }
    }
}