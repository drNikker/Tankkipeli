using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class XInputJoin : MonoBehaviour {
	public PlayerIndex playerIndex;

	public GameObject playerPrefab;
	public GameObject playerInstance;

	public GUIText text;

	uint lastPacketNumber;
	float lastPacketTime;

	void Update(){

		GamePadState currentState = GamePad.GetState( playerIndex );

		if(playerInstance == null){
			//check to see if the player pushed the A button
			if( currentState.Buttons.A == ButtonState.Pressed){
				playerInstance = (GameObject)Instantiate(playerPrefab, transform.position, transform.rotation);
				playerInstance.GetComponent<XCharacterController2D>().playerIndex = playerIndex;
				text.enabled = false;
			}
		}
		else{
			//NOTE:  Doesn't work with some XInput emulated device drivers like the popular PS3 Controller one
			//destroy the player instance if the controller disconnects
			if(currentState.IsConnected == false){
				Destroy ( playerInstance );
				text.enabled = true;
				return;
			}
			else{

				//destroy the player instance if the player pushed the Back button
				if(currentState.Buttons.Back == ButtonState.Pressed){
					Destroy ( playerInstance );
					text.enabled = true;
					return;
				}

				if(currentState.PacketNumber > lastPacketNumber){
					lastPacketNumber = currentState.PacketNumber;
					lastPacketTime = Time.time;
				}
				else{
					//NOTE:  Doesn't work with some XInput emulated device drivers like the popular PS3 Controller one
					if(Time.time - lastPacketTime > 10){
						//controller has been idle for 10 seconds
					}
				}

			}
		}
	}
}
