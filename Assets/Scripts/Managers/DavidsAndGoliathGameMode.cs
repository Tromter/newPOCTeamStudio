using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "GameModes/DavidsAndGoliathGameMode")]
public class DavidsAndGoliathGameMode : GameMode {


	public float davidKillPoints;
	public float goliathKillPoints;
    public float goliathKillDavidPoints;

    public int killModifier;

	public int currentGoliath = 0;

	public float expPerDavidOnDavid;

	public override void StartPhase ()
	{
		base.StartPhase ();
		//this is where I would set core upgrade rate...IF I HAD ONE
		currentGoliath = 0;
	}

	public override void MainPhase ()
	{
		base.MainPhase ();
		if (currentGoliath != 0) {
			m_players [currentGoliath - 1].weapExp = 10000f;
		}
	}

    public override void AddScoreDamage(int playerNum, PlayerMovement killedPlayer)
    {
        if(playerNum != currentGoliath && killedPlayer.playerNumber != currentGoliath) { // david on david
            m_playerScores[playerNum - 1] += davidKillPoints;
            m_players[playerNum - 1].myCanvasManager.PopupMessage("+" + davidKillPoints, .5f, .25f, 1f, 1.2f);
        }
        else if(playerNum == currentGoliath) { // goliath on david
            m_playerScores[playerNum - 1] += goliathKillDavidPoints;
            m_players[playerNum - 1].myCanvasManager.PopupMessage("+" + goliathKillDavidPoints, .5f, .25f, 1f, 1.2f);
        }
        else if(currentGoliath == 0) { // first damage
            m_playerScores[playerNum - 1] += davidKillPoints;
            m_players[playerNum - 1].myCanvasManager.PopupMessage("+" + davidKillPoints, .5f, .25f, 1f, 1.2f);
        }
        else { // david on goliath
            m_playerScores[playerNum - 1] += goliathKillPoints;
            m_players[playerNum - 1].myCanvasManager.PopupMessage("+" + goliathKillPoints, .5f, .25f, 1f, 1.2f);
        }
        m_players[playerNum - 1].myScore.text = "P" + playerNum + " Score: " + m_playerScores[playerNum - 1];
    }

    public override void AddScoreKill (int playerNum, PlayerMovement killedPlayer)
	{
        if (currentGoliath == 0)
        { //first kill
            currentGoliath = playerNum;
            m_players[playerNum - 1].weapExp = 10000f;
            m_playerScores[playerNum - 1] += davidKillPoints * killModifier;
            m_players[playerNum - 1].myCanvasManager.PopupMessage("IT BEGINS", .25f, 1f, 1f, 1f);
        } else if (playerNum != currentGoliath && killedPlayer.playerNumber != currentGoliath) { //david on david
            m_playerScores[playerNum - 1] += davidKillPoints * killModifier;
            //m_players [playerNum - 1].weapExp += expPerDavidOnDavid;
            killedPlayer.weapExp = 0f;
            m_players[playerNum - 1].myCanvasManager.PopupMessage("+" + davidKillPoints * killModifier, .5f, .25f, 1f, 1.2f);
        } else if(playerNum == currentGoliath) { // if goliath killed david
            m_playerScores[playerNum - 1] += goliathKillDavidPoints * killModifier;
            killedPlayer.weapExp = 0f;
            m_players[playerNum - 1].myCanvasManager.PopupMessage("+" + goliathKillDavidPoints * killModifier, .5f, .25f, 1f, 1.2f);
		} 
		else { //david kills goliath
			m_playerScores[playerNum - 1] += goliathKillPoints * killModifier;
			currentGoliath = playerNum;
			m_players [playerNum - 1].weapExp = 10000f;
			killedPlayer.weapExp = 0f;
			m_players[playerNum - 1].myCanvasManager.PopupMessage("GOLIATHIZED", .25f, 1f, 1f, 1f); 
		}
        m_players[playerNum - 1].myScore.text = "P" + playerNum + " Score: " + m_playerScores[playerNum - 1];
	}
}

