using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XInputDotNetPure;

public class SelectOnInput : MonoBehaviour
{

    private int selected = 0;

    Button[] menuButtons;

    private bool initialized = false;
    private bool isFirstFrame = true;

    GamePadState P1state;
    GamePadState P1prevState;
    GamePadState P2state;
    GamePadState P2prevState;
    GamePadState P3state;
    GamePadState P3prevState;
    GamePadState P4state;
    GamePadState P4prevState;

    private AudioScript audioScript;
    private AudioClip currentAudioClip;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        audioSource = gameObject.GetComponent<AudioSource>();
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

        ////up
        //|| (P1state.ThumbSticks.Left.Y > 0.3f && P1prevState.ThumbSticks.Left.Y < 0.3f) || (P2state.ThumbSticks.Left.Y > 0.3f && P2prevState.ThumbSticks.Left.Y < 0.3f) || (P3state.ThumbSticks.Left.Y > 0.3f && P3prevState.ThumbSticks.Left.Y < 0.3f) || (P4state.ThumbSticks.Left.Y > 0.3f && P4prevState.ThumbSticks.Left.Y < 0.3f)

        ////down
        //|| (P1state.ThumbSticks.Left.Y < -0.3f && P1prevState.ThumbSticks.Left.Y > -0.3f) || (P2state.ThumbSticks.Left.Y < -0.3f && P2prevState.ThumbSticks.Left.Y > -0.3f) || (P3state.ThumbSticks.Left.Y < -0.3f && P3prevState.ThumbSticks.Left.Y > -0.3f) || (P4state.ThumbSticks.Left.Y < -0.3f && P4prevState.ThumbSticks.Left.Y > -0.3f)


        if (isFirstFrame == false)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || (P1state.DPad.Down == ButtonState.Pressed && P1prevState.DPad.Down == ButtonState.Released) || (P2state.DPad.Down == ButtonState.Pressed && P2prevState.DPad.Down == ButtonState.Released) || (P3state.DPad.Down == ButtonState.Pressed && P3prevState.DPad.Down == ButtonState.Released) || (P4state.DPad.Down == ButtonState.Pressed && P4prevState.DPad.Down == ButtonState.Released) || (P1state.ThumbSticks.Left.Y < -0.3f && P1prevState.ThumbSticks.Left.Y > -0.3f) || (P2state.ThumbSticks.Left.Y < -0.3f && P2prevState.ThumbSticks.Left.Y > -0.3f) || (P3state.ThumbSticks.Left.Y < -0.3f && P3prevState.ThumbSticks.Left.Y > -0.3f) || (P4state.ThumbSticks.Left.Y < -0.3f && P4prevState.ThumbSticks.Left.Y > -0.3f))
            {
                //eventSystem.SetSelectedGameObject(selectedObject);
                //buttonSelected = true;
                if (selected < menuButtons.Length - 1)
                {
                    selected++;
                    menuButtons[selected].Select();
                    playSound();
                }

            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || (P1state.DPad.Up == ButtonState.Pressed && P1prevState.DPad.Up == ButtonState.Released) || (P2state.DPad.Up == ButtonState.Pressed && P2prevState.DPad.Up == ButtonState.Released) || (P3state.DPad.Up == ButtonState.Pressed && P3prevState.DPad.Up == ButtonState.Released) || (P4state.DPad.Up == ButtonState.Pressed && P4prevState.DPad.Up == ButtonState.Released) || (P1state.ThumbSticks.Left.Y > 0.3f && P1prevState.ThumbSticks.Left.Y < 0.3f) || (P2state.ThumbSticks.Left.Y > 0.3f && P2prevState.ThumbSticks.Left.Y < 0.3f) || (P3state.ThumbSticks.Left.Y > 0.3f && P3prevState.ThumbSticks.Left.Y < 0.3f) || (P4state.ThumbSticks.Left.Y > 0.3f && P4prevState.ThumbSticks.Left.Y < 0.3f))
            {
                if (selected > 0)
                {
                    selected--;
                    menuButtons[selected].Select();
                    playSound();
                }

            }

            if (Input.GetKeyDown(KeyCode.K) || (P1state.Buttons.A == ButtonState.Pressed && P1prevState.Buttons.A == ButtonState.Released) || (P2state.Buttons.A == ButtonState.Pressed && P2prevState.Buttons.A == ButtonState.Released) || (P3state.Buttons.A == ButtonState.Pressed && P3prevState.Buttons.A == ButtonState.Released) || (P4state.Buttons.A == ButtonState.Pressed && P4prevState.Buttons.A == ButtonState.Released))
            {
                playConfirmSound();
                menuButtons[selected].onClick.Invoke();
            }
            

        }
        isFirstFrame = false;
    }

    private void OnEnable()
    {

        selected = 0;
        if (initialized == true)
        {
            menuButtons[selected].Select();
        }
        isFirstFrame = true;
    }

    //private void OnDisable()
    //{
    //    selected = 0;
    //    menuButtons[selected].Select();
    //}

    private void playSound()
    {
        currentAudioClip = audioScript.menuClick;
        audioSource.clip = currentAudioClip;
        audioSource.Play();
    }
    private void playConfirmSound()
    {
        currentAudioClip = audioScript.menuConfirm;
        audioSource.clip = currentAudioClip;
        audioSource.Play();
    }

}
