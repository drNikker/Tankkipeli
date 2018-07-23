using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class MenuSelection : MonoBehaviour {

    public Animator anim;
    public int option;
    private bool menu;
    private bool open;


    public SelectOnInput[] menuLists;
    private SelectOnInput rightList;

    GamePadState P1state;
    GamePadState P1prevState;
    GamePadState P2state;
    GamePadState P2prevState;
    GamePadState P3state;
    GamePadState P3prevState;
    GamePadState P4state;
    GamePadState P4prevState;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        P1prevState = P1state;
        P2prevState = P2state;
        P3prevState = P3state;
        P4prevState = P4state;
        P1state = GamePad.GetState(PlayerIndex.One);
        P2state = GamePad.GetState(PlayerIndex.Two);
        P3state = GamePad.GetState(PlayerIndex.Three);
        P4state = GamePad.GetState(PlayerIndex.Four);


        if (((P1state.Buttons.Start == ButtonState.Pressed && P1prevState.Buttons.Start == ButtonState.Released) || (P2state.Buttons.Start == ButtonState.Pressed && P2prevState.Buttons.Start == ButtonState.Released) || (P3state.Buttons.Start == ButtonState.Pressed && P3prevState.Buttons.Start == ButtonState.Released) || (P4state.Buttons.Start == ButtonState.Pressed && P4prevState.Buttons.Start == ButtonState.Released)))
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
        // && P1prevState.ThumbSticks.Left.X < -0.7)
        if (((P1state.Buttons.LeftShoulder == ButtonState.Pressed && P1prevState.Buttons.LeftShoulder == ButtonState.Released)|| (P2state.Buttons.LeftShoulder == ButtonState.Pressed && P2prevState.Buttons.LeftShoulder == ButtonState.Released) || (P3state.Buttons.LeftShoulder == ButtonState.Pressed && P3prevState.Buttons.LeftShoulder == ButtonState.Released) || (P4state.Buttons.LeftShoulder == ButtonState.Pressed && P4prevState.Buttons.LeftShoulder == ButtonState.Released)) && menu && !open)
        {
            anim.SetTrigger("Left");
        }
        if (((P1state.Buttons.RightShoulder == ButtonState.Pressed && P1prevState.Buttons.RightShoulder == ButtonState.Released) || (P2state.Buttons.RightShoulder == ButtonState.Pressed && P2prevState.Buttons.RightShoulder == ButtonState.Released) || (P3state.Buttons.RightShoulder == ButtonState.Pressed && P3prevState.Buttons.RightShoulder == ButtonState.Released) || (P4state.Buttons.RightShoulder == ButtonState.Pressed && P4prevState.Buttons.RightShoulder == ButtonState.Released)) && menu && !open)
        {
            anim.SetTrigger("Right");
        }



        if ((P1state.Buttons.A == ButtonState.Pressed && P1prevState.Buttons.A == ButtonState.Released || P2state.Buttons.A == ButtonState.Pressed && P2prevState.Buttons.A == ButtonState.Released || P3state.Buttons.A == ButtonState.Pressed && P3prevState.Buttons.A == ButtonState.Released || P4state.Buttons.A == ButtonState.Pressed && P4prevState.Buttons.A == ButtonState.Released ) && menu )

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
        rightList = menuLists[0];
        SelectList();
       
    }
    public void Option2() //Video
    {
        option = 2;
        anim.SetFloat("Option", 2f);
        rightList = menuLists[1];
        SelectList();
    }
    public void Option3() //Audio
    {
        option = 3;
        anim.SetFloat("Option", 3f);
        rightList = menuLists[2];
        SelectList();
    }
    public void Option4() //Controls
    {
        option = 4;
        anim.SetFloat("Option", 4f);
        rightList = menuLists[3];
        SelectList();
    }
    public void Option5() //Credits
    {
        option = 5;
        anim.SetFloat("Option", 5f);
        rightList = menuLists[4];
        SelectList();
    }
    public void Option6() //Back to lobby
    {
        option = 6;
        anim.SetFloat("Option", 6f);
        rightList = menuLists[5];
        SelectList();
    }
    

    void SelectList()
    {
        for (int i = 0; i < menuLists.Length; i++)
        {
            if (menuLists[i] != rightList)
            {
                menuLists[i].enabled = false;
            }
            else
            {
                menuLists[i].enabled = true;
            }
        }
    }
}
