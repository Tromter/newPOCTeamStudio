using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : ScriptableObject {
	public GameManager myGM;

	[SerializeField] protected int m_gameState;
	[SerializeField] protected List<PlayerMovement> m_players;
	[SerializeField] public float[] m_playerScores;
	public float m_scoreToWin;
	public int m_gamewinner = 0;

    public GameObject winField;

	public virtual void Initialize(GameManager gm){
		myGM = gm;
		m_gameState = 0;
		m_players = myGM.players;
		m_playerScores = new float[4];
	}

	public virtual void RunGameMode(){
		if (m_gameState == 0){
			StartPhase();
		}
		if (m_gameState == 1){
			MainPhase();
		}
		if (m_gameState == 2){
			Endphase();
		}
	}

	public virtual void StartPhase(){
		for (int i = 0; i < m_playerScores.Length; i++) {
			m_playerScores[i] = 0f;
		}
        m_gameState = 1;
	}

	public virtual void MainPhase(){
		for (int i = 0; i < m_playerScores.Length; i++) {
			if (m_playerScores[i] >= m_scoreToWin){
				m_gamewinner = i + 1;
				m_gameState = 2;
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
