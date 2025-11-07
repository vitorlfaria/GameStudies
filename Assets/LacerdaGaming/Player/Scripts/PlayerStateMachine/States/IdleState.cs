using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerContext context, PlayerStateMachine.EPlayerState stateKey, PlayerStateMachine playerStateMachine) : base(context, stateKey, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        _context.PlayerAnimation.SetMaxBlendValue(_context.MovementConfig.idleBlendValue);
    }

    public override void UpdateState()
    {
        GetNextState();
    }

    public override void ExitState()
    {
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        if (_context.PlayerLocomotionInput.MovementInput.magnitude > 0.01f)
        {
            _currentSuperState.SetSubState(_playerStateMachine.States[PlayerStateMachine.EPlayerState.Run] as PlayerState);
        }

        if (_context.PlayerLocomotionInput.SprintToggledOn)
        {
            _currentSuperState.SetSubState(_playerStateMachine.States[PlayerStateMachine.EPlayerState.Sprint] as PlayerState);
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
