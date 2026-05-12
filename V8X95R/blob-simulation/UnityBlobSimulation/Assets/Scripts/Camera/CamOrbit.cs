using System.Runtime.InteropServices;
using UnityEngine;


public class CamOrbit : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);
    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT {
        public int X;
        public int Y;
    }


    POINT mousePos;

    public bool moveCam;
    public float camSmoothing;

    [Header("Sensitivity")]
    public Vector2 sens = new Vector2(0.2f, 0.2f);

    [Space, Header("Bounds")]
    public bool lockX;
    public bool lockY;
    public Vector2 lockLookX = new Vector2(0f, 0f);
    public Vector2 lockLookY = new Vector2(0f, 0f);

    private Vector2 lookInput;
    private Vector2 targetLookRot = Vector2.zero;
    private Vector2 currentLookRot = Vector2.zero;



    private void Update() {
        if (Input.GetKey(KeyCode.Mouse1)) {
            if (!moveCam) EnterFreeCam();
        }
        else if (moveCam) ExitFreeCam();
    }

    private void LateUpdate() {
        Vector2 mouseInput = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );
        ProcessLook(mouseInput);

        currentLookRot = Vector2.Lerp(
            currentLookRot,
            targetLookRot,
            Mathf.Exp(-camSmoothing * Time.deltaTime)
        );

        if ((currentLookRot - targetLookRot).sqrMagnitude < 0.0001f)
            currentLookRot = targetLookRot;

        transform.rotation = Quaternion.Euler(
            currentLookRot.x,
            currentLookRot.y,
            0f
        );
    }

    public void ProcessLook(Vector2 input) {
        if (!moveCam) return;

        lookInput = input * sens;

        targetLookRot.y += lookInput.x;
        targetLookRot.x -= lookInput.y;

        if (lockX) targetLookRot.x = Mathf.Clamp(targetLookRot.x, lockLookX.x, lockLookX.y);
        if (lockY) targetLookRot.y = Mathf.Clamp(targetLookRot.y, lockLookY.x, lockLookY.y);
    }


    void EnterFreeCam() {
        moveCam = true;

        GetCursorPos(out mousePos);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void ExitFreeCam() {
        moveCam = false;

        Cursor.lockState = CursorLockMode.None;
        SetCursorPos(mousePos.X, mousePos.Y);
        Cursor.visible = true;
    }

}
