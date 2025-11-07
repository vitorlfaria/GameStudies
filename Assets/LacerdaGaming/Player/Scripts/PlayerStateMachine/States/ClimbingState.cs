using UnityEngine;

public class ClimbingState : PlayerState
{
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
        HandleMovement();
    }

    protected override void HandleMovement()
    {
        Vector2 input = _context.PlayerLocomotionInput.MovementInput;

        Vector3 upward = _context.ClimbingConfig.climbSpeed * input.y * Vector3.up;
        Vector3 side = _context.ClimbingConfig.climbStrafeSpeed * input.x * _context.PlayerTransform.right;

        Vector3 move = upward + side;
        _context.CharacterController.Move(move * Time.deltaTime);

        _context.PlayerTransform.forward = -_climbHit.normal;
        _context.PlayerTransform.position = Vector3.Lerp(
            _context.PlayerTransform.position,
            new Vector3(
                _climbHit.point.x,
                _climbHit.point.y - _context.CharacterController.height / 2f,
                _climbHit.point.z) + _climbHit.normal * 0.51f,
            10f * Time.deltaTime
        );
    }

    public override PlayerStateMachine.EPlayerState GetNextState() => StateKey;

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
