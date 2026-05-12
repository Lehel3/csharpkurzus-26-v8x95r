using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform followTarget;
    public float camHeight;

    public bool smoothFollow;
    public float followSpeed;

    private void Start() {
    }

    private void Update() {

    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    void FollowTarget() {
        if (followTarget == null) return;

        Vector3 targetPos = followTarget.position + Vector3.up * camHeight;

        if (smoothFollow) {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

        }
        else {
            transform.position = targetPos;
        }
    }
}
