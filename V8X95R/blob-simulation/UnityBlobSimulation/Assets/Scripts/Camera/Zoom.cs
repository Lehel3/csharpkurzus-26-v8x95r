using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    Camera cam;
    public Transform camHolder;
    [Range(0f, 1f)] public float zoom = 0f;
    [Range(0f, 1f)] public float newZoom;
    public float scrollSpeed = 1f;
    public float zoomSpeed = 5f;

    public Vector2 minMaxFov;
    public float fov;

    public AnimationCurve camRotCurve;
    public float defaultRot;
    public float minRot;


    private void Start() {
        camHolder = transform;
        cam = Camera.main;

        newZoom = zoom;

        fov = (minMaxFov.y - minMaxFov.x) * zoom + minMaxFov.x;
        cam.fieldOfView = fov;
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        newZoom = Mathf.Clamp01(newZoom + scrollInput * scrollSpeed);
        zoom = Mathf.Lerp(zoom, newZoom, Time.deltaTime * zoomSpeed);

        fov = (minMaxFov.y - minMaxFov.x) * zoom + minMaxFov.x;
        cam.fieldOfView = fov;


        float newRot = camRotCurve.Evaluate(zoom) * (defaultRot - minRot) + minRot;
        camHolder.localRotation = Quaternion.Euler(newRot, camHolder.eulerAngles.y, camHolder.eulerAngles.z);

    }

    void HandleFov() {
        
    }
}
