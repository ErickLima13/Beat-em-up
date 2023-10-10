using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBase : MonoBehaviour
{
    #region Base
    [Header("base")]
    public EnemyState currentState;
    protected SpawnManager _spawnManager;
    protected PlayerController _playerController;
    protected Animator _animator;
    protected Rigidbody _enemyRb;
    protected float movHorizontal, movVertical;
    protected float posXplayer, posZplayer;
    protected Vector3 dirPlayer;
    [SerializeField] protected float speed;
    [SerializeField] protected float maxDistancePlayer;
    [SerializeField] protected float percStopAttack;
    [SerializeField] protected bool isLookLeft;
    [SerializeField] protected bool canHit;
    [SerializeField] protected bool isPositionLeft;
    protected IsVisible visible;
    public bool isVisible;
    protected bool isInPosition;
    #endregion

    #region Chase
    [Header("Chase")]
    [SerializeField] protected float delayTime;
    [SerializeField] protected float delayAttack;
    [SerializeField] protected float delayNewAttack;
    [SerializeField] protected float chaseTime;
    [SerializeField] protected bool canAttack;
    [SerializeField] protected bool isChase;
    #endregion

    [Header("Escape")]
    [SerializeField] protected float safeDistance;
    [SerializeField] protected float minTime, maxTime;
    protected float timeTemp;
    protected bool isWalk;
    protected float walkTime;

    [Header("Lackey")]
    #region Lackey
    public GameObject _bossScript;
    public bool isBoss;
    #endregion

    [Header("Boss")]
    #region Boss
    public bool iamBoss;
    #endregion

    private Status _status;

    [SerializeField] private GameObject soundHit;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _spawnManager = FindObjectOfType<SpawnManager>();
        _enemyRb = GetComponent<Rigidbody>();
        _status = GetComponent<Status>();   
        _animator = GetComponentInChildren<Animator>();
        visible = GetComponentInChildren<IsVisible>();

        _status.OnEnemyTakeHit += GetHit;
    }

    private void OnDestroy()
    {
        _status.OnEnemyTakeHit -= GetHit;
    }

    public virtual void Update()
    {
        if (isBoss && _spawnManager._currentGame != GameState.Battleboss)
        {
            return;
        }

        Movement();
    }

    private void Movement()
    {
        posXplayer = _playerController.transform.position.x;
        posZplayer = _playerController.transform.position.z;
        dirPlayer = _playerController.transform.position - transform.position;

        if (float.IsNaN(movHorizontal))
            movHorizontal = 0;
        if (float.IsNaN(movVertical))
            movVertical = 0;

        if (movHorizontal != 0 || movVertical != 0)
        {
            _animator.SetBool("walk", true);
        }
        else if (movHorizontal == 0 && movVertical == 0)
        {
            _animator.SetBool("walk", false);
        }

        if (currentState != EnemyState.BossAttack && iamBoss)
        {
            _enemyRb.velocity = new Vector3(movHorizontal * speed, _enemyRb.velocity.y, movVertical * speed);
        }

        if (!iamBoss)
        {
            _enemyRb.velocity = new Vector3(movHorizontal * speed, _enemyRb.velocity.y, movVertical * speed);
        }

        CheckIfIsInPosition();
        CheckIsVisible();
    }

    private void CheckIsVisible()
    {
        if (!isVisible)
        {
            isVisible = visible.Visible;
        }

        if (isVisible)
        {
            if (transform.position.x <= _playerController.limitXMinus.position.x)
            {
                transform.position = new(_playerController.limitXMinus.position.x, transform.position.y, transform.position.z);
            }
            else if (transform.position.x >= _playerController.limitXPlus.position.x)
            {
                transform.position = new(_playerController.limitXPlus.position.x, transform.position.y, transform.position.z);
            }
        }
    }

    private void CheckIfIsInPosition()
    {
        if (isBoss && !isInPosition)
        {
            if (isPositionLeft && transform.position.x > posXplayer)
            {
                currentState = EnemyState.Positioning;
            }
            else if (!isPositionLeft && transform.position.x < posXplayer)
            {
                currentState = EnemyState.Positioning;
            }

            if (isPositionLeft && transform.position.x < posXplayer)
            {
                isInPosition = true;
            }
            else if (!isPositionLeft && transform.position.x > posXplayer)
            {
                isInPosition = true;
            }
        }
    }


    #region States

    protected void Positioning()
    {
        if (isPositionLeft)
        {
            movHorizontal = -1;
        }
        else
        {
            movHorizontal = 1;
        }


        if (isPositionLeft && transform.position.x < _playerController.transform.position.x && MathF.Abs(dirPlayer.x) >= safeDistance)
        {
            movHorizontal = 0;
        }
        else if (!isPositionLeft && transform.position.x > _playerController.transform.position.x && MathF.Abs(dirPlayer.x) >= safeDistance)
        {
            movHorizontal = 0;
        }

        FlipController();
    }

    protected void Patrol()
    {
        float perc = safeDistance * 0.5f;

        if (MathF.Abs(dirPlayer.x) <= perc)
        {
            ChangeState(EnemyState.Chase);
            if (!isChase)
            {
                isChase = true;
                StartCoroutine(nameof(EndChase));
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

    protected void Chase()
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
                    StartCoroutine(nameof(Attack));
                }
            }
        }
        else if (MathF.Abs(dirPlayer.x) > maxDistancePlayer)
        {
            StopAttack();
        }
    }

    protected void Escape()
    {
        FlipController();

        if (MathF.Abs(dirPlayer.x) <= safeDistance)
        {
            movHorizontal = dirPlayer.x / Mathf.Abs(dirPlayer.x) * -1;
            timeTemp += Time.deltaTime;
            if (timeTemp >= Random.Range(1, 3))
            {
                movVertical = Random.Range(-1, 2);
            }
        }
        else
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    #endregion

    private IEnumerator EndChase()
    {
        yield return new WaitForSeconds(chaseTime);
        isChase = false;
        ChangeState(EnemyState.Escape);
    }

    public void ChangeState(EnemyState newState)
    {
        if (isBoss && isInPosition)
        {
            currentState = newState;
        }
        else if (!isBoss)
        {
            currentState = newState;
        }
    }

    protected void FlipController()
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
        ChangeState(EnemyState.Chase);

        if (Random.Range(0, 100) <= percStopAttack)
        {
            StopAttack();
        }

        StartCoroutine(nameof(DelayHit));
    }

    private IEnumerator DelayHit()
    {
        soundHit.SetActive(true);
        canHit = true;
        yield return new WaitForSeconds(delayTime);
        canHit = false;
        soundHit.SetActive(false);
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(delayAttack);
        _animator.SetTrigger("attack");
        yield return new WaitForSeconds(delayNewAttack);
        canAttack = false;
    }

    private void StopAttack()
    {
        StopCoroutine(nameof(Attack));
        canAttack = false;
    }

    public void SetIsPosition(bool value)
    {
        isPositionLeft = value;
    }

    public void SetBoss(GameObject value)
    {
        _bossScript = value;
    }
}
