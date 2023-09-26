using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnPlayer : MonoBehaviour
{
    [SerializeField] private int damage;

    [SerializeField] private GameObject hitBoxPrefab;

    [SerializeField] private Transform hitBoxPostion;

    public GameObject temp;

    public ObjectPool objectPool;

    public void SpawnHitBox()
    {
        temp = objectPool.GetObject();
        temp.transform.position = hitBoxPostion.position;
        temp.transform.localRotation = transform.localRotation;
        temp.GetComponent<Hitbox>().SetDamage(damage);
        objectPool.ReturnObject(temp, 0.5f);
    }
}
