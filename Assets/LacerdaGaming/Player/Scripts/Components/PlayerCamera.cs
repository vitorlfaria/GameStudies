using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private PlayerLocomotionInput _playerLocomotionInput;
    [SerializeField] private PlayerStateMachine _playerStateMachine;
    [SerializeField] private PlayerMovementConfig _config;

    private void UpdateCameraRotation()
    {
        _playerStateMachine.PlayerContext.CameraRotation.x += _config.lookSensitivityH * _playerLocomotionInput.LookInput.x;

        _playerStateMachine.PlayerContext.CameraRotation.y = Math.Clamp(
            _playerStateMachine.PlayerContext.CameraRotation.y - _config.lookSensitivityV * _playerLocomotionInput.LookInput.y,
            -_config.lookLimitV, _config.lookLimitV
        );

        _playerStateMachine.PlayerContext.PlayerCamera.transform.rotation = Quaternion.Euler(
            _playerStateMachine.PlayerContext.CameraRotation.y,
            _playerStateMachine.PlayerContext.CameraRotation.x, 0
        );
    }
}
