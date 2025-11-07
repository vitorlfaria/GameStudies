using UnityEngine;

/// <summary>
/// Configuration for climbing mechanics
/// </summary>
[CreateAssetMenu(fileName = "PlayerClimbingConfig", menuName = "Player/Climbing Config")]
public class PlayerClimbingConfig : ScriptableObject
{
    [Header("Climb Detection")]
    [Tooltip("Maximum angle of surface that can be climbed (degrees from vertical)")]
    [Range(0f, 90f)]
    public float maxClimbAngle = 80f;

    [Tooltip("Distance to raycast forward to detect climbable surfaces")]
    public float climbCheckDistance = 1f;

    [Tooltip("Layers that are considered climbable")]
    public LayerMask climbableLayers;

    [Header("Climb Movement")]
    [Tooltip("Speed when climbing up/down")]
    public float climbSpeed = 2.5f;

    [Tooltip("Speed when moving left/right while climbing")]
    public float climbStrafeSpeed = 2f;

    [Tooltip("Gravity applied while climbing (usually 0 or small value)")]
    public float climbGravity = 0f;

    [Header("Climb Stamina")]
    [Tooltip("Whether climbing consumes stamina")]
    public bool consumesStamina = true;

    [Tooltip("Stamina consumed per second while climbing")]
    public float staminaDrainRate = 5f;

    [Tooltip("Minimum stamina required to start climbing")]
    public float minimumClimbStamina = 10f;

    [Header("Climb Transitions")]
    [Tooltip("How close to the top before triggering climb-up animation")]
    public float climbUpThreshold = 0.5f;

    [Tooltip("Can the player jump off the wall while climbing?")]
    public bool allowClimbJump = true;

    [Tooltip("Force applied when jumping off a wall")]
    public float wallJumpForce = 8f;

    [Header("Animation")]
    [Tooltip("Animation blend speed for climb animations")]
    public float climbAnimationBlendSpeed = 5f;
}