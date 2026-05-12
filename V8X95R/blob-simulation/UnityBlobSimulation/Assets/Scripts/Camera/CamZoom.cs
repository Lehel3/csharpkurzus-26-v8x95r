using UnityEngine;


[ExecuteInEditMode]
public class CamZoom : MonoBehaviour {
    [SerializeField] private Transform camHolder;
    [SerializeField] private Camera cam;
    
    [Range(0f, 1f)] public float zoom = 0f;
    [Range(0f, 1f)] public float newZoom;

    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private float zoomSpeed = 5f;

    [SerializeField] private float dist;
    [SerializeField] private float maxZoomDist;
    [SerializeField] private float minZoomDist;

    float scrollInput;

    private void Start() {
        if (!camHolder) camHolder = transform;
        if (!cam) cam = Camera.main;

        newZoom = zoom;

        HandleDist(zoom);
    }

    void Update() {
        scrollInput = Input.GetAxis("Mouse ScrollWheel");

        float zoomFactor = Mathf.Lerp(1f, 0.1f, zoom);

        newZoom = Mathf.Clamp01(newZoom + scrollInput * scrollSpeed * zoomFactor);
        zoom = Mathf.Lerp(zoom, newZoom, Time.deltaTime * zoomSpeed);
        
        HandleDist(zoom);

    }

    void HandleDist(float t) {
        //dist = Mathf.Lerp(minMaxDist.x, minMaxDist.y, Mathf.SmoothStep(0f, 1f, t));
        //dist = (minMaxDist.y - minMaxDist.x) * t + minMaxDist.x;
        dist = Mathf.Lerp(minZoomDist, maxZoomDist, t);
        dist = Mathf.Clamp(dist, maxZoomDist, minZoomDist);

        cam.transform.localPosition = Vector3.forward * -dist;
    }
}
