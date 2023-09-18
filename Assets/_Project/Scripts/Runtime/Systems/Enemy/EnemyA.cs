using Unity.Mathematics;
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

    [SerializeField] private float speed;

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

        movHorizontal = dirPlayer.x / Mathf.Abs(dirPlayer.x);

        movVertical = dirPlayer.z / Mathf.Abs(dirPlayer.z);
        
        enemyRb.velocity = new Vector3(movHorizontal * speed,enemyRb.velocity.y,movVertical * speed);

        if (transform.position.x < posXplayer && isLookLeft)
        {
            Flip();
        }
        else if(transform.position.x > posXplayer && !isLookLeft)
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
}
