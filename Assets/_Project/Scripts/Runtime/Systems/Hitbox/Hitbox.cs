using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private GameObject popUp;

    [SerializeField] private int idValue;

    [SerializeField] private int damage;

    private bool isHit;

    private void OnTriggerEnter(Collider col)
    {
        if (!isHit)
        {
            switch (idValue)
            {
                case 0:
                    damage = 1;
                    break;
                case 1:
                    damage = 3;
                    break;
                case 2:
                    damage = 2;
                    break;
                case 3:
                    damage = 1;
                    break;
            }

            col.gameObject.SendMessage("GetHit", SendMessageOptions.DontRequireReceiver);

            if (col.gameObject.TryGetComponent(out Status status))
            {
                status.HealthChange(damage);
            }

            isHit = true;
            GameObject temp = Instantiate(popUp, transform.position, transform.localRotation);
            Destroy(temp, 0.3f);
        }
    }

    public void SetDamage(int value)
    {
        idValue = value;
    }

    private void OnDisable()
    {
        isHit = false;
    }
}
