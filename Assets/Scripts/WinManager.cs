using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WinManager : MonoBehaviour {

	public static WinManager instance;

	public Transform parentCanvas;
	public Slider winSliderPrefab;
	public Image slidersBG;


	public Text WinText;
	PlayerMovement[] Players;
	Slider[] winSliders;

	private GameMode myMode;
	// Use this for initialization
	void Start () {
		instance = this;
        myMode = GameManager.Instance.currentGameMode;
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
		Players = new PlayerMovement[playerObjects.Length];
		winSliders = new Slider[Players.Length];
		for (int i = 0; i < Players.Length; i++) {
			Players[i] = playerObjects[i].GetComponent<PlayerMovement>();
			winSliders[i] = Instantiate (winSliderPrefab, parentCanvas);
			winSliders[i].image.color = Players[i].GetComponent<SpriteRenderer>().color;
			///winSliders[i].colors.normalColor = Players[i].GetComponent<SpriteRenderer>().color;
			winSliders[i].minValue = 0;
			winSliders[i].maxValue = myMode.m_scoreToWin;
            //winSliders[i].transform.Find("Background").GetComponent<Image>().color = winSliders[i].image.color;
        }
    }
	
	// Update is called once per frame
	void Update () {
		float bestPlayerScore = 0;
		for (int i = 0; i < Players.Length; i++) {
			if (myMode.m_playerScores[i] > bestPlayerScore){
				// slidersBG.color = winSliders[i].image.color;
				bestPlayerScore = myMode.m_playerScores[i];
				}
			winSliders[i].value = myMode.m_playerScores[i];
            if (winSliders[i].image.sprite != Players[i].GetComponent<SpriteRenderer>().sprite)
                winSliders[i].image.sprite = Players[i].GetComponent<SpriteRenderer>().sprite;
        }
	}

	public void DisplayWinmessage(int playerInt){
		//Debug.Log("Player " + playerInt + " has won via " + method);
		WinText.transform.parent.gameObject.SetActive(true);
        Transform statsSection = WinText.transform.parent.Find("Stats");
        Text[] stats = statsSection.GetComponentsInChildren<Text>();
        for(int i = 0; i < stats.Length; i++)
        {
            PlayerMovement playerData = GameManager.Instance.players[i];
            stats[i].text = "Total Kills: " + playerData.killCount;
            stats[i].text += "\nTotal Deaths: " + playerData.totalDeaths;
            stats[i].text += "\nLongest Life: " + Mathf.Round(playerData.longestLifeSpan) + " seconds";
            string favWeapon = playerData.favWeapons.Keys.Max();
            float timeHeld = playerData.favWeapons[favWeapon];
            stats[i].text += "\nFavorite Weapon: " + favWeapon + "(" + Mathf.Round(timeHeld) + "s)";
            stats[i].color = playerData.GetComponent<SpriteRenderer>().color;

        }
        /*
		if (playerInt == 1)
			WinText.text = "Congratulations RED! You're the best at killing! Press R to restart and let your lesser pals have another shot.";
		else if (playerInt == 2)
			WinText.text = "Congratulations BLUE! You're the best at killing! Press R to restart and let your lesser pals have another shot.";
		else if (playerInt == 3)
			WinText.text = "Congratulations GREEN! You're the best at killing! Press R to restart and let your lesser pals have another shot.";
		else if (playerInt == 4)
			WinText.text = "Congratulations YELLOW! You're the best at killing! Press R to restart and let your lesser pals have another shot.";
		*/
        WinText.text = "Player " + playerInt + " wins!";
        WinText.color = GameManager.Instance.playerColors[playerInt - 1];
	}
}
