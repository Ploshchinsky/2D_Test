using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerFollowingCamera : MonoBehaviour
{

    [SerializeField] private Vector3 _staticOffset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float _smoothTime = 0.2f;
    [SerializeField] private bool _mouseOffset = false;
    [SerializeField] private float _mouseOffsetCoefficient = 0.2f;

    private Player _player;
    private GameInput _gameInput;
    private Transform _myTransform;
    private Vector3 _currentVelocity = Vector3.zero;
    private Camera _mainCamera;

    private void Awake()
    {
        _myTransform = this.transform;
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        GetPlayerInstance();
        GetGameInputInstance();
    }


    private void LateUpdate()
    {
        HandlingCameraPositionByPlayerPosition();
    }

    private void HandlingCameraPositionByPlayerPosition()
    {
        Transform playerTransform = _player.transform;
        if (playerTransform == null) return;

        Vector3 cameraOffset = _mouseOffset ? GetCameraOffset() : _staticOffset;

        Vector3 targetPosition = playerTransform.position + cameraOffset;

        _myTransform.position = Vector3.SmoothDamp(
            current: _myTransform.position,
            target: targetPosition,
            currentVelocity: ref _currentVelocity,
            smoothTime: _smoothTime
        );
    }

    private Vector3 GetCameraOffset()
    {
        // Получаем позицию мыши в Viewport coordinates(0 - 1)
        Vector3 mouseViewport = _mainCamera.ScreenToViewportPoint(Input.mousePosition);

        // Преобразуем в диапазон (-1 до 1)
        Vector3 mouseOffset = new Vector3(
            (mouseViewport.x - 0.5f) * 2f,
            (mouseViewport.y - 0.5f) * 2f,
            0f
        );

        // Вычисляем максимальное смещение на основе размера камеры и коэффициента
        float cameraSize = _mainCamera.orthographicSize;
        float horizontalMaxOffset = cameraSize * _mainCamera.aspect * _mouseOffsetCoefficient;
        float verticalMaxOffset = 1.7f * (cameraSize * _mouseOffsetCoefficient);

        // Применяем смещение
        Vector3 cameraOffset = new Vector3(
            mouseOffset.x * horizontalMaxOffset,
            mouseOffset.y * verticalMaxOffset,
            _staticOffset.z
        );

        return _staticOffset + cameraOffset;
    }

    private void GetGameInputInstance()
    {
        if (GameInput.Instance != null)
        {
            _gameInput = GameInput.Instance;
        }
        else
        {
            Debug.Log("GameInput is null!");
        }
    }

    private void GetPlayerInstance()
    {
        if (Player.Instance != null)
        {
            _player = Player.Instance;
        }
        else
        {
            Debug.Log("Player is null!");
        }
    }
}
