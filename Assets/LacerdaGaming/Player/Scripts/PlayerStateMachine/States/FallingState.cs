using UnityEngine;

public class FallingState : PlayerState
{
    public FallingState(PlayerContext context, PlayerStateMachine.EPlayerState stateKey, PlayerStateMachine playerStateMachine) : base(context, stateKey, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        _context.CurrentDrag = _context.MovementConfig.airborneDrag;
        _context.PlayerAnimation.SetIsFalling(true);
    }

    public override void ExitState()
    {
        _context.PlayerAnimation.SetIsFalling(false);
    }

    public override void UpdateState()
    {
        HandleMovement();
        GetNextState();
    }

    public override void LateUpdateState()
    {
        HandlePlayerRotationWhenMoving();
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        if (_context.CharacterController.isGrounded)
        {
            _playerStateMachine.TransitionToState(PlayerStateMachine.EPlayerState.Grounded);
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
}
