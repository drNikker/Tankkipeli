using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasImageAni : MonoBehaviour
{

    public List<Sprite> frames;

    public float fps;
    public Image myImage;
    public int currentFrame;
    public bool inverse;
    public bool animate = true;



    // Use this for initialization
    void Start()
    {
        myImage = gameObject.GetComponent<Image>();
        ChangeFrame();
    }

    // Update is called once per frame
    void ChangeFrame()
    {
        if (!inverse)
        {
            if (frames.Count - 1 > currentFrame)
            {
                currentFrame += 1;
            }
            else
            {
                currentFrame = 0;
            }
        }
        else
        {
            if (0 < currentFrame)
            {
                currentFrame -= 1;
            }
            else
            {
                currentFrame = frames.Count - 1;
            }
        }
        myImage.sprite = frames[currentFrame];

        if (animate)
        {
            Invoke("ChangeFrame", (float)1/fps);
        }
    }
    
}
