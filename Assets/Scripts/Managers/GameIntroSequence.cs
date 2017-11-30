using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;
using UnityEngine.PostProcessing;

public class GameIntroSequence : MonoBehaviour {

	public IntroSlide[] slides;
	public float slideTransitionLerpSpd;
	public float introBloom;
	public float introJitter;
	public float introTear;
	public float introColorShake;
	public float gameStartDelay;

	private int currentSlide = 0;
	private float originalBloom;
	private float originalJitter;
	private float originalTear;
	private float originalColorShake;

	private float internalTimer;
	private AnalogGlitch glitchSettings;
	// Use this for initialization
	void Start () {
		//set original bloom and jitter
		originalBloom = CamControl.instance.baseBloom;
		glitchSettings = Camera.main.GetComponent<AnalogGlitch>();

		var bloomSettings = Camera.main.GetComponent<PostProcessingBehaviour>().profile.bloom.settings;
		originalBloom = bloomSettings.bloom.intensity;
		bloomSettings.bloom.intensity = introBloom;
		Camera.main.GetComponent<PostProcessingBehaviour>().profile.bloom.settings = bloomSettings;

		originalJitter = glitchSettings.scanLineJitter;
		originalTear = glitchSettings.verticalJump;
		originalColorShake = glitchSettings.colorDrift;

		glitchSettings.scanLineJitter = introJitter;
		glitchSettings.verticalJump = introTear;
		glitchSettings.colorDrift = introColorShake;
	}
	
	// Update is called once per frame
	void Update () {

		if (currentSlide < slides.Length){
			Vector2 slideVector = Vector2.left * slides[currentSlide].slideTransform.anchoredPosition.x * slideTransitionLerpSpd;
			for (int i = 0; i < slides.Length; i++) {
				slides[i].slideTransform.anchoredPosition += slideVector * Time.deltaTime;
			}
			if (internalTimer >= slides[currentSlide].slideStayDuration){
				internalTimer = 0f;
				currentSlide += 1;

			}
		}
		else if (currentSlide == slides.Length){ //beyond by 1
			for (int i = 0; i < slides.Length; i++) {
				slides[i].slideTransform.gameObject.SetActive(false);
			}
			currentSlide += 1;
		}
		if (currentSlide > slides.Length){
			//downbloom and jitter
			if (internalTimer > gameStartDelay) internalTimer = gameStartDelay;

			float bloomToSet = Mathf.Lerp(introBloom, originalBloom, internalTimer / gameStartDelay);
			float jitterToSet = Mathf.Lerp(introJitter, originalJitter, internalTimer / gameStartDelay);
			float tearToSet = Mathf.Lerp(introTear, originalTear, internalTimer / gameStartDelay);
			float colorToSet = Mathf.Lerp(introColorShake, originalColorShake, internalTimer / gameStartDelay);
			var bloomSettings = Camera.main.GetComponent<PostProcessingBehaviour>().profile.bloom.settings;
			bloomSettings.bloom.intensity = bloomToSet;
			Camera.main.GetComponent<PostProcessingBehaviour>().profile.bloom.settings = bloomSettings;
			glitchSettings.scanLineJitter = jitterToSet;
			glitchSettings.verticalJump = tearToSet;
			glitchSettings.colorDrift = colorToSet;

			if (internalTimer == gameStartDelay){
				//launch that fucker
				GameManager.Instance.currentGameMode.gameState = 1; //make it main game time
				Destroy(this.gameObject);
			}
		}

		internalTimer += Time.deltaTime;
	}
}

[System.Serializable]
public class IntroSlide {
	public RectTransform slideTransform;
	public float slideStayDuration;
}
