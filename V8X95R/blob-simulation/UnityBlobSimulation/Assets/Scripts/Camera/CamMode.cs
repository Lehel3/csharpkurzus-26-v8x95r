using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMode : MonoBehaviour
{
    int modes = 2;
    int modeNum = 0;
    CamFollow camFollow;
    CameraMouseMove camMouseMove;

    private void Start() {
        camFollow = GetComponent<CamFollow>();
        camMouseMove = GetComponent<CameraMouseMove>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Y)) {
            SwitchMode();
        }

        if (modeNum == 1) {
            if (Input.GetKey(KeyCode.Space)) {
                camFollow.enabled = true;
            }
            else {
                camFollow.enabled = false;
            }
        }
    }

    void SwitchMode() {
        modeNum++;
        if(modeNum == modes) modeNum = 0;

        if (modeNum == 0) {
            camFollow.enabled = true; camMouseMove.enabled = false;
        }
        else if (modeNum == 1) {
            camFollow.enabled = false; camMouseMove.enabled = true;
        }
    }
}
