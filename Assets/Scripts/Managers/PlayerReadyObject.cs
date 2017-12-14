using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public class PlayerReadyObject : MonoBehaviour {

	public AudioClip swapClip;
	public AudioClip readyClip;

	public SubModifier dashReference;
	public GameObject dashImage;
	public SubModifier shieldReference;
	public GameObject shieldImage;
	public SubModifier teleReference;
	public GameObject teleImage;
	public SubModifier chosenSubmodifier;

	public int playerID = 0;
	public bool controllerAvail = false;
	public bool lockedIn = false;

	public int viewingMod {
		get {
			return a_viewingMod;
		}
		set {
			if (value < 0) a_viewingMod = 2;
			else if (value > 2) a_viewingMod = 0;
			else a_viewingMod = value;
		}
	}
	private int a_viewingMod = 0;

	public Color notLockedColor;
	public Color lockedColor;
	public Color noControllerColor;
	private Image bgImage;

//	bool upHit = false;
//	bool downHit = false;
	bool scrollHit = false;
	bool submitHit = false;
	// Use this for initialization
	void Start () {
		chosenSubmodifier = dashReference;

		bgImage = this.GetComponent<Image>();

        bgImage.color = notLockedColor;

	}
	
	// Update is called once per frame
	void Update () {
		if (ControllerPool.me.numConnected >= playerID){
			controllerAvail = true;

//			upHit = InputManager.Devices[playerID - 1].DPadUp.WasPressed;
//			downHit = InputManager.Devices[playerID - 1].DPadDown.WasPressed;
			scrollHit = ControllerPool.me.connectedDevices[playerID - 1].Action2.WasPressed;
			submitHit = ControllerPool.me.connectedDevices[playerID - 1].Action1.WasPressed;

            if (lockedIn)
            {
				bgImage.color = lockedColor;
				if (scrollHit)
                {
                    lockedIn = false;
                    
					Sound.me.Play (swapClip);
                }
            }
            else
            {
				bgImage.color = notLockedColor;
				if (scrollHit)
                {
                    viewingMod += 1;
					Sound.me.Play (swapClip);
                }
                if (submitHit)
                {
                    if (viewingMod == 0)
                        chosenSubmodifier = dashReference;
                    else if (viewingMod == 1)
                        chosenSubmodifier = shieldReference;
                    else if (viewingMod == 2)
                        chosenSubmodifier = teleReference;

					Sound.me.Play (readyClip);
                    lockedIn = true;
                }
                if (viewingMod == 0)
                {
                    dashImage.SetActive(true);
                    shieldImage.SetActive(false);
                    teleImage.SetActive(false);
                }
                else if (viewingMod == 1)
                {
                    dashImage.SetActive(false);
                    shieldImage.SetActive(true);
                    teleImage.SetActive(false);
                }
                else if (viewingMod == 2)
                {
                    dashImage.SetActive(false);
                    shieldImage.SetActive(false);
                    teleImage.SetActive(true);
                }
            }
		}
		else {
			controllerAvail = false;
			bgImage.color = noControllerColor;
		}

        /*
		if (!lockedIn){ //selection thingy
			if (bgImage.color != notLockedColor)
				bgImage.color = notLockedColor;

			if (upHit){
				viewingMod += 1;
			}
			if (submitHit){
				if (viewingMod == 0)
					chosenSubmodifier = dashReference;
				else if (viewingMod == 1)
					chosenSubmodifier = shieldReference;
				else if (viewingMod == 2)
					chosenSubmodifier = teleReference;

				lockedIn = true;
			}
			if (viewingMod == 0){
				dashImage.SetActive(true);
				shieldImage.SetActive(false);
				teleImage.SetActive(false);
			}
			else if (viewingMod == 1){
				dashImage.SetActive(false);
				shieldImage.SetActive(true);
				teleImage.SetActive(false);
			}
			else if (viewingMod == 2){
				dashImage.SetActive(false);
				shieldImage.SetActive(false);
				teleImage.SetActive(true);
			}

		}
		else {
			if (bgImage.color != lockedColor)
				bgImage.color = lockedColor;

			if (myInput.circlePressed){
				lockedIn = false;
			}
		}
        */
	}
}
