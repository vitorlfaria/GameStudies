using UnityEngine;

public class WalkState : PlayerState
{
    public WalkState(PlayerContext context, PlayerStateMachine.EPlayerState stateKey, PlayerStateMachine playerStateMachine) : base(context, stateKey, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        _context.CurrentAcceleration = _context.MovementConfig.walkAcceleration;
        _context.CurrentSpeed = _context.MovementConfig.walkSpeed;
        _context.PlayerAnimation.SetMaxBlendValue(_context.MovementConfig.walkBlendValue);
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        HandleMovement();
        GetNextState();
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        if (_context.PlayerLocomotionInput.MovementInput.magnitude <= 0.01f)
        {
            _currentSuperState.SetSubState(_playerStateMachine.States[PlayerStateMachine.EPlayerState.Idle] as PlayerState);
        }

        if (_context.PlayerLocomotionInput.SprintToggledOn)
        {
            _currentSuperState.SetSubState(_playerStateMachine.States[PlayerStateMachine.EPlayerState.Sprint] as PlayerState);
        }

        if (!_context.PlayerLocomotionInput.WalkToggledOn)
        {
            _currentSuperState.SetSubState(_playerStateMachine.States[PlayerStateMachine.EPlayerState.Run] as PlayerState);
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
