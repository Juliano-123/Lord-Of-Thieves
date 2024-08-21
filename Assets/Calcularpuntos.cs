using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Calcularpuntos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        int number = 14; // how many number to be placed
        int size = 3; // size of circle i.e. w = h = 260
        double cx = size / 2; // center of x(in a circle)
        double cy = size / 2; // center of y(in a circle)
        double r = size / 2; // radius of a circle

        for (int i = 1; i <= number; i++)
        {
            double ang = i * (Math.PI / (number / 2));
            double left = cx + (r * Math.Cos(ang));
            double top = cy + (r * Math.Sin(ang));
            Debug.Log(top +" "+ left);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DrawCirclePoints(int points, double radius, Point center)
    {




        //double slice = 2 * Math.PI / points;
        //for (int i = 0; i < points; i++)
        //{
        //    double angle = slice * i;
        //    int newX = (int)(center.X + radius * Math.Cos(angle));
        //    int newY = (int)(center.Y + radius * Math.Sin(angle));
        //    Point p = new Point(newX, newY);
        //    Debug.Log(p);
        //}
    }



}
