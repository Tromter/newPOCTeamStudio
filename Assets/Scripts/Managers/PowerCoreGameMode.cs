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

	public override void StartPhase ()
	{
		base.StartPhase ();
		GameObject newCore = Instantiate(powerCorePrefab, (Vector3)powerCoreInitialPos, Quaternion.identity);
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
	}

    public override void AddScoreKill(int playerNum, PlayerMovement killedPlayer)
    {
        if (killedPlayer.upgradeObject != null)
        {
            m_playerScores[playerNum - 1] += coreHolderKillPoints * killModifier;
        }
        else {
            m_playerScores[playerNum - 1] += normalKillPoints * killModifier;
        }
    }
}
