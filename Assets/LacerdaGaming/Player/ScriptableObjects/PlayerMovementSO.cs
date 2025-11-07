using UnityEngine;

/// <summary>
/// Configuration for all player movement states
/// Place in Assets/ScriptableObjects/Player/
/// </summary>
[CreateAssetMenu(fileName = "PlayerMovementConfig", menuName = "Player/Movement Config")]
public class PlayerMovementConfig : ScriptableObject
{
    [Header("Idle State")]
    [Tooltip("Animation blend value for idle (typically 0)")]
    public float idleBlendValue = 0f;

    [Header("Walk State")]
    [Tooltip("How fast the player accelerates when walking")]
    public float walkAcceleration = 25f;
    [Tooltip("Maximum walk speed")]
    public float walkSpeed = 2f;
    [Tooltip("Animation blend value for walking")]
    public float walkBlendValue = 0.5f;

    [Header("Run State")]
    public float runAcceleration = 35f;
    public float runSpeed = 4f;
    public float runBlendValue = 1f;

    [Header("Sprint State")]
    public float sprintAcceleration = 50f;
    public float sprintSpeed = 7f;
    public float sprintBlendValue = 1.5f;
    [Tooltip("Minimum stamina required to start sprinting")]
    public float minimumSprintStamina = 40f;

    [Header("Drag & Friction")]
    [Tooltip("Drag applied when grounded (higher = stops faster)")]
    public float groundedDrag = 20f;
    [Tooltip("Drag applied when airborne (lower = more air control)")]
    public float airborneDrag = 5f;

    [Header("Jump")]
    [Tooltip("Height of the jump in meters")]
    public float jumpHeight = 1.5f;

    [Header("Gravity")]
    [Tooltip("Gravity acceleration (negative value)")]
    public float gravity = -15f;
    [Tooltip("Maximum falling speed")]
    public float terminalVelocity = 50f;
    [Tooltip("Small downward force when grounded to prevent bouncing")]
    public float antiBumpForce = 7f;

    [Header("Camera")]
    [Tooltip("Horizontal look sensitivity")]
    public float lookSensitivityH = 0.1f;
    [Tooltip("Vertical look sensitivity")]
    public float lookSensitivityV = 0.1f;
    [Tooltip("Maximum vertical look angle (degrees)")]
    public float lookLimitV = 89f;

    [Header("Rotation")]
    [Tooltip("How fast the player rotates to face movement direction")]
    public float rotationSpeed = 5f;
    [Tooltip("Speed at which animation blends between states")]
    public float animationBlendSpeed = 5f;

    [Header("Stamina")]
    [Tooltip("Maximum stamina amount")]
    public float maxStamina = 100f;
    [Tooltip("Stamina consumed per second while sprinting")]
    public float staminaDepletionRate = 10f;
    [Tooltip("Stamina recovered per second while not sprinting")]
    public float staminaRecoveryRate = 15f;
}