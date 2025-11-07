using UnityEngine;
using static PlayerStateMachine;

public class PlayerContext
{
    // Components
    public CharacterController CharacterController { get; set; }
    public Camera PlayerCamera { get; set; }
    public PlayerLocomotionInput PlayerLocomotionInput { get; set; }
    public PlayerAnimation PlayerAnimation { get; set; }
    public Transform PlayerTransform { get; set; }

    // Configurations (ScriptableObjects)
    public PlayerMovementConfig MovementConfig { get; private set; }
    public PlayerClimbingConfig ClimbingConfig { get; private set; }

    // Environmental
    public LayerMask GroundLayers { get; set; }

    // State tracking
    public EPlayerState LastMovementState { get; set; }
    public bool IsGrounded { get; set; }
    public bool JumpedLastFrame { get; set; }
    public bool IsSprinting { get; set; }
    public bool IsClimbing { get; set; }

    // Runtime values
    public Vector2 CameraRotation;
    public Vector3 MovementDirection;
    public float VerticalVelocity { get; set; }
    public float Stamina { get; set; }

    // Current state values (set by states based on config)
    public float CurrentAcceleration { get; set; }
    public float CurrentSpeed { get; set; }
    public float CurrentDrag { get; set; }

    private float _stepOffset;

    public PlayerContext(
        CharacterController characterController,
        Camera playerCamera,
        PlayerLocomotionInput playerLocomotionInput,
        PlayerAnimation playerAnimation,
        LayerMask groundLayers,
        Transform playerTransform,
        PlayerMovementConfig movementConfig,
        PlayerClimbingConfig climbingConfig)
    {
        CharacterController = characterController;
        PlayerCamera = playerCamera;
        PlayerLocomotionInput = playerLocomotionInput;
        PlayerAnimation = playerAnimation;
        GroundLayers = groundLayers;
        PlayerTransform = playerTransform;
        MovementConfig = movementConfig;
        ClimbingConfig = climbingConfig;

        // Initialize from config
        Stamina = movementConfig.maxStamina;
        VerticalVelocity = 0f;
        JumpedLastFrame = false;
        LastMovementState = EPlayerState.Idle;
        _stepOffset = CharacterController.stepOffset;
    }

    public void IsPlayerGrounded()
    {
        IsGrounded = CharacterController.isGrounded ? IsGroundedWhileGrounded() : IsGroundedWhileAirborne();
    }

    private bool IsGroundedWhileGrounded()
    {
        Vector3 spherePosition = new Vector3(
            PlayerTransform.position.x,
            PlayerTransform.position.y - CharacterController.radius,
            PlayerTransform.position.z);

        return Physics.CheckSphere(spherePosition, CharacterController.radius, GroundLayers, QueryTriggerInteraction.Ignore);
    }

    private bool IsGroundedWhileAirborne()
    {
        Vector3 normal = CharacterControllerUtils.GetNormalWithSphereCast(CharacterController, GroundLayers);
        float angle = Vector3.Angle(normal, Vector3.up);
        bool validAngle = angle <= CharacterController.slopeLimit;

        return CharacterController.isGrounded && validAngle;
    }

    public bool IsStateGroundedState(EPlayerState playerState)
    {
        return playerState == EPlayerState.Idle ||
            playerState == EPlayerState.Walk ||
            playerState == EPlayerState.Run ||
            playerState == EPlayerState.Sprint;
    }

    public bool CheckForClimbableSurface(out RaycastHit hitInfo)
    {
        Vector3 origin = PlayerTransform.position + Vector3.up * (CharacterController.height / 2f);
        return Physics.Raycast(
            origin,
            PlayerTransform.forward,
            out hitInfo,
            ClimbingConfig.climbCheckDistance,
            ClimbingConfig.climbableLayers);
    }

    public float GetStepOffset() => _stepOffset;

    public void SetStepOffset(float value)
    {
        CharacterController.stepOffset = value;
    }

    public void ResetStepOffset()
    {
        CharacterController.stepOffset = _stepOffset;
    }
}