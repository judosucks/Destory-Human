using UnityEngine;

public class PlayerSprintJumpState : PlayerState
{
    public PlayerSprintJumpState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        player.isFallingFromJump = false;
        // if (!playerData.reachedApex)
        // {
        //     player.startFallHeight = player.transform.position.y;
        //     playerData.reachedApex = true;
        // }
        // Debug.LogWarning("startfallheight sprint jump state: " + player.startFallHeight);
    }
}