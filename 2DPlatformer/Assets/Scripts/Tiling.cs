using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(SpriteRenderer))]  //We demand that a SpriteRenderer is used here so that we don't have render errors


public class Tiling : MonoBehaviour
{
    public int offsetX = 2; //the offset so that we don't get any weird errors

    //these two are used for checking if we need to instantiate stuff
    public bool hasARightPart = false;
    public bool hasALeftPart = false;

    public bool reverseScale = false;   //used for the objects that are not tillable

    private float spriteWidth = 0f; //the width of our element
    private Camera camera;
    private Transform myTransform;  //storing object's transformation into a variable (good practice)

    //Is called before Start(). Great for references
    void Awake ()
    {
        camera = Camera.main;
        myTransform = transform;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();     //get the first component of the Sprite Renderer
        spriteWidth = sRenderer.sprite.bounds.size.x;

    }


    // Update is called once per frame
    void Update()
    {
        //does it still need parts? If not, do nothing
        if(hasALeftPart == false || hasARightPart == false)
        {
            //calculate he camera's extend (half the width) of what the camera can see in world coordinates
            float cameraHorizontalExtend = camera.orthographicSize * Screen.width/Screen.height;
            
            //calculate the x position where the camera can see the edge of the sprite (element)
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - cameraHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth/2) + cameraHorizontalExtend;

            //checking if we can see the edge of the element and then calling MakeNewPart if we can
            if (camera.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightPart == false)
            {
                MakeNewPart(1);
                hasARightPart = true;
            }
            else if (camera.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftPart == false)
            {
                MakeNewPart(-1);
                hasALeftPart = true;
            }
        }
    }


    //Creates a part on the side required
    void MakeNewPart(int rightOrLeft)
    {
        //calculating the new position for the new part
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        //instantiating the new part and storing it in a variable
        Transform newPart = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;  //transformation, position and rotation

        //if not tillable, reverse the x size of the object to get rid of ugly seams
        if (reverseScale == true)
        {
            newPart.localScale = new Vector3(newPart.localScale.x*-1, newPart.localScale.y, newPart.localScale.z);
        }

        newPart.parent = myTransform.parent;
        if (rightOrLeft > 0)
        {
            newPart.GetComponent<Tiling>().hasALeftPart = true;
        }
        else
        {
            newPart.GetComponent<Tiling>().hasARightPart = true;
        }

    }
}
