using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : ScriptableObject {
	[HideInInspector]
	public GameManager myGM;

	public int gameState;
	[SerializeField] protected List<PlayerMovement> m_players;
	[SerializeField] public float[] m_playerScores;
	public float m_scoreToWin;
	public int m_gamewinner = 0;

	[HideInInspector]
    public GameObject winField;
	public GameObject myIntroSeq;

	public virtual void Initialize(GameManager gm){
		myGM = gm;
		gameState = 0;
		m_players = myGM.players;
		m_playerScores = new float[4];
		Instantiate(myIntroSeq);
	}

	public virtual void RunGameMode(){
		if (gameState == 0){
			StartPhase();
		}
		if (gameState == 1){
			MainPhase();
		}
		if (gameState == 2){
			Endphase();
		}
	}

	public virtual void StartPhase(){
		for (int i = 0; i < m_playerScores.Length; i++) {
			m_playerScores[i] = 0f;
		}
		if (myIntroSeq == null)
      	  gameState = 1;
		//otherwise wait for it to tell you
	}

	public virtual void MainPhase(){
		for (int i = 0; i < m_playerScores.Length; i++) {
			if (m_playerScores[i] >= m_scoreToWin){
				m_gamewinner = i + 1;
				gameState = 2;
			}
		}
	}

	public virtual void Endphase(){
        //do something
        winField = GameObject.Find("WinBGImage");
        if (winField && winField.activeInHierarchy) {
            return;
        }
		WinManager.instance.DisplayWinmessage(m_gamewinner);
	}

    void OnApplicationQuit() {
        winField = null;
    }

	public virtual void AddScoreDamage(int playerNum, PlayerMovement killedPlayer){ //actual number, not the index in the list
		
	}

    public virtual void AddScoreKill(int playerNum, PlayerMovement killedPlayer) { // actual number, not the index in the list

    }
}
