using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoomDist : MonoBehaviour
{
    public Transform camHolder;
    public Transform camTransform;
    public Camera cam;
    [Range(0f, 1f)] public float zoom = 0f;
    [Range(0f, 1f)] public float newZoom;
    public float scrollSpeed = 1f;
    public float zoomSpeed = 5f;

    public Vector2 minMaxDist;
    public float dist;

    public float fov;

    public AnimationCurve camRotCurve;
    public float defaultRot;
    public float minRot;


    private void Start() {
        camHolder = transform;
        cam = Camera.main;

        newZoom = zoom;

        HandleDist();
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        newZoom = Mathf.Clamp01(newZoom + scrollInput * scrollSpeed);
        zoom = Mathf.Lerp(zoom, newZoom, Time.deltaTime * zoomSpeed);

        HandleDist();
        HandleTilt();
        

    }

    void HandleDist() {
        dist = (minMaxDist.y - minMaxDist.x) * zoom + minMaxDist.x;
        camTransform.localPosition = Vector3.forward * -dist;
    }

    void HandleTilt() {
        float newRot = camRotCurve.Evaluate(zoom) * (defaultRot - minRot) + minRot;
        camHolder.localRotation = Quaternion.Euler(newRot, camHolder.eulerAngles.y, camHolder.eulerAngles.z);
    }

#if UNITY_EDITOR
    private void OnValidate() {
        if (cam != null) {
            cam.fieldOfView = fov;
        }

        HandleDist();
        HandleTilt();
    }
#endif
}
