using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeatTesting : MonoBehaviour
{
    public GameObject heatCube;
    public Transform cubeParent;
    HeatGrid heatGrid;

    private void Start()
    {
        heatGrid = new HeatGrid(5, 5, 5, 10f);

        for (int x = 0; x < heatGrid.gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < heatGrid.gridArray.GetLength(1); z++)
            {
                for (int y = 0; y < heatGrid.gridArray.GetLength(2); y++)
                {
                    //cubeParent.position = heatGrid.GetWorldPosition(x, z, y);
                    Instantiate(heatCube, heatGrid.GetWorldPosition(x, z, y), Quaternion.identity, cubeParent);
                }
            }
        }
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ShootRay();
    }

    void ShootRay()
    {

        RaycastHit hit;
        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Camera.main.farClipPlane));

        if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, out hit, Camera.main.farClipPlane))
        {
            Debug.Log("Checked");
            Debug.Log((int)hit.transform.position.x / 10 + " " + (int)hit.transform.position.z / 10 + " " + (int)hit.transform.position.y / 10);


            //hit.collider.gameObject.GetComponent<Renderer>().material.color = new Color(1f, 0.2f, 0.2f);

            heatGrid.heatArray[(int)(hit.transform.position.x / 10), (int)(hit.transform.position.z / 10), (int)(hit.transform.position.y / 10)] += 50;
            StartCoroutine(heatGrid.UpdateCube((int)(hit.collider.transform.position.x / 10), (int)(hit.collider.transform.position.z / 10), (int)(hit.collider.transform.position.y / 10), hit.collider.transform.gameObject));
            //SetColor("_Color",Color.red); //new Color(0.5f, 0.2f, 0.2f)

            //heatGrid.SendHeatToNearCube((int)(hit.transform.position.x /10), (int)(hit.transform.position.z /10), (int)(hit.transform.position.y /10));
            
        }
    }

}
