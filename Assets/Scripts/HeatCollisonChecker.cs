using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatCollisonChecker : MonoBehaviour
{
    public ParticleSystem particleLauncher;
    public ParticleSystem heatParticles;


    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Collide!");
    }
}
