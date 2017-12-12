using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class ControllerPool : MonoBehaviour {

	public InputDevice[] connectedDevices;
	private bool fourConnected = false;
	// Use this for initialization
	void OnEnable(){
		InputManager.OnDeviceAttached += AddControllerToPool;
		InputManager.OnDeviceDetached += RemoveControllerFromPool;

	}

	void OnDisable(){
		InputManager.OnDeviceAttached -= AddControllerToPool;
		InputManager.OnDeviceDetached -= RemoveControllerFromPool;
	}

	void Start () {
		connectedDevices = new InputDevice[4];

	}
	
	// Update is called once per frame
	void Update () {
		fourConnected = true;
		for (int i = 0; i < connectedDevices.Length; i++) {
			
			if (connectedDevices [i] == null) {
				fourConnected = false;

		}
	}
}

	void AddControllerToPool(InputDevice someDevice){
		//look for first vacancy
		if (!fourConnected && someDevice.HasControl(InputControlType.LeftStickDown)) {
			bool foundSlot = false;
			int currentCheck = 0;
			while (!foundSlot) {
				if (connectedDevices [currentCheck] == null) {
					connectedDevices [currentCheck] = someDevice;
					Debug.Log ("Device " + someDevice.Name + " connected and added as player " + (1 + currentCheck));
					foundSlot = true;
				} else {
					currentCheck += 1;
				}
				if (currentCheck > 3) {
					Debug.Log ("Controller addition failed: no slots found");
				}
			}
		}
	}

	void RemoveControllerFromPool(InputDevice someDevice){
		for (int i = 0; i < connectedDevices.Length; i++) {
			if (connectedDevices [i] == someDevice) {
				Debug.Log ("Device " + someDevice.Name + " disconnected from slot " + (1 + i));
				connectedDevices [i] = null;
			}
		}
	}
}
