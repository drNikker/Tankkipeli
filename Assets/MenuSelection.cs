using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelection : MonoBehaviour {

    public Animator anim;
    public int option;
    private bool menu;
    private bool open;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("sad");
            if (!menu)
            {
                anim.SetBool("Menu", true);
                menu = true;
            }
            else if (menu)
            {
                anim.SetBool("Menu", false);
                menu = false;

            }
        }
        if (Input.GetKeyDown("a") && menu && !open)
        {
            anim.SetTrigger("Left");
        }
        if (Input.GetKeyDown("d") && menu && !open)
        {
            anim.SetTrigger("Right");
        }



        if (Input.GetKeyDown("x") && menu)

        {
            if (!open)
            {
                if (option == 1) //Resume
                {

                    anim.SetBool("Menu", false);
                    menu = false;

                }
  
                if (option == 6) //Back to lobby
                {
                    anim.SetBool("Menu", false);
                    menu = false;
                }
     
                
            }
            else if (open)
            {
                anim.SetBool("Open", false);
                open = false;
            }
        }
    }

    public void Option1() //Resume
    {
        option = 1;
        anim.SetFloat("Option", 1f);
       
        }
    public void Option2() //Video
    {
        option = 2;
        anim.SetFloat("Option", 2f);
    }
    public void Option3() //Audio
    {
        option = 3;
        anim.SetFloat("Option", 3f);
    }
    public void Option4() //Controls
    {
        option = 4;
        anim.SetFloat("Option", 4f);
    }
    public void Option5() //Credits
    {
        option = 5;
        anim.SetFloat("Option", 5f);
    }
    public void Option6() //Back to lobby
    {
        option = 6;
        anim.SetFloat("Option", 6f);
    }
}
