using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllingAspectRatio : MonoBehaviour
{
    private float targetaspect;
    private float windowaspect;
    private float scaleheight;

    private float newWindowaspect;
    private float newScaleheight;

    private float screenWidth;
    private float screenHeight;
    private float newScreenWidth;
    private float newScreenHeight;
    //private Camera camera;

    // Use this for initialization
    void Start () 
    {
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        targetaspect = 16.0f / 9.0f;

        // determine the game window's current aspect ratio
        windowaspect = (float)Screen.width / (float)Screen.height;
        screenWidth = (float)Screen.width;
        screenHeight = (float)Screen.height;
        // current viewport height should be scaled by this amount
        scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {  
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
            
            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }


    void Update()
    {
        newScreenWidth = (float)Screen.width;
        newScreenHeight = (float)Screen.height;

        newWindowaspect = (float)Screen.width / (float)Screen.height;

        // newScaleheight = newWindowaspect / targetaspect;
        //     if (newScaleheight < 1.0f)
        //     {  
        //         Rect rect = GetComponent<Camera>().rect;

        //         rect.width = 1.0f;
        //         rect.height = newScaleheight;
        //         rect.x = 0;
        //         rect.y = (1.0f - newScaleheight) / 2.0f;
                
        //         GetComponent<Camera>().rect = rect;
        //     }
        //     else // add pillarbox
        //     {
        //         float scalewidth = 1.0f / newScaleheight;

        //         Rect rect = GetComponent<Camera>().rect;

        //         rect.width = scalewidth;
        //         rect.height = 1.0f;
        //         rect.x = (1.0f - scalewidth) / 2.0f;
        //         rect.y = 0;

        //         GetComponent<Camera>().rect = rect;
        //     }

        if(screenWidth != newScreenWidth || screenHeight != newScreenHeight)
        {
            
            newScaleheight = newWindowaspect / targetaspect;
            if (newScaleheight < 1.0f)
            {  
                Rect rect = GetComponent<Camera>().rect;

                rect.width = 1.0f;
                rect.height = newScaleheight;
                rect.x = 0;
                rect.y = (1.0f - newScaleheight) / 2.0f;
                
                GetComponent<Camera>().rect = rect;
            }
            else // add pillarbox
            {
                float scalewidth = 1.0f / newScaleheight;

                Rect rect = GetComponent<Camera>().rect;

                rect.width = scalewidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scalewidth) / 2.0f;
                rect.y = 0;

                GetComponent<Camera>().rect = rect;
            }
            screenWidth = newScreenWidth;
            screenHeight = newScreenHeight;
        }
        

    }
}
