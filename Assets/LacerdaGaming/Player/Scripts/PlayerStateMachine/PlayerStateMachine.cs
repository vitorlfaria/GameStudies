using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class PlayerStateMachine : StateManager<PlayerStateMachine.EPlayerState>
{
    public enum EPlayerState
    {
        Idle,
        Walk,
        Run,
        Sprint,
        Jumping,
        Falling,
        Grounded,
        Climbing
    }

    public event EventHandler<OnStaminaChangeEventArgs> OnStaminaChange;
    public class OnStaminaChangeEventArgs : EventArgs
    {
        public float stamina;
        public float maxStamina;
        public float percentage;
    }

    private PlayerContext _context;

    public PlayerContext PlayerContext { get => _context; }

    [Header("Components")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private PlayerLocomotionInput _playerLocomotionInput;
    [SerializeField] private PlayerAnimation _playerAnimation;

    [Header("Configuration")]
    [SerializeField] private PlayerMovementConfig _movementConfig;
    [SerializeField] private PlayerClimbingConfig _climbingConfig;

    [Header("Environmental Details")]
    [SerializeField] private LayerMask _groundLayers;

    private void Awake()
    {
        ValidateConfigs();

        _context = new PlayerContext(
            _characterController,
            _playerCamera,
            _playerLocomotionInput,
            _playerAnimation,
            _groundLayers,
            transform,
            _movementConfig,
            _climbingConfig
        );

        InitializeStates();
    }

    private void ValidateConfigs()
    {
        if (_movementConfig == null)
        {
            Debug.LogError("PlayerMovementConfig is not assigned! Please create one in Assets/Create/Player/Movement Config");
        }

        if (_climbingConfig == null)
        {
            Debug.LogError("PlayerClimbingConfig is not assigned! Please create one in Assets/Create/Player/Climbing Config");
        }
    }

    public override void Update()
    {
        _context.IsPlayerGrounded();
        HandleGravity();
        CurrentState.UpdateState();
        HandleStamina();
    }

    private void LateUpdate()
    {
        CurrentState.LateUpdateState();
        UpdateCameraRotation();
    }

    public void SetCurrentState(BaseState<EPlayerState> newState)
    {
        CurrentState = newState;
    }

    private void HandleGravity()
    {
        _context.VerticalVelocity += _movementConfig.gravity * Time.deltaTime;

        if (_context.VerticalVelocity < 0 && _context.IsGrounded)
        {
            _context.VerticalVelocity = -_movementConfig.antiBumpForce;
        }

        if (Mathf.Abs(_context.VerticalVelocity) > Mathf.Abs(_movementConfig.terminalVelocity))
        {
            _context.VerticalVelocity = -1f * Mathf.Abs(_movementConfig.terminalVelocity);
        }
    }

    private void UpdateCameraRotation()
    {
        _context.CameraRotation.x += _movementConfig.lookSensitivityH * _playerLocomotionInput.LookInput.x;
        _context.CameraRotation.y = Math.Clamp(
            _context.CameraRotation.y - _movementConfig.lookSensitivityV * _playerLocomotionInput.LookInput.y,
            -_movementConfig.lookLimitV, _movementConfig.lookLimitV
        );

        _context.PlayerCamera.transform.rotation = Quaternion.Euler(_context.CameraRotation.y, _context.CameraRotation.x, 0f);
    }

    private void HandleStamina()
    {
        if (_context.IsSprinting)
        {
            DepletStamina();
            return;
        }

        if (_context.IsClimbing)
        {
            if (_context.PlayerLocomotionInput.MovementInput.magnitude > 0.01f)
            {
                DepletStamina();
                return;
            }

            DepletStamina(0.18f);
            return;
        }

        if (_context.Stamina < 100f)
        {
            _context.Stamina += _movementConfig.staminaRecoveryRate;
            OnStaminaChange?.Invoke(this, new OnStaminaChangeEventArgs
            {
                stamina = _context.Stamina
            });
        }
    }

    private void DepletStamina(float rate = 0)
    {
        _context.Stamina -= _movementConfig.staminaDepletionRate - rate;
        OnStaminaChange?.Invoke(this, new OnStaminaChangeEventArgs
        {
            stamina = _context.Stamina
        });
    }

    private void InitializeStates()
    {
        States.Add(EPlayerState.Idle, new IdleState(_context, EPlayerState.Idle, this));
        States.Add(EPlayerState.Walk, new WalkState(_context, EPlayerState.Walk, this));
        States.Add(EPlayerState.Run, new RunState(_context, EPlayerState.Run, this));
        States.Add(EPlayerState.Sprint, new SprintState(_context, EPlayerState.Sprint, this));
        States.Add(EPlayerState.Jumping, new JumpingState(_context, EPlayerState.Jumping, this));
        States.Add(EPlayerState.Falling, new FallingState(_context, EPlayerState.Falling, this));
        States.Add(EPlayerState.Grounded, new GroundedState(_context, EPlayerState.Grounded, this));
        States.Add(EPlayerState.Climbing, new ClimbingState(_context, EPlayerState.Climbing, this));

        (States[EPlayerState.Grounded] as GroundedState).Initialize();

        CurrentState = States[EPlayerState.Grounded];
    }
}
