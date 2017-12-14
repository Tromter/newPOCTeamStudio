using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeObject : MonoBehaviour {

	public WeaponBax heldBox;

	public float chargeTime;
	private float currentCharge;

	public Color startChargingColor;
	public Color endChargingColor;
	public Color fullyChargedColor;

	private SpriteRenderer mySR;
	// Use this for initialization
	void Start () {
		// mySR = this.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
