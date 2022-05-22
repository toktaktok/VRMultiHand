using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatParticleSystem : MonoBehaviour
{

    public ParticleSystem heatParticleSystem;

    [Range(5, 20)]
    public float diffusivity = 10f; //열원으로부터 열에너지 확산도

    void Start()
    {
        
    }

    void Update()
    {

        var particleNoise = heatParticleSystem.noise;
        particleNoise.strength = diffusivity;

        var PSMain = heatParticleSystem.main;
            PSMain.gravityModifier = diffusivity * -.5f;

    }
}
