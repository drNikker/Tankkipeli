using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperHexagon : MonoBehaviour
{
    public Animator animator;

    private bool timerUntilDrop;
    [Space(10)]
    public float timerUntilDropTime;

    void Start()
    {
        timerUntilDrop = true;
    }

    void Update()
    {
        if (timerUntilDrop)
        {
            TimerUntilDrop();
        }
    }

    private void TimerUntilDrop()
    {
        timerUntilDropTime -= Time.deltaTime;

        if (timerUntilDropTime <= 0)
        {
            switch (gameObject.tag)
            {
                case "Fist1":
                    animator.SetTrigger("1");
                    break;
                case "Fist2":
                    animator.SetTrigger("2");
                    break;
                case "Fist3":
                    animator.SetTrigger("3");
                    break;
                case "Fist4":
                    animator.SetTrigger("4");
                    break;
                case "Untagged":
                    animator.SetTrigger("5");
                    break;
            }

            timerUntilDropTime = 0;
            timerUntilDrop = false;
        }
    }
}
