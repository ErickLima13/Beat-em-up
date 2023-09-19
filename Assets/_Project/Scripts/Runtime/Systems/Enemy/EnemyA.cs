using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyState
{
    Patrol,
    Chase,
    Escape
}


public class EnemyA : MonoBehaviour
{
    public EnemyState currentState;

    private PlayerController playerController;
    private Animator animator;
    private Rigidbody enemyRb;

    private float movHorizontal, movVertical;
    private float posXplayer, posZplayer;

    private Vector3 dirPlayer;

    public bool isLookLeft;
    public bool canHit;
    public bool canAttack;

    [SerializeField] private float speed;
    [SerializeField] private float delayTime;
    [SerializeField] private float delayAttack;
    [SerializeField] private float delayNewAttack;
    [SerializeField] private float maxDistancePlayer;
    [SerializeField] private float percStopAttack;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        enemyRb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        posXplayer = playerController.transform.position.x;
        posZplayer = playerController.transform.position.z;

        dirPlayer = playerController.transform.position - transform.position;

        if (!canHit && !canAttack)
        {
            movHorizontal = dirPlayer.x / Mathf.Abs(dirPlayer.x);
            movVertical = dirPlayer.z / Mathf.Abs(dirPlayer.z);

            if (float.IsNaN(movHorizontal))
                movHorizontal = 0;
            if (float.IsNaN(movVertical))
                movVertical = 0;
        }
        else
        {
            movHorizontal = 0;
            movVertical = 0;
        }

        if (MathF.Abs(dirPlayer.x) <= maxDistancePlayer && MathF.Abs(dirPlayer.z) <= 0.2f)
        {
            movHorizontal = 0;
            if (!canAttack)
            {
                if (!canHit)
                {
                    canAttack = true;
                    StartCoroutine(Attack());
                }
            }
        }
        else if (MathF.Abs(dirPlayer.x) > maxDistancePlayer)
        {
            StopAttack();
        }

        enemyRb.velocity = new Vector3(movHorizontal * speed, enemyRb.velocity.y, movVertical * speed);

        if (transform.position.x < posXplayer && isLookLeft)
        {
            Flip();
        }
        else if (transform.position.x > posXplayer && !isLookLeft)
        {
            Flip();
        }

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Escape:
                Escape();
                break;
        }
    }


    #region States
    private void Patrol()
    {

    }

    private void Chase()
    {

    }

    private void Escape()
    {

    }

    #endregion

    private void Flip()
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x;
        x *= -1;
        transform.localScale = new(x, transform.localScale.y, transform.localScale.z);
    }

    public void GetHit()
    {
        if (Random.Range(0, 100) <= percStopAttack)
        {
            StopAttack();
        }

        StartCoroutine(DelayHit());
    }

    private IEnumerator DelayHit()
    {
        canHit = true;
        yield return new WaitForSeconds(delayTime);
        canHit = false;
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(delayAttack);
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(delayNewAttack);
        canAttack = false;
    }

    private void StopAttack()
    {
        StopCoroutine(nameof(Attack));
        canAttack = false;
    }
}
