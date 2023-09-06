using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
   private Rigidbody playerRb;
    private Animator animator;

    public Transform groundCheck;

    public LayerMask groundMask;

    public bool isLookLeft;

    private bool isGround;
    private bool isWalk;
    private bool isAirKick;

    public float speed;
    public float jumpForce;

    private float horizontal, vertical;

    [SerializeField] private Vector2 boxSize;

    [Header("Punch Settings")]
    [SerializeField] private float jabCounter;
    [SerializeField] private float jabDelay;


    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Movement();
       
    }

    private void FixedUpdate()
    {
        playerRb.velocity = new(horizontal * speed, playerRb.velocity.y, vertical * speed);      
        var hitColliders = Physics.OverlapBox(groundCheck.position, boxSize,Quaternion.identity, groundMask);
        isGround = hitColliders.Length > 0;

        if (isGround)
        {
            isAirKick = false;
        }
    }

    private void LateUpdate()
    {
        UpdateAnimator();
    }

    private void Movement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Jump();
        Punch();
        Kick();

        if (horizontal > 0 && isLookLeft)
        {
            Flip();
        }
        else if(horizontal < 0 && !isLookLeft)
        {
            Flip();
        }
    }

    private void Punch()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StopCoroutine(nameof(ComboPunch));
            StartCoroutine(ComboPunch());
            animator.SetBool("walk", false);

            if (jabCounter < 2)
            {
                animator.SetTrigger("punchA");
                jabCounter++;
            }
            else
            {
                animator.SetTrigger("punchB");
                jabCounter = 0;
            }      
        }
    }

    private void Kick()
    {
        if (Input.GetButtonDown("Fire2") && isGround)
        {
            animator.SetBool("walk", false);
            animator.SetTrigger("kick");
        }

        if (Input.GetButtonDown("Fire2") && !isGround)
        {
            isAirKick = true;
            animator.SetBool("walk", false);
            animator.SetTrigger("airKick");
        }
    }

    private IEnumerator ComboPunch()
    {
        yield return new WaitForSeconds(jabDelay);
        jabCounter = 0;
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            playerRb.AddForce(new(0, jumpForce, 0));
        }
    }

    private bool IsWalk()
    {     
        return isWalk = horizontal != 0 || vertical != 0;     
    }

    private void Flip()
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x;
        x *= -1;
        transform.localScale = new(x,transform.localScale.y,transform.localScale.z);

    }

    private void UpdateAnimator()
    {
        animator.SetBool("isGround", isGround);
        animator.SetBool("walk", IsWalk());
        animator.SetBool("onAir", isAirKick);
        animator.SetFloat("speedY", playerRb.velocity.y);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
    }


}
