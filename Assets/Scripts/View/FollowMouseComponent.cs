using System;
using UnityEngine;
using UnityEngine.Rendering;

public class FollowMouseComponent : MonoBehaviour
{
    public enum PlacingState
    {
        Position,
        Rotation
    }

    public Camera mainCamera;
    public float distanceFromCamera = 10f;
    private Action<Vector3, Quaternion> _mouseClickCallback;
    private Action _rightMouseClickCallback;
    private ResourceLoader _resourceLoader;
    private PlacingState _currentState;

    public void Initialize(ResourceLoader resourceLoader, Action<Vector3, Quaternion> onMouseClicked, Action rightMouseClicked)
    {
        _resourceLoader = resourceLoader;
        _mouseClickCallback = onMouseClicked;
        _rightMouseClickCallback = rightMouseClicked;
        _currentState = PlacingState.Position;
    }

    void Update()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        if (_currentState == PlacingState.Position)
        {
            UpdatePosition();
        }
        else if (_currentState == PlacingState.Rotation)
        {
            UpdateRotation();
        }

        if (Input.GetMouseButtonDown(1))
        {
            _rightMouseClickCallback?.Invoke();
            _resourceLoader.ReleaseGO(gameObject);
        }
    }

    void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distanceFromCamera; // distance from camera

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Terrain")
            {
                transform.position = hit.point;
                if (Input.GetMouseButtonDown(0)) // Left click
                {
                    _currentState = PlacingState.Rotation;
                }
            }
        }
    }

    void UpdateRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPos = hit.point;

            // Get direction from object to mouse hit point
            Vector3 direction = targetPos - transform.position;
            direction.y = 0f; // Lock rotation to horizontal plane

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = targetRotation;
            }

            if (Input.GetMouseButtonDown(0)) // Left click
            {
                _mouseClickCallback?.Invoke(transform.position, transform.rotation);
                _resourceLoader.ReleaseGO(gameObject);
            }
        }
    }
}
