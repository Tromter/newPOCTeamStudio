using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "GameModes/PowerCore")]
public class PowerCoreGameMode : GameMode {

	public GameObject powerCorePrefab;
	public Vector2 powerCoreInitialPos;

	public float normalKillPoints;
	public float coreHolderKillPoints;
    public int killModifier;

	public float coreUpgradeRate;

	public bool spawnCore = true;
	public override void Initialize (GameManager gm)
	{
		base.Initialize (gm);
		spawnCore = true;
	}

	public override void StartPhase ()
	{
		base.StartPhase ();
		if (spawnCore){
			GameObject newCore = Instantiate(powerCorePrefab, (Vector3)powerCoreInitialPos, Quaternion.identity);
			spawnCore = false;
		}

		//this is where I would set core upgrade rate...IF I HAD ONE
	}

	public override void AddScoreDamage (int playerNum, PlayerMovement killedPlayer)
	{
		if (killedPlayer.upgradeObject != null){
			m_playerScores[playerNum - 1] += coreHolderKillPoints;
		}
		else {
			m_playerScores[playerNum - 1] += normalKillPoints;
        }
        m_players[playerNum - 1].myScore.text = "P" + playerNum + " Score: " + m_playerScores[playerNum - 1];
    }

    public override void AddScoreKill(int playerNum, PlayerMovement killedPlayer)
    {
        if (killedPlayer.upgradeObject != null)
        {
            m_playerScores[playerNum - 1] += coreHolderKillPoints * killModifier;
            m_players[playerNum - 1].myCanvasManager.PopupMessage("+" + coreHolderKillPoints * killModifier, .5f, .25f, 1f, 1.2f);
			HoldToWinItem ballRef = FindObjectOfType<HoldToWinItem>();
			ballRef.currentHolderTransform = m_players[playerNum - 1].transform;
			m_players[playerNum - 1].upgradeObject = ballRef.transform;
        }
        else {
            m_playerScores[playerNum - 1] += normalKillPoints * killModifier;
            m_players[playerNum - 1].myCanvasManager.PopupMessage("+" + normalKillPoints * killModifier, .5f, .25f, 1f, 1.2f);
        }
        m_players[playerNum - 1].myScore.text = "P" + playerNum + " Score: " + m_playerScores[playerNum - 1];
    }
}
