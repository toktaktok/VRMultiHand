using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{

    public class Point
    {
        public Vector2 position, prevPosition;
        public bool locked;

    }

    public class Stick
    {
        public Point pointA, pointB;
        public float length;

        public Stick(Point pointA, Point pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
            length = Vector2.Distance(pointA.position, pointB.position);
        }
    }


    float gravity;
    List<Point> points;
    List<Stick> sticks;

    int numIterations = 5;
    public bool constrainStickMinLength = true;
    

 
    void Start()
    {
        if (points == null)
            points = new List<Point>();
        if (sticks == null)
            sticks = new List<Stick>();

        gravity = 9.8f;
    }

    void Simulate()
    {
        foreach (Point p in points)
        {
            if (!p.locked)
            {
                Vector2 positionBeforeUpdate = p.position;
                p.position += p.position - p.prevPosition;
                p.position += Vector2.down * gravity * Time.deltaTime * Time.deltaTime;
                p.prevPosition = positionBeforeUpdate;
            }
        }

        for(int i = 0; i < numIterations; i++)
        {
            foreach (Stick stick in sticks)
            {

                Vector2 stickCentre = (stick.pointA.position + stick.pointB.position) / 2;
                Vector2 stickDir = (stick.pointA.position - stick.pointB.position).normalized;
                float length = (stick.pointA.position - stick.pointB.position).magnitude;

                if(length > stick.length || constrainStickMinLength)
                {
                    if (!stick.pointA.locked)
                        stick.pointA.position = stickCentre + stickDir * stick.length / 2;

                    if (!stick.pointB.locked)
                        stick.pointB.position = stickCentre - stickDir * stick.length / 2;
                }
          
            }
        }
        
    }
}
