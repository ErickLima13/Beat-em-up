public class EnemyA : EnemyBase
{
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
            case EnemyState.Positioning:
                Positioning();
                break;
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
}
