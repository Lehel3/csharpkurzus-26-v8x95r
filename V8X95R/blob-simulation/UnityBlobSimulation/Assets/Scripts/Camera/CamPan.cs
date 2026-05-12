using UnityEngine;


public class CamPan : MonoBehaviour
{
    Transform cam;

    public bool moveCam;
    [Header("Pan")]
    public float panSpeed = 0.002f;
    public float panSmoothing = 12f;


    [Header("Input")]
    public Vector2 sens = new Vector2(0.2f, 0.2f);
    public Vector2 mouseInput;

    public Vector3 targetPivotPos;
    public Vector3 currentPivotPos;

    private void Start() {
        cam = Camera.main.transform;
        currentPivotPos = transform.position;
        targetPivotPos = currentPivotPos;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Mouse2)) {
            if (!moveCam) EnterFreeCam();
        }
        else if (moveCam) ExitFreeCam();
    }

    private void LateUpdate() {
        Vector2 mouseDelta = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        ProcessPan(mouseDelta, -cam.localPosition.z);

        currentPivotPos = Vector3.Lerp(
            currentPivotPos,
            targetPivotPos,
            Mathf.Exp(-panSmoothing * Time.deltaTime)
        );

        if ((currentPivotPos - targetPivotPos).sqrMagnitude < 0.0001f)
            currentPivotPos = targetPivotPos;

        transform.position = currentPivotPos;
    }

    public void ProcessPan(Vector2 input, float distance) {
        if (!moveCam) return;

        Vector3 right = cam.right;
        Vector3 up = cam.up;

        mouseInput = input * sens;

        Vector3 pan = (-right * mouseInput.x + -up * mouseInput.y) * panSpeed * distance;

        targetPivotPos += pan;
    }


    void EnterFreeCam() {
        moveCam = true;

    }
    void ExitFreeCam() {
        moveCam = false;
    }

}
