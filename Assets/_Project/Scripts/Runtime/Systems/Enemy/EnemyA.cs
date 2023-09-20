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

    #region Base
    [Header("base")]
    [SerializeField] private float speed;
    [SerializeField] private float maxDistancePlayer;
    [SerializeField] private float percStopAttack;
    [SerializeField] private bool isLookLeft;
    [SerializeField] private bool canHit;
    #endregion

    #region Chase
    [Header("Chase")]
    [SerializeField] private float delayTime;
    [SerializeField] private float delayAttack;
    [SerializeField] private float delayNewAttack;
    [SerializeField] private float chaseTime;
    [SerializeField] private bool canAttack;
    [SerializeField] private bool isChase;
    #endregion

    [Header("Escape")]
    [SerializeField] private float safeDistance;
    [SerializeField] private float minTime,maxTime;
    private float timeTemp;
    private bool isWalk;
    private float walkTime;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        enemyRb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
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

        Movement();
    }

    private void Movement()
    {
        posXplayer = playerController.transform.position.x;
        posZplayer = playerController.transform.position.z;
        dirPlayer = playerController.transform.position - transform.position;

        if (float.IsNaN(movHorizontal))
            movHorizontal = 0;
        if (float.IsNaN(movVertical))
            movVertical = 0;

        enemyRb.velocity = new Vector3(movHorizontal * speed, enemyRb.velocity.y, movVertical * speed);
    }


    #region States
    private void Patrol()
    {
        float perc = safeDistance * 0.5f;

        if (MathF.Abs(dirPlayer.x) <= perc)
        {
            currentState = EnemyState.Chase;
            if (!isChase)
            {
                isChase = true;
                StartCoroutine(EndChase());
            }
        }

        timeTemp += Time.deltaTime;

        if (!isWalk)
        {
            movHorizontal = Random.Range(-1, 2);
            movVertical = Random.Range(-1, 2);
            walkTime = Random.Range(minTime, maxTime);
            isWalk = true;

            if (movHorizontal < 0 && !isLookLeft)
            {
                Flip();
            }
            else if (movHorizontal > 0 && isLookLeft)
            {
                Flip();
            }
        }

        if (timeTemp >= walkTime)
        {
            timeTemp = 0;
            isWalk = false;
        } 
    }

    private void Chase()
    {
        FlipController();

        if (!canHit && !canAttack)
        {
            movHorizontal = dirPlayer.x / Mathf.Abs(dirPlayer.x);
            movVertical = dirPlayer.z / Mathf.Abs(dirPlayer.z);
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
    }

    private void Escape()
    {
        FlipController();

        if (MathF.Abs(dirPlayer.x) <= safeDistance)
        {
            movHorizontal = dirPlayer.x / Mathf.Abs(dirPlayer.x) * -1;
            movVertical = 0;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }

    #endregion

    private IEnumerator EndChase()
    {
        print("chamei aqui");
        yield return new WaitForSeconds(chaseTime);
        print("esperei o tempo");
        isChase = false;
        currentState = EnemyState.Escape;
    }

    private void FlipController()
    {
        if (transform.position.x < posXplayer && isLookLeft)
        {
            Flip();
        }
        else if (transform.position.x > posXplayer && !isLookLeft)
        {
            Flip();
        }
    }

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
