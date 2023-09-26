using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnPlayer : MonoBehaviour
{
    private Transform _manager;
    private GameObject temp;
    private ObjectPool objectPool;

    [SerializeField] private int damage;
    [SerializeField] private GameObject hitBoxPrefab;
    [SerializeField] private Transform hitBoxPostion;

    private void Start()
    {
        _manager = FindObjectOfType<SpawnManager>().transform;
        objectPool = _manager.GetComponentInChildren<ObjectPool>();
    }

    public void SpawnHitBox()
    {
        temp = objectPool.GetObject();
        temp.transform.position = hitBoxPostion.position;
        temp.transform.localRotation = transform.localRotation;
        temp.GetComponent<Hitbox>().SetDamage(damage);
        objectPool.ReturnObject(temp, 0.5f);
    }
}
