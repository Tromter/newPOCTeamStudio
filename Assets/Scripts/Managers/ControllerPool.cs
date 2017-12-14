using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class ControllerPool : MonoBehaviour {

	public static ControllerPool me;

	public bool debugHas4 = false;

	public InputDevice[] connectedDevices;
	public int numConnected = 0;
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

	void Awake(){
		if (me == null) {
			me = this;
		} else {
			Destroy (this.gameObject);
		}
	}

	void Start () {
		DontDestroyOnLoad (this.gameObject);
		connectedDevices = new InputDevice[4];
	}
	
	// Update is called once per frame
	void Update () {
		fourConnected = true;
		int tempNum = 0;
		for (int i = 0; i < connectedDevices.Length; i++) {
			
			if (connectedDevices [i] == null) {
				fourConnected = false;
			} else {
				tempNum += 1;
			}
		}
		numConnected = tempNum;
		if (debugHas4)
			numConnected = 4;
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
