using UnityEngine;

public class GroundedState : PlayerState
{
    public GroundedState(PlayerContext context, PlayerStateMachine.EPlayerState stateKey, PlayerStateMachine playerStateMachine) : base(context, stateKey, playerStateMachine)
    {
    }

    public void Initialize()
    {
        SetSubState(_playerStateMachine.States[PlayerStateMachine.EPlayerState.Idle] as PlayerState);
    }

    public override void EnterState()
    {
        _context.CurrentDrag = _context.MovementConfig.groundedDrag;
        _context.PlayerAnimation.SetIsGrounded(true);
        _currentSubState?.EnterState();
    }

    public override void UpdateState()
    {
        GetNextState();
        _currentSubState?.UpdateState();
    }

    public override void ExitState()
    {
        _context.PlayerAnimation.SetIsGrounded(false);
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        if (_context.PlayerLocomotionInput.JumpPressed)
        {
            _playerStateMachine.TransitionToState(PlayerStateMachine.EPlayerState.Jumping);
        }

        if (!_context.CharacterController.isGrounded)
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
}
