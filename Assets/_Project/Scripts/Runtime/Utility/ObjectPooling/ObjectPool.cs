using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectPrefab;

    [SerializeField] private Queue<GameObject> objectPool = new Queue<GameObject>();

    [SerializeField] private Transform parent;

    public int poolSize = 10;

    public int index;

    private void Initialization()
    {
        for(int i = 0; i < poolSize; i++) 
        {
            GameObject obj = Instantiate(objectPrefab);
            objectPool.Enqueue(obj);
            obj.SetActive(false);
            obj.transform.parent = parent;
        }
    }

    void Start()
    {
        Initialization();
    }

    public GameObject GetObject() // coloca o objeto na fila, se nao cria um objeto.
    {
        //GameObject obj = objectPool.Dequeue();
        //obj.SetActive(true);
        //index++;
        //return obj;

        if (objectPool.Count > 0)
        {
            GameObject obj = objectPool.Dequeue();
            obj.SetActive(true);
            index++;
            return obj;
        }

        else
        {
            GameObject obj = Instantiate(objectPrefab);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj,float value) // retorna o objeto para fila,
    {
        StartCoroutine(TimeToReturn(obj,value));
    }

    private IEnumerator TimeToReturn(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        objectPool.Enqueue(obj);
        obj.SetActive(false);
    }
}
