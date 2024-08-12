using UnityEngine;

public class ControllingAspectRatio : MonoBehaviour
{
    private float _targetaspect;
    private float _windowaspect;
    private float _scaleheight;
    private float _screenWidth;
    private float _screenHeight;
    private Camera _cam;

    private void Start () 
    {
        _targetaspect = 16.0f / 9.0f;

        _windowaspect = (float)Screen.width / (float)Screen.height;
        _screenWidth = (float)Screen.width;
        _screenHeight = (float)Screen.height;

        _scaleheight = _windowaspect / _targetaspect;

        _cam = GetComponent<Camera>();

        UpdateCameraSize(_scaleheight);
    }

    private void Update()
    {
        float newScreenWidth = (float)Screen.width;
        float newScreenHeight = (float)Screen.height;

        float newWindowaspect = (float)Screen.width / (float)Screen.height;

        if(_screenWidth != newScreenWidth || _screenHeight != newScreenHeight)
        { 
            float newScaleheight = newWindowaspect / _targetaspect;
            UpdateCameraSize(newScaleheight);

            _screenWidth = newScreenWidth;
            _screenHeight = newScreenHeight;
        }
    }
    
    private void UpdateCameraSize(float scaleheight)
    {
        if (scaleheight < 1.0f)
        {  
            Rect rect = _cam.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
            
            _cam.rect = rect;
        }
        else
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = _cam.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            _cam.rect = rect;
        }
    }
}
