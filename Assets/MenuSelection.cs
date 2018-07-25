using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class MenuSelection : MonoBehaviour
{

    public Animator anim;
    public int option;
    public bool menu;
    private bool open;

    private bool lockLeftRight;
    private int inputTimer = 10;
    [SerializeField]
    private int inputLock = 0;


    public SelectOnInput[] menuLists;
    [SerializeField]
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


        //(P1state.DPad.Right == ButtonState.Pressed && P1prevState.DPad.Right == ButtonState.Released) || (P2state.DPad.Right == ButtonState.Pressed && P2prevState.DPad.Right == ButtonState.Released) || (P3state.DPad.Right == ButtonState.Pressed && P3prevState.DPad.Right == ButtonState.Released) || (P4state.DPad.Right == ButtonState.Pressed && P4prevState.DPad.Right == ButtonState.Released)

        if (inputLock == 0)
        {
            if ((Input.GetKeyDown(KeyCode.O) || (P1state.Buttons.Start == ButtonState.Pressed && P1prevState.Buttons.Start == ButtonState.Released) || (P2state.Buttons.Start == ButtonState.Pressed && P2prevState.Buttons.Start == ButtonState.Released) || (P3state.Buttons.Start == ButtonState.Pressed && P3prevState.Buttons.Start == ButtonState.Released) || (P4state.Buttons.Start == ButtonState.Pressed && P4prevState.Buttons.Start == ButtonState.Released)))
            {

                if (!menu)
                {
                    anim.SetBool("Menu", true);
                    menu = true;
                    anim.SetBool("lockLeftRight", false);
                    rightList = menuLists[0];
                }
                else if (menu)
                {
                    anim.SetBool("Menu", false);
                    menu = false;
                    anim.SetBool("lockLeftRight", true);
                    rightList = null;
                    anim.SetBool("SureQuit", false);
                    anim.SetBool("SureMenu", false);
                }
                inputLock = inputTimer;
            }

            if (inputLock == 0)
            {
                if (menu)
                {
                    if (anim.GetBool("lockLeftRight") == false)
                    {
                        // && P1prevState.ThumbSticks.Left.X < -0.7)
                        if ((Input.GetKeyDown(KeyCode.LeftArrow) || (P1state.Buttons.LeftShoulder == ButtonState.Pressed && P1prevState.Buttons.LeftShoulder == ButtonState.Released) || (P2state.Buttons.LeftShoulder == ButtonState.Pressed && P2prevState.Buttons.LeftShoulder == ButtonState.Released) || (P3state.Buttons.LeftShoulder == ButtonState.Pressed && P3prevState.Buttons.LeftShoulder == ButtonState.Released) || (P4state.Buttons.LeftShoulder == ButtonState.Pressed && P4prevState.Buttons.LeftShoulder == ButtonState.Released) || (P1state.DPad.Left == ButtonState.Pressed && P1prevState.DPad.Left == ButtonState.Released) || (P2state.DPad.Left == ButtonState.Pressed && P2prevState.DPad.Left == ButtonState.Released) || (P3state.DPad.Left == ButtonState.Pressed && P3prevState.DPad.Left == ButtonState.Released) || (P4state.DPad.Left == ButtonState.Pressed && P4prevState.DPad.Left == ButtonState.Released)) && menu && !open)
                        {
                            anim.SetTrigger("Left");
                            inputLock = inputTimer;
                        }
                        if ((Input.GetKeyDown(KeyCode.RightArrow) || (P1state.Buttons.RightShoulder == ButtonState.Pressed && P1prevState.Buttons.RightShoulder == ButtonState.Released) || (P2state.Buttons.RightShoulder == ButtonState.Pressed && P2prevState.Buttons.RightShoulder == ButtonState.Released) || (P3state.Buttons.RightShoulder == ButtonState.Pressed && P3prevState.Buttons.RightShoulder == ButtonState.Released) || (P4state.Buttons.RightShoulder == ButtonState.Pressed && P4prevState.Buttons.RightShoulder == ButtonState.Released) || (P1state.DPad.Right == ButtonState.Pressed && P1prevState.DPad.Right == ButtonState.Released) || (P2state.DPad.Right == ButtonState.Pressed && P2prevState.DPad.Right == ButtonState.Released) || (P3state.DPad.Right == ButtonState.Pressed && P3prevState.DPad.Right == ButtonState.Released) || (P4state.DPad.Right == ButtonState.Pressed && P4prevState.DPad.Right == ButtonState.Released)) && menu && !open)
                        {
                            anim.SetTrigger("Right");
                            inputLock = inputTimer;
                        }
                    }

                    if (inputLock == 0)
                    {
                        if (Input.GetKeyDown(KeyCode.K) || (P1state.Buttons.A == ButtonState.Pressed && P1prevState.Buttons.A == ButtonState.Released || P2state.Buttons.A == ButtonState.Pressed && P2prevState.Buttons.A == ButtonState.Released || P3state.Buttons.A == ButtonState.Pressed && P3prevState.Buttons.A == ButtonState.Released || P4state.Buttons.A == ButtonState.Pressed && P4prevState.Buttons.A == ButtonState.Released) && menu)

                        {
                            if (!open)
                            {
                                if (option == 1) //Resume
                                {

                                    anim.SetBool("Menu", false);
                                    menu = false;

                                }

                                if (option == 5) //Back to lobby
                                {
                                    anim.SetBool("SureMenu", true);

                                    rightList = menuLists[5];
                                    anim.SetBool("lockLeftRight", true);
                                    SelectList();

                                }


                                if (option == 6) //Quit Game
                                {
                                    anim.SetBool("SureQuit", true);
                                    rightList = menuLists[4];
                                    anim.SetBool("lockLeftRight", true);
                                    SelectList();

                                }


                            }
                            else if (open)
                            {
                                anim.SetBool("Open", false);
                                open = false;
                            }
                            inputLock = inputTimer;
                        }
                    }
                }
            }
        }
        if (inputLock > 0)
        {
            inputLock--;
        }
    }

    public void Option1() //Resume
    {
        option = 1;
        anim.SetFloat("Option", 1f);
        rightList = menuLists[0];
        SelectList();

    }
    public void Option2() //settings
    {
        option = 2;
        anim.SetFloat("Option", 2f);
        rightList = menuLists[1];
        SelectList();
    }
    public void Option3() //Controls
    {
        option = 3;
        anim.SetFloat("Option", 3f);
        rightList = menuLists[2];
        SelectList();
    }
    public void Option4() //Credits
    {
        option = 4;
        anim.SetFloat("Option", 4f);
        rightList = menuLists[3];
        SelectList();
    }
    public void Option5() //Mainmenu
    {
        option = 5;
        anim.SetFloat("Option", 5f);
    }
    public void Option6() //Quit
    {
        option = 6;
        anim.SetFloat("Option", 6f);
    }

    public void QuitButtonNo()
    {
        anim.SetBool("SureQuit", false);
        anim.SetBool("lockLeftRight", false);
        rightList.enabled = false;
        rightList = null;
    }
    public void MenuButtonNo()
    {
        anim.SetBool("SureMenu", false);
        anim.SetBool("lockLeftRight", false);

        rightList.enabled = false;
        rightList = null;
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
