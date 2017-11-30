using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIntroSequence : MonoBehaviour {

	public IntroSlide[] slides;

	private float internalTimer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class IntroSlide {
	Transform slideTransform;
	float slideStayDuration;
}
