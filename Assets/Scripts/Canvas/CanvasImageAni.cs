using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasImageAni : MonoBehaviour
{

    public List<Sprite> frames;

    public float fps;
    private Image myImage;
    public int currentFrame;
    public bool reverse;
    public bool animate = true;

    // Use this for initialization
    void Start()
    {
        myImage = gameObject.GetComponent<Image>();
        StartCoroutine(TimeUpdate());
    }

    private IEnumerator TimeUpdate()
    {
        if (animate)
        {
            if (!reverse)
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
        }

        yield return new WaitForSecondsRealtime(1/fps);
        StartCoroutine(TimeUpdate());
    }
}
