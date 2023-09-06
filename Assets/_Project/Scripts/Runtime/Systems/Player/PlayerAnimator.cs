using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private PlayerController playerController;
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
        Animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        UpdateAnimator();
    }

    public void EndAttack()
    {
        IsAttack = false;
    }

    public void SpawnHitBox()
    {

    }

    public void SetIsAttack(bool value)
    {
        IsAttack = value;
    }

    private void UpdateAnimator()
    {
        Animator.SetBool("isGround", playerController.IsGround);
        Animator.SetBool("walk", playerController.Walking());
        Animator.SetBool("onAir", playerController.IsAirKick);
        Animator.SetFloat("speedY", playerController.playerRb.velocity.y);
    }
}
