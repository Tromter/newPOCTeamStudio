﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Global variables
    public static GameManager Instance;
    public bool gameRunning;
	public GameMode currentGameMode;

    #region Player Related Variables

    public List<PlayerMovement> players = new List<PlayerMovement>();
    public int playerCount;
    public PlayerMovement playerPrefab;
    public Transform[] playerSpawns;
    public Color[] playerColors;
    public Transform[] weaponSpawns;
    public ShotModifier[] shotMods;
    public Sprite[] shotSprites;
    public WeaponBax weaponBoxPrefab;

    float weapSpawnRechargeStart;
    float weapRechargeDuration = 5f;

    #endregion

    public Transform nextWeaponSpawn;
    public Color weaponSpawnCharging;
    public Color weaponSpawnReady;

    #region UI Stuff

    public GameObject scoreBoard;
    public GameObject playerScoreCard;

    #endregion

    #endregion
	bool setupDone = false;
    #region Start and Update
    // Use this for initialization
    void Awake () {
        Instance = this;
        if (gameRunning)
        {
			currentGameMode.Initialize(this);
            
        }
	}

	public void DoSetup(){
        scoreBoard.SetActive(true);
		for(int i = 0; i < playerCount; i++)
		{
			if(i >= playerSpawns.Length) { break; }
			PlayerMovement newPlayer = Instantiate(playerPrefab, playerSpawns[i].position, Quaternion.identity);
			players.Add(newPlayer);
			newPlayer.playerNumber = players.Count;
			ParticleOverlord.instance.SpawnParticle(playerSpawns[i].position, "LevelUpParticle");
			/*
                newPlayer.inputControllerHorizontal = "Horizontal_P" + newPlayer.playerNumber;
                newPlayer.inputControllerVertical = "Vertical_P" + newPlayer.playerNumber;
                newPlayer.inputControllerHorizontalLook = "HorizontalLook_P" + newPlayer.playerNumber;
                newPlayer.inputControllerVerticalLook = "VerticalLook_P" + newPlayer.playerNumber;
                newPlayer.inputControllerFire = "Shoot_P" + newPlayer.playerNumber;
                */
			newPlayer.transform.name = "Player " + newPlayer.playerNumber;
			PlayerInput newPlayerInput = newPlayer.gameObject.GetComponent<PlayerInput>();
			newPlayerInput.playerNum = newPlayer.playerNumber - 1;
			// newPlayer.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.3f, 0.9f), Random.Range(0.3f, 0.9f), Random.Range(0.3f, 0.9f));
			newPlayer.GetComponent<SpriteRenderer>().color = playerColors[i];
			                GameObject newScoreCard = Instantiate(playerScoreCard);
			                newScoreCard.transform.SetParent(scoreBoard.transform, false);
			                newPlayer.myScore = newScoreCard.GetComponent<Text>();
							newPlayer.myScore.text = "P" + newPlayer.playerNumber + " Score: 0";
		}
		weapSpawnRechargeStart = Time.time;
        for(int i = 0; i < weaponSpawns.Length; i++)
        {
            SpriteRenderer weapsr = weaponSpawns[i].GetComponent<SpriteRenderer>();
            weapsr.color = Color.clear;
        }
		if (GameObject.Find("DataTransfer") != null){
			FromMenuData sumData = GameObject.Find("DataTransfer").GetComponent<FromMenuData>();
			for (int i = 0; i < 4; i++) {
				players[i].sub = sumData.playerSubs[i];
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!setupDone && currentGameMode.gameState > 0){
			DoSetup();
			setupDone = true;
		}
		currentGameMode.RunGameMode();
        if (Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
        if(gameRunning) {
            if(Time.time - weapSpawnRechargeStart >= weapRechargeDuration) {
                spawnNewWeapon();
            }
            else if(nextWeaponSpawn != null) {
                SpriteRenderer sr = nextWeaponSpawn.GetComponent<SpriteRenderer>();
                sr.color = Color.Lerp(Color.clear, weaponSpawnCharging, (Time.time - weapSpawnRechargeStart) / weapRechargeDuration);
            }
        }
	}

    void spawnNewWeapon()
    {
        if(nextWeaponSpawn != null) { // if there is a next weapon spawn queued
            SpriteRenderer sr = nextWeaponSpawn.GetComponent<SpriteRenderer>();
            sr.color = weaponSpawnReady;
            WeaponBax newWeapon = Instantiate(weaponBoxPrefab, nextWeaponSpawn.position, Quaternion.identity);
            newWeapon.mySpawn = nextWeaponSpawn.GetComponent<UpgradeObject>();
            nextWeaponSpawn.GetComponent<UpgradeObject>().heldBox = newWeapon;
            int rand = Random.Range(0, shotMods.Length);
            newWeapon.weaponHeld = shotMods[rand];
            newWeapon.GetComponent<SpriteRenderer>().sprite = shotSprites[rand];
        }

        // get the next weapon spawn
        List<Transform> potentialPos = new List<Transform>();
        for(int i = 0; i < weaponSpawns.Length; i++) {
            UpgradeObject upob = weaponSpawns[i].GetComponent<UpgradeObject>();
            if(upob.heldBox == null) { // if this one is available, add it to the list
                potentialPos.Add(weaponSpawns[i]);
            }
        }
        nextWeaponSpawn = null; // in case nothing is available
        if (potentialPos.Count > 0) { // grab a random one
            shuffle(potentialPos);
            int randSelect = Random.Range(0, potentialPos.Count);
            nextWeaponSpawn = potentialPos[randSelect];
        }
    }

    void shuffle<T>(List<T> arr)
    {
        for(int i = 0; i < arr.Count; i++)
        {
            T temp = arr[i];
            int rand = Random.Range(0, arr.Count);
            arr[i] = arr[rand];
            arr[rand] = temp;
        }
    }

    public Vector3 getNewSpawnLoc(PlayerMovement playerRespawning)
    {
        Transform farthestSpawn = playerSpawns[0]; // farthest spawn point
        float farthestSpawnDist = 0f; // average distance of farthest spawn point
        for(int i = 0; i < playerSpawns.Length; i++)
        {
            float sum = 0f;
            for(int j = 0; j < players.Count; j++) // get average distance from each player
            {
                if(players[j] != playerRespawning) // if the player in question is NOT the dead player
                {
                    sum += Vector2.Distance(players[j].transform.position, playerSpawns[i].position);
                }
            }
            float average = sum / players.Count - 1;
            if(average > farthestSpawnDist) {
                farthestSpawn = playerSpawns[i];
                farthestSpawnDist = average;
            }
        }
        return farthestSpawn.position;
    }
    #endregion

	public void YellScoreToMode(int pNum, PlayerMovement killedP, bool dead){
        if (dead) { currentGameMode.AddScoreKill(pNum, killedP);  }
        else { currentGameMode.AddScoreDamage(pNum, killedP); }
	}
}
