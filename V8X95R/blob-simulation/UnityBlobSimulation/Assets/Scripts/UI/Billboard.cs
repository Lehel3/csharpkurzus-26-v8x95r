using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform cam;

    [Header("Size")]
    public float size = 1f;
    public bool keepSize;
    float sizeMult = 0.0009f;


    bool unSized;

    void Start()
    {
        cam = Camera.main.transform;
        //SetSize();
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);


        if (keepSize) {
            float distance = Vector3.Distance(transform.position, cam.transform.position);
            float fov = Camera.main.fieldOfView;
            float sizeM = size * distance * sizeMult;
            //sizeM *= fov;
            transform.localScale = Vector3.one * sizeM;
            unSized = true;
        }
        else if (unSized) {
            SetSize();
        }
    }

    void SetSize() {
        float s = size / 100f;
        transform.localScale = Vector3.one * s;
    }
}
