using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thread : MonoBehaviour
{
    public int iterations = 5;
    public float gravity = 9.8f;
    public float damping = 0.7f;
    public PathAutoEndPoints pathInfo;

    public int numPoints = 20;
    public float meshThickness = 0.3f;
    public MeshFilter meshFilter;
    public int cylinderResolution = 5;

    float pathLength;
    float pointSpacing;
    Vector3[] points;
    Vector3[] pointsOld;
    public int remainder;

    Mesh mesh;
    bool pinStart = false;
    bool pinEnd = false;

    void Start()
    {
        points = new Vector3[numPoints];
        pointsOld = new Vector3[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (numPoints - 1f);
            points[i] = pathInfo.pathCreator.path.GetPointAtTime(t, PathCreation.EndOfPathInstruction.Stop);
            pointsOld[i] = points[i];
        }

        for (int i = 0; i < numPoints - 1; i++)
        {
            pathLength += Vector3.Distance(points[i], pointsOld[i + 1]);
        }

        pointSpacing = pathLength / points.Length;
        //MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        //meshCollider.sharedMesh = mesh;


    }

    void LateUpdate()
    {
        CylinderGenerator.CreateMesh(ref mesh, points, cylinderResolution, meshThickness);
        meshFilter.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        meshFilter.mesh = mesh;
    }
    void FixedUpdate()
    {
        points[0] = pathInfo.origin.position;


        for (int i = 0; i < points.Length; i++)
        {
            //bool pinned = (i == 0 && pinStart) || (i == points.Length - 1 && pinEnd);
            

            //if (!pinned)
            //{
                Vector3 curr = points[i];
                points[i] = points[i] + (points[i] - pointsOld[i]) * damping + Vector3.down * gravity * Time.deltaTime * Time.deltaTime;
                pointsOld[i] = curr;
            //}
        }

        for (int i = 0; i < iterations; i++)
        {
            ConstrainCollisions();
            ConstrainConnections();
        }

        
    }

    void ConstrainConnections()
    {
        for(int i = 0; i < points.Length; i++)
        {
            remainder = (i == points.Length - 1) ? 0 : i + 1;

            Vector3 centre = (points[i] + points[remainder]) / 2;
            Vector3 offset = points[i] - points[remainder];
            float length = offset.magnitude;

            Vector3 dir = offset / length;


            if(i != 0 || !pinStart)
            {
                points[i] = centre + dir * pointSpacing / 2;

            }

            if (i + 1 != points.Length - 1 || !pinEnd)
            {
                points[remainder] = centre - dir * pointSpacing / 2;
            }


        }

    }

    void ConstrainCollisions()
    {

        for(int i = 0; i < points.Length; i++)
        {

            bool pinned = i == 0 || i == points.Length - 1;

            if(!pinned)
            {
                if (points[i].y < meshThickness / 2)
                    points[i].y = meshThickness / 2;
            }

            if (points[i].y < 0)
                points[i].y = 0;
        }
    }
}
