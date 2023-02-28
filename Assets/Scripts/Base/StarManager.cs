using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    CameraParticle cameraParticle;

    public void EnableParticle()
    {
        cameraParticle = Camera.main.GetComponent<CameraParticle>();
        cameraParticle.PlayWinParticle();
    }
}
