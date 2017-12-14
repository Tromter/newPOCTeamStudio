using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeObject : MonoBehaviour {

	public GameObject heldBox;

	public float chargeTime;
	private float currentCharge;

	public Color startChargingColor;
	public Color endChargingColor;
	public Color fullyChargedColor;

	private SpriteRenderer mySR;
	// Use this for initialization
	void Start () {
		mySR = this.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (heldBox != null) {
			if (currentCharge < chargeTime) {

				currentCharge += Time.deltaTime;
				mySR.color = Color.Lerp (startChargingColor, endChargingColor, currentCharge / chargeTime);
				if (heldBox.activeSelf)
					heldBox.SetActive (false);
			} else {
				if (!heldBox.activeSelf)
					heldBox.SetActive (true);
				if (mySR.color != fullyChargedColor)
					mySR.color = fullyChargedColor;
			}
		}
	}
}
