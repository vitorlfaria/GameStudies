using UnityEngine;

public class JumpingState : PlayerState
{
    public JumpingState(PlayerContext context, PlayerStateMachine.EPlayerState stateKey, PlayerStateMachine playerStateMachine) : base(context, stateKey, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        _context.CurrentDrag = _context.MovementConfig.airborneDrag;
        _context.JumpedLastFrame = false;
        _context.SetStepOffset(0f);
        _context.PlayerAnimation.SetIsJumping(true);
        HandleVerticalMovement();
    }

    public override void ExitState()
    {
        _context.JumpedLastFrame = false;
        _context.ResetStepOffset();
        _context.PlayerAnimation.SetIsJumping(false);
    }

    public override void UpdateState()
    {
        HandleMovement();
        GetNextState();
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        if (_context.VerticalVelocity < 0f)
        {
            _playerStateMachine.TransitionToState(PlayerStateMachine.EPlayerState.Falling);
        }

        return StateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {
    }

    public override void OnTriggerExit(Collider other)
    {
    }

    public override void OnTriggerStay(Collider other)
    {
    }

    private void HandleVerticalMovement()
    {
        _context.VerticalVelocity = Mathf.Sqrt(_context.MovementConfig.jumpHeight * -2.0f * _context.MovementConfig.gravity);
        _context.JumpedLastFrame = true;
    }
}
