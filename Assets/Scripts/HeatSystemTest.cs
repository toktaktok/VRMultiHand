using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeatSystemTest : MonoBehaviour
{
    public TextMeshProUGUI heatTextMesh;


    float timer;
    float checkingTime = 2f;

    public float receivedDiffusivity = 0; //열에너지 확산도
    public float heatDamping; //damp heat energy by distance

    float receivedHeatEnergy;
    public float receivedHeatPerCheckTime;
    public float feelingHeat;
    float baseHeat = 36.5f;
    float previousHeatEnergy = 36.5f;

    Renderer feelingRenderer;




    void Start()
    {
        feelingRenderer = gameObject.GetComponent<Renderer>();
    }

    void OnParticleCollision(GameObject other)
    {

        //if(receivedDiffusivity == 0)
        receivedDiffusivity = other.GetComponent<HeatParticleSystem>().diffusivity;
        heatDamping = (receivedDiffusivity * 1.5f - Vector3.Distance(transform.position,other.transform.position)) / receivedDiffusivity;

        if (heatDamping < 0)
            heatDamping = 0;

        receivedHeatEnergy += 1f * heatDamping;


    }


    void Update()
    {


        timer += Time.deltaTime;

        if (checkingTime < timer)
        {
            timer = 0;
            
            float changedAmoundOfHeat = receivedHeatEnergy - previousHeatEnergy;

            if (changedAmoundOfHeat > 2)
                feelingRenderer.material.color = Color.red;
            else if (changedAmoundOfHeat < -5)
                feelingRenderer.material.color = Color.blue;
            else
                feelingRenderer.material.color = Color.grey;
            changedAmoundOfHeat = 0;

            receivedHeatPerCheckTime = receivedHeatEnergy;
            previousHeatEnergy = receivedHeatEnergy;

            receivedHeatEnergy = 0;
        }


        if(feelingHeat < receivedHeatPerCheckTime - 2)
        {
            feelingHeat += Mathf.Abs(feelingHeat - receivedHeatPerCheckTime) * .8f * Time.deltaTime;

        }
        else if (feelingHeat > receivedHeatPerCheckTime + 2)
        {
            feelingHeat -= Mathf.Abs(feelingHeat - receivedHeatEnergy) * .8f * Time.deltaTime;
        }


        if(feelingHeat < baseHeat - 2)
            feelingHeat += .05f;
        else if(feelingHeat > baseHeat + 2)
            feelingHeat -= .05f;



        heatTextMesh.text = Mathf.RoundToInt(feelingHeat + baseHeat).ToString();

    }

}
