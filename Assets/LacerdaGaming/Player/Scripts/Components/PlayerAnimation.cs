using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerLocomotionInput _playerLocomotionInput;
    [SerializeField] private Animator _animator;

    [Header("Locomotion")]
    [SerializeField] private float _locomotionBlendSpeed;

    private static readonly int inputMagnitudeHash = Animator.StringToHash("inputMagnitude");
    private static readonly int isJumpingHash = Animator.StringToHash("isJumping");
    private static readonly int isGroundedHash = Animator.StringToHash("isGrounded");
    private static readonly int isFallingHash = Animator.StringToHash("isFalling");
    private static readonly int isClimbingHash = Animator.StringToHash("isClimbing");

    private Vector3 _currentBlendValue = Vector3.zero;

    private float _maxBlendValue;

    private void Update()
    {
        UpdateAnimationState();
    }

    public void SetIsJumping(bool value)
    {
        _animator.SetBool(isJumpingHash, value);
    }

    public void SetIsClimbing(bool value)
    {
        _animator.SetBool(isClimbingHash, value);
    }

    public void SetIsFalling(bool value)
    {
        _animator.SetBool(isFallingHash, value);
    }

    public void SetIsGrounded(bool value)
    {
        _animator.SetBool(isGroundedHash, value);
    }

    private void UpdateAnimationState()
    {
        Vector2 inputTarget = _playerLocomotionInput.MovementInput * _maxBlendValue;
        _currentBlendValue = Vector3.Lerp(_currentBlendValue, inputTarget, _locomotionBlendSpeed * Time.deltaTime);

        _animator.SetFloat(inputMagnitudeHash, _currentBlendValue.magnitude);
    }

    public void SetMaxBlendValue(float value)
    {
        _maxBlendValue = value;
    }
}
