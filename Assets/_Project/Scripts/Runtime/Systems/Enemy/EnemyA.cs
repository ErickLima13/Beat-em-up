using System.Collections;
using UnityEngine;

public class EnemyA : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;
    private Rigidbody enemyRb;

    private float movHorizontal, movVertical;
    private float posXplayer, posZplayer;

    private Vector3 dirPlayer;

    public bool isLookLeft;
    public bool canHit;


    [SerializeField] private float speed;
    [SerializeField] private float delayTime;

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

        if (!canHit)
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

        enemyRb.velocity = new Vector3(movHorizontal * speed, enemyRb.velocity.y, movVertical * speed);

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
        StartCoroutine(DelayHit());
    }

    private IEnumerator DelayHit()
    {
        canHit = true;
        yield return new WaitForSeconds(delayTime);
        canHit = false;
    }
}
