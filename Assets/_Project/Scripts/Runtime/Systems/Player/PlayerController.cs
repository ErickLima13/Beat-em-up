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

        isGround = Physics.Linecast(transform.position + new Vector3(0,0.1f,0), groundCheck.position, groundMask);

        Debug.DrawLine(transform.position + new Vector3(0, 0.1f, 0), groundCheck.position,Color.red);
 
    }

    private void LateUpdate()
    {
        UpdateAnimator();
    }

    private void Movement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");


        
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
    }




}
