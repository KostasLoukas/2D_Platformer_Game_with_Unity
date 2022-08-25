using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{

    public Transform[] backgrounds;  //Array (list) with all the back- and foregrounds to be parallax-ed
    public float[] parallaxScales;   //The proportion of the camera's movement to move the backgrounds by
    public float smoothing = 1f;    //How smooth the parallax is going to be (must be above zero)

    private Transform camera;                //Reference to the main camera's transform
    private Vector3 previousCameraPosition;  //The position of the camera in the previous frame

    //It is called before Start(). Great for references
    void Awake ()
    {
        //setup the camera reference
        camera = Camera.main.transform;

    }


    // Start is called before the first frame update
    void Start()
    {
        //store the previous frame which had the current frame's position
        previousCameraPosition = camera.position;

        //Assigning corresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0 ; i<backgrounds.Length ; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z*-1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //For each background
        for (int i = 0 ; i<backgrounds.Length ; i++)
        {
            //The parallax is the opposite of the camera movement because the previous frame multiplied by the scale
            float parallax = (previousCameraPosition.x - camera.position.x)*parallaxScales[i];

            //Set a target x position which is the current position plus the parallax
            float backgroundTargetPositionX = backgrounds[i].position.x + parallax;

            //Create a target position which is the background's current position with it's target x position
            Vector3 backgroundTargetPosition = new Vector3 (backgroundTargetPositionX, backgrounds[i].position.y, backgrounds[i].position.z);

            //Fade between current position and the target position using Lerp
            backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPosition, smoothing*Time.deltaTime);

        }

        //Set the previous camera position to the camera's position at the end of the frame
        previousCameraPosition = camera.position;
    }
}
