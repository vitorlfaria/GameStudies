using UnityEngine;

public class ClimbingState : PlayerState
{
    private RaycastHit _climbHit;

    public ClimbingState(PlayerContext context, PlayerStateMachine.EPlayerState stateKey, PlayerStateMachine playerStateMachine)
        : base(context, stateKey, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        _context.IsClimbing = true;
        _context.PlayerAnimation.SetIsClimbing(true);
        _context.VerticalVelocity = 0f;
    }

    public override void ExitState()
    {
        _context.IsClimbing = false;
        _context.PlayerAnimation.SetIsClimbing(false);
    }

    public override void UpdateState()
    {
        if (!_context.CheckForClimbableSurface(out _climbHit))
        {
            _playerStateMachine.TransitionToState(PlayerStateMachine.EPlayerState.Falling);
            return;
        }

    }

    public override PlayerStateMachine.EPlayerState GetNextState() => StateKey;

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
