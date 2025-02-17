using UnityEngine;

public class PlayerJumpState :PlayerAbilityState
{
    private int amountOfJumpsLeft;
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();
        player.inputController.UseRunJumpInput();
        playerData.isJumpState = true;
        player.SetVelocityY( playerData.jumpForce);
        isAbilityDone = true;
        amountOfJumpsLeft--;
        player.airState.SetIsJumping();
    }

    public override void Exit()
    {
        base.Exit();
        playerData.isJumpState = false;
        
    }

    public override void Update()
    {
        base.Update();
       
       
    }

    public bool CanJump()
    {
        if (amountOfJumpsLeft > 0)
        {
            Debug.LogWarning("Can Jump");
            return true;
        }
        else
        {
            Debug.LogWarning("Can't Jump");
            return false;
        }
    }

    public void ResetAmountOfJumps()
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }
    public void DecrementAmountOfJumpsLeft()=> amountOfJumpsLeft--;
}
