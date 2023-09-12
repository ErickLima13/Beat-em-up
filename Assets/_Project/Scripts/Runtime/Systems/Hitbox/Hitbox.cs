using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private GameObject popUp;

    [SerializeField] private int damageValue;

    private bool isHit;

    private void OnTriggerEnter(Collider other)
    {
        if (!isHit)
        {
            isHit = true;
            GameObject temp = Instantiate(popUp,transform.position,transform.localRotation);
            Destroy(temp,0.3f);
        }
    }
}
