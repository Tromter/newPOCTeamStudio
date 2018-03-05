using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public static MenuManager instance;


	public Scene[] DanGScenes;
	public Scene[] PB_Scenes;

	public GameObject launchObject;

	public int chosenGameMode = 0; //0 is DanG, 1 is PowerBall
	public int chosenMap = 0; //0  is og

	public RectTransform[] screens;
	public float screenLerpSpeed = 0.17f;

	public PlayerReadyObject[] playerReadys;

	private int currentScreen = 0;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		launchObject.SetActive (false);
		DanGScenes = new Scene[3];
		DanGScenes[0] = SceneManager.GetSceneByName("DangReact");
		DanGScenes[1] = SceneManager.GetSceneByName("DangHell");
		DanGScenes[2] = SceneManager.GetSceneByName("DangCross");

		PB_Scenes = new Scene[3];
		PB_Scenes[0] = SceneManager.GetSceneByName("PBReact");
		PB_Scenes[1] = SceneManager.GetSceneByName("PBHell") 
		PB_Scenes[2] = SceneManager.GetSceneByName("PBCross");
	}
	
	// Update is called once per frame
	void Update () {
		if (screens [currentScreen].anchoredPosition.x != 0) {
			float moveAmount = Mathf.Lerp (screens [currentScreen].anchoredPosition.x, 0, screenLerpSpeed);
			moveAmount -= screens [currentScreen].anchoredPosition.x;
			for (int i = 0; i < screens.Length; i++) {
				screens [i].anchoredPosition += Vector2.right * moveAmount;
			}
		}
		if (ControllerPool.me.numConnected >= 4) {
			if (!launchObject.activeSelf)
				launchObject.SetActive (true);
		} else if (launchObject.activeSelf) {
			launchObject.SetActive (false);
		}
	}

	public void ButtonPressSound(){

	}

	public void LaunchGame(){
		bool canLaunch = true;
		foreach (PlayerReadyObject p in playerReadys) {
			if (!p.lockedIn)
				canLaunch = false;
		}

		if (canLaunch){
			SubModifier[] playerSubArray = new SubModifier[4];
			for (int i = 0; i < 4; i++) {
				playerSubArray [i] = playerReadys [i].chosenSubmodifier;
			}
			GameObject DataObject = new GameObject("DataTransfer");
			FromMenuData newDataTransfer = DataObject.AddComponent<FromMenuData>();
			newDataTransfer.SetData (playerSubArray);
			if (chosenGameMode == 0) {
				
				//launch DanG
				SceneManager.LoadScene(DanGScenes[chosenMap].buildIndex);

			} else if (chosenGameMode == 1) {
				
				//launch Powerball
				SceneManager.LoadScene(PB_Scenes[chosenMap].buildIndex);
			}
		}
	}

	public void SetCurrentScreen(int newScreen){
		currentScreen = newScreen;
	}

	public void SetGameMode(int newGamemode){
		chosenGameMode = newGamemode;
	}

	public void SetMap(int newMap){
		chosenMap = newMap;
	}

	public void QuitGame(){
		Debug.Log ("quitting");
		Application.Quit ();
	}
}
