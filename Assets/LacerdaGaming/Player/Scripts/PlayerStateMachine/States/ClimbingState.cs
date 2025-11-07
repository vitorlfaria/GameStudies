using UnityEngine;

public class ClimbingState : PlayerState
{
    private bool isWallJumping;

    public ClimbingState(PlayerContext context, PlayerStateMachine.EPlayerState stateKey, PlayerStateMachine playerStateMachine)
        : base(context, stateKey, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        _context.IsClimbing = true;
        _context.PlayerAnimation.SetIsClimbing(true);
        _context.VerticalVelocity = 0f;
        isWallJumping = false;
    }

    public override void ExitState()
    {
        _context.IsClimbing = false;
        _context.PlayerAnimation.SetIsClimbing(false);
        isWallJumping = false;
    }

    public override void UpdateState()
    {
        if (!_context.CheckForClimbableSurface(out _climbHit))
        {
            _playerStateMachine.TransitionToState(PlayerStateMachine.EPlayerState.Falling);
            return;
        }
        HandleMovement();
        GetNextState();
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        // Check for jump off wall
        if (_context.ClimbingConfig.allowClimbJump && _context.PlayerLocomotionInput.JumpPressed)
        {
            PerformWallJump();
            _playerStateMachine.TransitionToState(PlayerStateMachine.EPlayerState.Falling);
        }

        return StateKey;
    }

    protected override void HandleMovement()
    {
        Vector2 input = _context.PlayerLocomotionInput.MovementInput;

        Vector3 upward = _context.ClimbingConfig.climbSpeed * input.y * Vector3.up;
        Vector3 side = _context.ClimbingConfig.climbStrafeSpeed * input.x * _context.PlayerTransform.right;

        Vector3 move = upward + side;
        _context.CharacterController.Move(move * Time.deltaTime);

        if (!isWallJumping)
        {
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
    }

    private void PerformWallJump()
    {
        Debug.Log("Wall jumping!");
        isWallJumping = true;

        // Jump away from wall with upward force
        Vector3 jumpDirection = (_climbHit.normal + Vector3.up).normalized;
        _context.VerticalVelocity = _context.ClimbingConfig.wallJumpForce;

        // Add horizontal velocity away from wall
        Vector3 horizontalJump = jumpDirection * _context.ClimbingConfig.wallJumpForce * 0.5f;
        _context.CharacterController.Move(horizontalJump * Time.deltaTime);
        HandlePlayerRotation(_climbHit.normal);
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
