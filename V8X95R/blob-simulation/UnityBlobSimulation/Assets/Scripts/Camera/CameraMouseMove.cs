using UnityEngine;


public class CameraMouseMove : MonoBehaviour
{
    public float screenEdgeThreshold = 10f;
    public float moveSpeed = 5f;
    public float smoothing = 0.1f;

    private Vector3 targetPosition;
    public Vector3 velocity = Vector3.zero;

    public Vector2 min;
    public Vector2 max;


    void Update()
    {
        HandleInput();
        MoveCamera();
    }

    void HandleInput() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 inputDirection = Vector3.zero;

        if (mousePos.y >= Screen.height - screenEdgeThreshold) {
            inputDirection.z = 1;
        }
        else if (mousePos.y <= screenEdgeThreshold) // Bottom edge
{
            inputDirection.z = -1;
        }
        
        if (mousePos.x <= screenEdgeThreshold*2) // Left edge
        {
            inputDirection.x = -1;
        }
        else if (mousePos.x >= Screen.width - screenEdgeThreshold*2) // Right edge
        {
            inputDirection.x = 1;
        }
        inputDirection.Normalize();

        Vector3 moveDirection = Quaternion.AngleAxis(transform.localEulerAngles.y, Vector3.up) * inputDirection;
        targetPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;

        targetPosition.x = Mathf.Clamp(targetPosition.x, min.x, max.x);
        targetPosition.z = Mathf.Clamp(targetPosition.z, min.y, max.y);
    }

    void MoveCamera() {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothing);
    }
}
