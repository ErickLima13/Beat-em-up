using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : EnemyBase
{
    [SerializeField] private bool isOnTheGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Vector2 boxSize;

    public bool isJump;
    public GameObject impactPrefab;

    public override void Update()
    {
        base.Update();

        switch (currentState)
        {
            case EnemyState.Idle:
                movHorizontal = 0;
                movVertical = 0;
                FlipController();
                break;
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Escape:
                EscapeBoss();
                break;
        }
    }


    private void FixedUpdate()
    {
        var hitColliders = Physics.OverlapBox(groundCheck.position, boxSize, Quaternion.identity, groundMask);
        isOnTheGround = hitColliders.Length > 0;

        if (currentState.Equals(EnemyState.BossAttack) && isOnTheGround && isJump)
        {
            GameObject temp = Instantiate(impactPrefab, transform.position, transform.localRotation);
            Destroy(temp, 0.6f);
            isJump = false;
            ChangeState(EnemyState.Idle);
        }
    }

    private void EscapeBoss()
    {
        FlipController();
        movHorizontal = dirPlayer.x / Mathf.Abs(dirPlayer.x) * -1;
        timeTemp += Time.deltaTime;
        if (timeTemp >= Random.Range(1, 3))
        {
            movVertical = Random.Range(-1, 2);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
    }

    public void JumpAttack()
    {
        ChangeState(EnemyState.BossAttack);
        Vector3 force = new(Mathf.Clamp(dirPlayer.x, -1, 1) * 100, 150, Mathf.Clamp(dirPlayer.z, -1, 1) * 40);
        _enemyRb.AddForce(force);
    }
}

