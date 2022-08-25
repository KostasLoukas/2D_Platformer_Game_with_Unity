using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{

    public int rotationOffset = 90;
    // Update is called once per frame
    void Update()
    {
        //This is the subtraction of the position of our mouse and the position of our character
        Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
        difference.Normalize();   //Normalizing the vector, meaning that the sum of the vector will be equal to one

        //Finding the angle and converting to degrees
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + rotationOffset);

        
    }
}
