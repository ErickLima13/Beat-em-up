using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    public bool IsGround
    {
        get; private set;
    }

    public bool IsWalk
    {
        get; private set;
    }

    public bool IsOnTheGround
    {
        get; private set;
    }

    public bool IsAirKick
    {
        get; private set;
    }

    public Rigidbody playerRb;
    private PlayerAnimator playerAnimator;

    public Transform groundCheck;

    public LayerMask groundMask;

    public bool isLookLeft;

    public float speed;
    public float jumpForce;

    private float horizontal, vertical;

    [SerializeField] private Vector2 boxSize;

    [Header("Punch Settings")]
    [SerializeField] private float jabCounter;
    [SerializeField] private float jabDelay;

    public int idAttack;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    private void Update()
    {
        Movement();
    }

    private void FixedUpdate()
    {
        playerRb.velocity = new(horizontal * speed, playerRb.velocity.y, vertical * speed);
        var hitColliders = Physics.OverlapBox(groundCheck.position, boxSize, Quaternion.identity, groundMask);
        IsGround = hitColliders.Length > 0;

        if (IsGround)
        {
            IsAirKick = false;

            if (playerAnimator.temp != null && idAttack == 3)
            {
                Destroy(playerAnimator.temp);
            }
        }
    }

    private void Movement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (playerAnimator.IsAttack)
        {
            horizontal = 0;
            vertical = 0;
        }

        Jump();
        Kick();
        Punch();


        if (horizontal > 0 && isLookLeft)
        {
            Flip();
        }
        else if (horizontal < 0 && !isLookLeft)
        {
            Flip();
        }
    }

    private void Punch()
    {
        if (Input.GetButtonDown("Fire1") && !playerAnimator.IsAttack)
        {
            playerAnimator.SetIsAttack(true);

            StopCoroutine(nameof(ComboPunch));
            StartCoroutine(ComboPunch());
            playerAnimator.Animator.SetBool("walk", false);

            if (jabCounter < 2)
            {
                idAttack = 0;
                playerAnimator.Animator.SetTrigger("punchA");
                jabCounter++;
            }
            else
            {
                idAttack = 1;
                playerAnimator.Animator.SetTrigger("punchB");
                jabCounter = 0;
            }
        }
    }

    private void Kick()
    {
        if (Input.GetButtonDown("Fire2") && IsGround && !playerAnimator.IsAttack)
        {
            idAttack = 2;
            playerAnimator.SetIsAttack(true);

            playerAnimator.Animator.SetBool("walk", false);
            playerAnimator.Animator.SetTrigger("kick");
        }

        if (Input.GetButtonDown("Fire2") && !IsGround)
        {
            idAttack = 3;
            IsAirKick = true;
            playerAnimator.Animator.SetBool("walk", false);
            playerAnimator.Animator.SetTrigger("airKick");
           
        }
    }

    private IEnumerator ComboPunch()
    {
        yield return new WaitForSeconds(jabDelay);
        jabCounter = 0;
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGround)
        {
            playerRb.AddForce(new(0, jumpForce, 0));
        }
    }

    public bool Walking()
    {
        return IsWalk = horizontal != 0 || vertical != 0;
    }

    private void Flip()
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x;
        x *= -1;
        transform.localScale = new(x, transform.localScale.y, transform.localScale.z);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
    }


}
