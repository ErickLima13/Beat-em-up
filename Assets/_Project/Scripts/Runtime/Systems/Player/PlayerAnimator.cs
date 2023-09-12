using System.Reflection;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField] private GameObject hitBoxPrefab;

    [SerializeField] private Transform hitBoxPostionA, hitBoxPostionB;

    public GameObject temp;

    public ObjectPool objectPool;

    public Animator Animator
    {
        get; private set;
    }

    public bool IsAttack
    {
        get; private set;
    }

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (temp != null && playerController.idAttack == 3 && !playerController.IsGround)
        {
            temp.transform.position = hitBoxPostionB.position;
            print(temp.transform.position);
        }
    }

    private void LateUpdate()
    {
        UpdateAnimator();
    }

    public void EndAttack()
    {
        IsAttack = false;
    }

    public void SpawnHitBox()
    {
        //Time.timeScale = 0;
       

        if (playerController.idAttack != 3)
        {
            temp = objectPool.GetObject();
            temp.transform.position = hitBoxPostionA.position;
            temp.transform.localRotation = transform.localRotation;
            //Instantiate(hitBoxPrefab,hitBoxPostionA.position,transform.localRotation);

            // Destroy(temp,0.3f);

            objectPool.ReturnObject(temp, 0.3f);
           
        }
        else
        {
            temp = objectPool.GetObject();
            temp.transform.position = hitBoxPostionB.position;
            temp.transform.localRotation = transform.localRotation;

           // temp = Instantiate(hitBoxPrefab, hitBoxPostionB.position, transform.localRotation);
        }
    }

    public void SetIsAttack(bool value)
    {
        IsAttack = value;
    }

    private void UpdateAnimator()
    {
        Animator.SetBool("isGround", playerController.IsGround);
        Animator.SetBool("walk", playerController.Walking());
        Animator.SetBool("onAir", playerController.IsAirKick);
        Animator.SetFloat("speedY", playerController.playerRb.velocity.y);
    }
}
