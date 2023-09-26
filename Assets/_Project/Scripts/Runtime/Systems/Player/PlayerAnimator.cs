using System.Collections;
using System.Reflection;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private PlayerController playerController;
    private Status _status;

    [SerializeField] private float delayTime;

    [SerializeField] private GameObject hitBoxPrefab;

    [SerializeField] private Transform hitBoxPostionA, hitBoxPostionB;

    public GameObject temp;

    public ObjectPool objectPool;

    private bool canHit;

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
        _status = GetComponentInParent<Status>();
        Animator = GetComponent<Animator>();

        _status.OnDie += Die;
        _status.OnPlayerDamage += GetHit;
    }

    private void OnDestroy()
    {
        _status.OnDie -= Die;
        _status.OnPlayerDamage -= GetHit;
    }

    private void Update()
    {
        if (temp != null && playerController.idAttack == 3 && !playerController.IsGround)
        {
            temp.transform.position = hitBoxPostionB.position;
        }
    }

    private void LateUpdate()
    {
        if (playerController.isAlive)
        {
            UpdateAnimator();
        }
    }

    public void EndAttack()
    {
       StartCoroutine(StopAttack());
    }

    private IEnumerator StopAttack()
    {
        yield return new WaitForEndOfFrame();
        SetIsAttack(false);
    }

    public void SpawnHitBox()
    {
        if (playerController.idAttack != 3)
        {
            temp = objectPool.GetObject();
            temp.transform.position = hitBoxPostionA.position;
            temp.transform.localRotation = transform.localRotation;
            temp.GetComponent<Hitbox>().SetIdvalue(playerController.idAttack);
            objectPool.ReturnObject(temp, 0.5f);    
        }
        else
        {
            temp = objectPool.GetObject();
            temp.transform.position = hitBoxPostionB.position;
            temp.transform.localRotation = transform.localRotation;
            temp.GetComponent<Hitbox>().SetIdvalue(playerController.idAttack);
        }
    }

    public void SetIsAttack(bool value)
    {
        IsAttack = value;
    }

    private void Die()
    {
        playerController.isAlive = false;
        Animator.SetTrigger("die");
    }

    public void GetHit()
    {
        if (playerController.isAlive)
        {
            SetIsAttack(false);
            Animator.SetTrigger("hit");
            StartCoroutine(nameof(DelayHit));
        }   
    }

    private IEnumerator DelayHit()
    {
        canHit = true;
        yield return new WaitForSeconds(delayTime);
        canHit = false;
    }

    private void UpdateAnimator()
    {
        Animator.SetBool("isGround", playerController.IsGround);
        Animator.SetBool("walk", playerController.Walking());
        Animator.SetBool("onAir", playerController.IsAirKick);
        Animator.SetFloat("speedY", playerController.playerRb.velocity.y);
    }
}
