using UnityEngine;

public class SprintState : PlayerState
{
    public SprintState(PlayerContext context, PlayerStateMachine.EPlayerState stateKey, PlayerStateMachine playerStateMachine) : base(context, stateKey, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        if (_context.Stamina <= 40f)
        {
            _currentSuperState.SetSubState(_playerStateMachine.States[PlayerStateMachine.EPlayerState.Run] as PlayerState);
            return;
        }

        _context.CurrentAcceleration = _context.MovementConfig.sprintAcceleration;
        _context.CurrentSpeed = _context.MovementConfig.sprintSpeed;
        _context.PlayerAnimation.SetMaxBlendValue(_context.MovementConfig.sprintBlendValue);
        _context.IsSprinting = true;
    }

    public override void ExitState()
    {
        _context.IsSprinting = false;
    }

    public override void UpdateState()
    {
        HandleMovement();
        GetNextState();
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        if (_context.Stamina <= 0f)
        {
            _currentSuperState.SetSubState(_playerStateMachine.States[PlayerStateMachine.EPlayerState.Run] as PlayerState);
        }

        if (_context.PlayerLocomotionInput.MovementInput.magnitude <= 0.01f)
        {
            _currentSuperState.SetSubState(_playerStateMachine.States[PlayerStateMachine.EPlayerState.Idle] as PlayerState);
        }

        if (!_context.PlayerLocomotionInput.SprintToggledOn)
        {
            _currentSuperState.SetSubState(_playerStateMachine.States[PlayerStateMachine.EPlayerState.Run] as PlayerState);
        }

        if (_context.PlayerLocomotionInput.WalkToggledOn)
        {
            _currentSuperState.SetSubState(_playerStateMachine.States[PlayerStateMachine.EPlayerState.Walk] as PlayerState);
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
