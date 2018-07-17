using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XInputDotNetPure;

public class SelectOnInput : MonoBehaviour {
    
    private int selected = 0;

    Button[] menuButtons;

    private bool initialized = false;

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
        menuButtons = this.GetComponentsInChildren<Button>();
        menuButtons[selected].Select();
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {

        

        P1prevState = P1state;
        P2prevState = P2state;
        P3prevState = P3state;
        P4prevState = P4state;
        P1state = GamePad.GetState(PlayerIndex.One);
        P2state = GamePad.GetState(PlayerIndex.Two);
        P3state = GamePad.GetState(PlayerIndex.Three);
        P4state = GamePad.GetState(PlayerIndex.Four);

        //Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false


        if ((P1state.DPad.Down == ButtonState.Pressed && P1prevState.DPad.Down == ButtonState.Released) || (P2state.DPad.Down == ButtonState.Pressed && P2prevState.DPad.Down == ButtonState.Released) || (P3state.DPad.Down == ButtonState.Pressed && P3prevState.DPad.Down == ButtonState.Released) || (P4state.DPad.Down == ButtonState.Pressed && P4prevState.DPad.Down == ButtonState.Released))
        {
            //eventSystem.SetSelectedGameObject(selectedObject);
            //buttonSelected = true;
            if (selected < menuButtons.Length - 1)
            {
                selected++;
                menuButtons[selected].Select();
            }
            
        }


        if ((P1state.DPad.Up == ButtonState.Pressed && P1prevState.DPad.Up == ButtonState.Released) || (P2state.DPad.Up == ButtonState.Pressed && P2prevState.DPad.Up == ButtonState.Released) || (P3state.DPad.Up == ButtonState.Pressed && P3prevState.DPad.Up == ButtonState.Released) || (P4state.DPad.Up == ButtonState.Pressed && P4prevState.DPad.Up == ButtonState.Released))
        {
            if (selected > 0)
            {
                selected--;
                menuButtons[selected].Select();
            }
            
        }


        if ((P1state.Buttons.A == ButtonState.Pressed && P1prevState.Buttons.A == ButtonState.Released) || (P2state.Buttons.A == ButtonState.Pressed && P2prevState.Buttons.A == ButtonState.Released) || (P3state.Buttons.A == ButtonState.Pressed && P3prevState.Buttons.A == ButtonState.Released) || (P4state.Buttons.A == ButtonState.Pressed && P4prevState.Buttons.A == ButtonState.Released))
        {
                menuButtons[selected].onClick.Invoke();
        }

    }

    private void OnEnable()
    {

        selected = 0;
        if (initialized == true)
        {
            menuButtons[selected].Select();
        }
        
    }

    //private void OnDisable()
    //{
    //    selected = 0;
    //    menuButtons[selected].Select();
    //}


}
