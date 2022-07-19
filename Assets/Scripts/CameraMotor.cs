using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform target;
    //boundX and boundY is the set the camera only move when the target moves too far from the camera range
    public float boundX = 0.3f;
    public float boundY = 0.3f;
    
    private void LateUpdate() 
    {
        Vector3 delta = Vector3.zero;
        
        //find the difference between x coord
        float deltaX = target.transform.position.x - transform.position.x;

        if(Mathf.Abs(deltaX) > boundX)
        {   
            //target is on the right side
            if(transform.position.x < target.transform.position.x)
            {
                //move camera to the right
                delta.x = deltaX - boundX;
            }
            //target is on the left side
            else
            {
                //move camera to the left
                delta.x = deltaX + boundX;
            }
        }
        
        //find the difference between y coord
        float deltaY = target.transform.position.y - transform.position.y;

        if(Mathf.Abs(deltaY) > boundY)
        {   
            //target is on top
            if(transform.position.y < target.transform.position.y)
            {
                //move camera up
                delta.y = deltaY - boundY;
            }
            //target is on bottom 
            else
            {
                //move camera down
                delta.y = deltaY + boundY;
            }
        }

        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
