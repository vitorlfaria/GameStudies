using UnityEngine;

public abstract class PlayerState : BaseState<PlayerStateMachine.EPlayerState>
{
    protected PlayerContext _context;
    protected PlayerStateMachine _playerStateMachine;
    protected PlayerState _currentSuperState;
    protected PlayerState _currentSubState;

    public PlayerState(PlayerContext context, PlayerStateMachine.EPlayerState stateKey, PlayerStateMachine playerStateMachine) : base(stateKey)
    {
        _context = context;
        _playerStateMachine = playerStateMachine;
    }

    public void SetSuperState(PlayerState newSuperState)
    {
        _currentSuperState?.ExitState();
        _currentSuperState = newSuperState;
        _currentSuperState?.EnterState();
    }

    public void SetSubState(PlayerState newSubState)
    {
        _currentSubState?.ExitState();
        _currentSubState = newSubState;
        _currentSubState?.EnterState();

        if (_currentSubState._currentSuperState != this)
        {
            _currentSubState.SetSuperState(this);
        }
    }

    public virtual void HandleMovement()
    {
        Vector3 cameraForwardXZ = new Vector3(_context.PlayerCamera.transform.forward.x, 0f, _context.PlayerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(_context.PlayerCamera.transform.right.x, 0f, _context.PlayerCamera.transform.right.z).normalized;
        Vector3 movementDirection = (cameraRightXZ * _context.PlayerLocomotionInput.MovementInput.x) + (cameraForwardXZ * _context.PlayerLocomotionInput.MovementInput.y);

        if (movementDirection.magnitude > 0.1f)
        {
            _context.MovementDirection = movementDirection.normalized;
        }

        Vector3 horizontalVelocity = new(_context.CharacterController.velocity.x, 0f, _context.CharacterController.velocity.z);
        Vector3 movementDelta = _context.CurrentAcceleration * Time.deltaTime * movementDirection;
        Vector3 newHorizontalVelocity = horizontalVelocity + movementDelta;

        Vector3 currentDrag = _context.CurrentDrag * Time.deltaTime * newHorizontalVelocity.normalized;
        newHorizontalVelocity = (newHorizontalVelocity.magnitude > _context.CurrentDrag * Time.deltaTime) ? newHorizontalVelocity - currentDrag : Vector3.zero;
        newHorizontalVelocity = Vector3.ClampMagnitude(newHorizontalVelocity, _context.CurrentSpeed);

        Vector3 finalVelocity = new(newHorizontalVelocity.x, _context.VerticalVelocity, newHorizontalVelocity.z);

        _context.CharacterController.Move(finalVelocity * Time.deltaTime);
    }
}
