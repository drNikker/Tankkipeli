using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour {


    public ParticleSystem Bumper;

    public void Bump()
    {
        Bumper.Play();
    }
}
