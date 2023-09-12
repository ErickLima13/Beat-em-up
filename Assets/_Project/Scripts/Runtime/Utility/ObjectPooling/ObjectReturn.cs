using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReturn : MonoBehaviour
{
    public ObjectPool objectPool;

    private void Initialization()
    {
        objectPool = FindObjectOfType<ObjectPool>();
    }

    void Start()
    {
        Initialization();
    }

    private void OnDisable()
    {
        if(objectPool != null)
        {
            objectPool.ReturnObject(this.gameObject,0);
        }
    }

}
