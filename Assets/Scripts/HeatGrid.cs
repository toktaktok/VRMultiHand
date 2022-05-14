using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeatGrid : MonoBehaviour
{
 
    int xWidth;
    int zWidth;
    int height;
    float cellSize;

    public int[,,] gridArray;
    public float[,,] heatArray;

    float timer;
    float delayTIme = 0.2f;
    WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);


    private Vector3[] rayDir =
        { Vector3.up, Vector3.down,
          Vector3.forward, Vector3.back,
          Vector3.right, Vector3.left};

    public HeatGrid(int xWidth, int zWidth, int height, float cellSize)
    {
        this.xWidth = xWidth;
        this.zWidth = zWidth;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[xWidth, zWidth, height];
        heatArray = new float[xWidth, zWidth, height];
        heatArray.Initialize();


    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    public Vector3 GetWorldPosition(int xWIdth, int zWidth, int height)
    {
            return new Vector3(xWIdth, zWidth, height) * cellSize;
    }

    public IEnumerator SendHeatToNearCube(int x, int z, int y)
    {
        RaycastHit hit;
        Vector3 cubePos = new Vector3(x, z, y) * 10;

        for (int i = 0; i < rayDir.Length; i++)
        {
            int[] rDirection = new int[]{x + (int)rayDir[i].x, z + (int)rayDir[i].z, y + (int)rayDir[i].y};

            if (Physics.Raycast(cubePos, rayDir[i], out hit, 1.5f))
            {
                if (heatArray[x, z, y] > heatArray[rDirection[0], rDirection[1], rDirection[2]] && heatArray[x, z, y] > 40)
                    heatArray[rDirection[0], rDirection[1], rDirection[2]]
                        = heatArray[x, z, y] * 0.6f;

                //hit.collider.GetComponent<Renderer>().material.SetColor("_Color",);
                //UpdateCube(x + rDirection[0], z + rDirection[1], y + rDirection[2], hit.collider.gameObject);
            }

            yield return waitForSeconds;
            yield return StartCoroutine(UpdateCube(x + rDirection[0], z + rDirection[1], y + rDirection[2], hit.collider.gameObject));
        }
        
    }
    
    public IEnumerator UpdateCube(int x, int z, int y, GameObject cube)
    {
        cube.GetComponent<Renderer>().material.color = new Color(1f, 0.6f, 0.2f);
        cube.GetComponentInChildren<TextMeshProUGUI>().text = heatArray[x, z, y].ToString();
        //SendHeatToNearCube(x, z, y);

        yield return waitForSeconds;
        yield return StartCoroutine(SendHeatToNearCube(x, z, y));

    }

}
