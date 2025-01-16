using UnityEngine;

public class PlayerHurtState : PlayerState
{
    private Enemy _enemy;
    private float knockbackDuration = 0.2f; // Optional: Duration of neutralizing input
    private Vector2 knockbackForce = new Vector2(-2, 2); // Knockback velocity
    public PlayerHurtState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) 
        : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("hurt state enter");
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        stateTimer = knockbackDuration;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        // Apply knockback force if applicable
        player.SetVelocity(knockbackForce.x * -player.facingDirection, knockbackForce.y);
        if (_enemy.facingDirection == player.facingDirection)
        {
            player.SetVelocity(knockbackForce.x * player.facingDirection, knockbackForce.y);
        }
        // Disable player controls (isBusy prevents inputs)
        player.SetIsBusy(true);
        
        // Trigger hurt animation
        player.anim.SetTrigger("Hurt");
        
        // Set a timer (optional) to end the hurt state after knockback
        
    }

    public override void Update()
    {
        base.Update();

        // If timer reaches 0, return to appropriate state
        if (stateTimer < 0)
        {
            // Transition back to `IdleState` or `AirState` depending on context
            if (player.IsGroundDetected())
            {
                Debug.Log("grounded from hurt");
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                Debug.Log("air from hurt");
                stateMachine.ChangeState(player.airState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        // Re-enable player controls
        player.SetIsBusy(false);
    }
}