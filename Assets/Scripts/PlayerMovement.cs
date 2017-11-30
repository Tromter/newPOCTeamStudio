using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    public float speed;
    public float normalSpeed;
    /*
    public string inputControllerHorizontal;
    public string inputControllerVertical;
    public string inputControllerHorizontalLook;
    public string inputControllerVerticalLook;
    public string inputControllerFire;
    */
    public int playerNumber;
    public int max_health = 1;
    public int health;

    [SerializeField] private PlayerInput myInput;

	public bool shootHeld = false;
    public bool strafeControls = false;

    public float timeAlive;
    public float currentLife;
    public float longestLifeSpan;
    public int killCount;

    public Dictionary<string, float> favWeapons = new Dictionary<string, float>();

    public int currentLifeKillCount;
    public int totalDeaths;
    public float respawnTime;
    public float iframes;
	public Text myScore;

    public Rigidbody2D rbody;
    public Vector3 moveDir;
    public Vector3 lookDir;

    bool dead;
    public Transform explosionPrefab;

	private SpaceGun myPlayerGun;
	[SerializeField] public ShotModifier weap;
    public float heldWeaponTime;
	public SubModifier sub;
    public float weapExp;

    public Transform upgradeObject;

    int powerMod = 1;
    public float upgradeFactor;

    public float dashTime;
    public float subActionRecoverTime;
    public bool canSubAction;
    public bool subActioning;

    Coroutine iframesRoutine;
	Coroutine injuryRoutine;

	public PlayerCanvasManager myCanvasManager;

	private SpriteRenderer mySR;
    [SerializeField] SpriteRenderer myTip;

	// Use this for initialization
	void Start () {
        health = 1;
        heldWeaponTime = 0f;
        killCount = 0;
        longestLifeSpan = 0;
        currentLife = 0;

		myCanvasManager = this.GetComponentInChildren<PlayerCanvasManager>();
        myInput = this.GetComponent<PlayerInput>();
		myPlayerGun = this.GetComponent<SpaceGun>();
		mySR = this.GetComponent<SpriteRenderer>();
        rbody = GetComponent<Rigidbody2D>();
        dead = false;
        canSubAction = true;
        subActioning = false;
        timeAlive = 0;
        weapExp = 0;
        myPlayerGun.currentShotMod = weap;
        favWeapons.Add(myPlayerGun.currentShotMod.modName, 0f);
        //        myScore.color = mySR.color;
    }
	
	// Update is called once per frame
	void Update () {
		// float shooting = Input.GetAxisRaw(inputControllerFire);
        if (!dead) {
            if(upgradeObject != null) {
                timeAlive += Time.deltaTime * (currentLifeKillCount + 1);
                weapExp += Time.deltaTime * upgradeFactor;
            }

            processMovement();
            processShooting();
        }
        int currLevel = 0;
        currentLife += Time.deltaTime;
        if(currentLife > longestLifeSpan) { longestLifeSpan = currentLife; }
        if (favWeapons.ContainsKey(myPlayerGun.currentShotMod.modName)) {
            favWeapons[myPlayerGun.currentShotMod.modName] += Time.deltaTime;
        }
        else {
            favWeapons.Add(myPlayerGun.currentShotMod.modName, 0f);
        }
	}

    void processShooting()
    {
        if (myInput.shootButtonHeld)
        {
            /*
            if (equip) { powerMod = Mathf.RoundToInt(weapExp1 / myPlayerGun.currentShotMod.timeToLevelRatio); }
            else { powerMod = Mathf.RoundToInt(weapExp2 / myPlayerGun.currentShotMod.timeToLevelRatio); }
            if (powerMod < 1) { powerMod = 1; }
            speed = normalSpeed / powerMod;
            if(speed < 1f) { speed = 1f; }
            */
            myPlayerGun.currentShotMod.ModifyAndShoot(weapExp, myPlayerGun, mySR.color);
        }
        
        /*
        else
        {
            speed = normalSpeed;
            powerMod = 1;
        }
        */
    }

    void processMovement()
    {
        rbody.velocity = Vector2.zero;
        float horizontal = myInput.leftStickInput.x;
        float vertical = myInput.leftStickInput.y;
        float horizontalLook = myInput.rightStickInput.x;
        float verticalLook = myInput.rightStickInput.y;

        Vector3 tempMoveDir = new Vector3(horizontal, vertical, 0);
        Vector3 tempLookDir = new Vector3(horizontalLook, verticalLook, 0f);

        float currSpeed = speed;

        if(!subActioning)
        {
            if (horizontal != 0 || vertical != 0) { lookDir = tempMoveDir; }
            else if(horizontalLook != 0 || verticalLook != 0) { lookDir = tempLookDir; }
            if (tempLookDir.magnitude > 0)
            {
                // if (moveDir == Vector3.zero) { moveDir = tempMoveDir; }if(tempLookDir.x != 0 || tempLookDir.y != 0) 
                { transform.rotation = Quaternion.LookRotation(Vector3.forward, tempLookDir); }
                rbody.MovePosition(transform.position + tempMoveDir * speed * Time.deltaTime);
            }
            else
            {
                moveDir = tempMoveDir;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, lookDir);
                rbody.MovePosition(transform.position + tempMoveDir * speed * Time.deltaTime);
            }
            if (myInput.dashButtonPressed && canSubAction) { StartCoroutine(subAction()); }
			else if (myInput.dashButtonPressed){
				myCanvasManager.PopupMessage("Sub Offline", .45f, .1f, 2f, 1f);
			}
        }
    }

    void processMovementDualStick()
    {
        /*
        rbody.velocity = Vector2.zero;
        float horizontal = Input.GetAxisRaw(inputControllerHorizontal);
        float vertical = Input.GetAxisRaw(inputControllerVertical);
        float horizontalLook = Input.GetAxisRaw(inputControllerHorizontalLook);
        float verticalLook = Input.GetAxisRaw(inputControllerVerticalLook);
        shootHeld = horizontalLook != 0 || verticalLook != 0;
        Vector3 tempFlyDir = new Vector3(horizontal, vertical, 0);
        Vector3 tempLookDir = new Vector3(horizontalLook, verticalLook, 0f);
        if (shootHeld)
        {
            if(tempLookDir.x != 0 || tempLookDir.y != 0) { transform.rotation = Quaternion.LookRotation(Vector3.forward, tempLookDir); }
        }
        else
        {
            if(tempFlyDir.x != 0 || tempFlyDir.y != 0) { transform.rotation = Quaternion.LookRotation(Vector3.forward, tempFlyDir); }
        }
        
        rbody.MovePosition(transform.position + tempFlyDir * speed * Time.deltaTime);
        */
    }

    public void TakeDamage(int killerID)
    {
        if(iframesRoutine != null) { return; }
        health--;
        if (killerID > 0 && killerID <= GameManager.Instance.players.Count) {
            PlayerMovement myKiller = GameManager.Instance.players[killerID - 1];
            if(health <= 0) { Die(killerID); return; }
            else { GameManager.Instance.YellScoreToMode(killerID, this, false); }
        }
        if (injuryRoutine != null) { StopCoroutine(injuryRoutine); }
		injuryRoutine = StartCoroutine(InjuryFlash(.2f));
        // if(health <= 0) { Die(killerID); }
    }

    public void HPForKill()
    {
        health++;
        if(health > max_health) { health = max_health; }
    }

    public void Die(int killerID)
    {
        if(iframesRoutine == null)
        {
            if (killerID > 0 && killerID <= GameManager.Instance.players.Count)
            {
                PlayerMovement myKiller = GameManager.Instance.players[killerID - 1];
                myKiller.AddScore();
                transferUpgradeObject(myKiller);
                // myKiller.HPForKill();
                GameManager.Instance.YellScoreToMode(killerID, this, true);

            }
            int weapLevel = weap.GetLevel(weapExp);
            totalDeaths++;
            
            currentLife = 0f;

			CamControl.instance.AddShake((float)weapLevel);

            float kd = (float)killCount / (float)totalDeaths;
            if(kd > 1) { kd = 1; }
            int levelsToSubtract = Mathf.CeilToInt(weapLevel * kd);
            int weapNewLevel = weapLevel - levelsToSubtract;
			max_health = weapNewLevel;
			health = weapNewLevel;

			if (max_health < 1) max_health = 1;
			if (health < 1) health = 1;

            weapExp = weap.timeToLevelRatio * weapNewLevel;
            if(weapExp < 0) { weapExp = 0; }
            
            currentLifeKillCount = 0;
            timeAlive = 0;
            // dropUpgradeObject();
            StartCoroutine(respawn());
        }
    }

    void transferUpgradeObject(PlayerMovement myKiller)
    {
        if(upgradeObject != null)
        {
            HoldToWinItem upgrader = GetComponent<HoldToWinItem>();
            upgrader.currentHolderTransform = null;
            myKiller.pickUpUpgrader(upgrader);
            upgradeObject = null;
        }
    }

    IEnumerator respawn()
    {
        dead = true;
        Transform newExp = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(newExp.gameObject, 1f);
        // Deactivate collider(s) and spriterenderer(s)
        GetComponent<Collider2D>().enabled = false;
        mySR.enabled = false;
        myTip.enabled = false;
		Vector3 newSpawnVector  = GameManager.Instance.getNewSpawnLoc(this);
		ParticleOverlord.instance.SpawnParticle(newSpawnVector, "PlayerSpawnParticle", mySR.color);

        yield return new WaitForSeconds(respawnTime);
        // reactivate collider(s) and spriterenderer(s)
        dead = false;
		transform.position = newSpawnVector;
        GetComponent<Collider2D>().enabled = true;
        mySR.enabled = true;
        myTip.enabled = true;
        health = max_health;
        iframesRoutine = StartCoroutine(freshSpawn());
    }

    public IEnumerator freshSpawn()
    {
        float startTime = Time.time;
        int frameCount = 0;
        while(Time.time - startTime < iframes)
        {
            if (myInput.shootButtonHeld)
            {
                mySR.enabled = true;
                myTip.enabled = true;
                iframesRoutine = null;
                yield break;
            }
            if (frameCount%3 == 0) {
                mySR.enabled = !mySR.enabled;
                myTip.enabled = !myTip.enabled;
            }
            frameCount++;
            yield return new WaitForEndOfFrame();
        }
        mySR.enabled = true;
        myTip.enabled = true;
        iframesRoutine = null;
    }

	public IEnumerator InjuryFlash(float dur){
		float startTime = Time.time;
		int frameCount = 0;
		while(Time.time - startTime < dur)
		{
			if (health <= 0){
				mySR.enabled = false;
				injuryRoutine = null;
				yield break;
			}
			
			if (frameCount%3 == 0) {
				mySR.enabled = !mySR.enabled;
				myTip.enabled = !myTip.enabled;
			}
			frameCount++;
			yield return new WaitForEndOfFrame();
		}
		mySR.enabled = true;
		myTip.enabled = true;
		injuryRoutine = null;
	}

    IEnumerator subAction()
    {
        float startTime = Time.time;
		Debug.Log ("Oh sheet");
        subActioning = true;
        canSubAction = false;
		subActionRecoverTime = sub.cooldown;
        dropUpgradeObject();
		while(Time.time - startTime < sub.value)
        {
			sub.runSubAction (this);
			yield return new WaitForEndOfFrame();
        }
        subActioning = false;
        yield return new WaitForSeconds(subActionRecoverTime);
        canSubAction = true;
    }

    public void AddScore()
    {
        killCount++;
        currentLifeKillCount++;
		// myScore.text = "Lv." + myPlayerGun.currentShotMod.currentLevel + " Score: " + killCount.ToString();
    }

    public void pickUpWeapon(ShotModifier newShotMod)
    { // Replace currently held weapon and reset experience points
        if(weap == newShotMod) { return; }
        weap = newShotMod;
        // weapExp = 0f;
        myPlayerGun.currentShotMod = newShotMod;
		myCanvasManager.PopupMessage(newShotMod.name, .5f, .25f, 2f, 1f);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.GetComponent<HoldToWinItem>() && upgradeObject == null)
        {
            pickUpUpgrader(coll.GetComponent<HoldToWinItem>());
        }
    }

	void OnCollisionEnter2D(Collision2D other){
		// Debug.Log ("NO!");
//		int levelconsider = other.GetComponent<ShipSpriteManager> ().GetPlayerLvl ();
//		int mylevel = this.GetComponent<ShipSpriteManager> ().GetPlayerLvl ();
		if(sub.name == "SubDashMod" && subActioning){
			if(other.collider.tag == "Player"){
				other.collider.GetComponent<PlayerMovement>().TakeDamage (playerNumber); 
//				if(levelconsider == 1)
//				{
//					other.GetComponent<PlayerMovement> ().Die (playerNumber);
//				}
//				else if(levelconsider == mylevel){
//					other.GetComponent<PlayerMovement>().TakeDamage (playerNumber); 
//				}
//				else if(levelconsider > 1 && mylevel > levelconsider){
//					other.GetComponent<PlayerMovement>().TakeDamage (playerNumber);
//					other.GetComponent<PlayerMovement>().TakeDamage (playerNumber);
//				}
//				else{
//					for(int i = 0; i<mylevel; i++){
//						other.GetComponent<PlayerMovement> ().TakeDamage (playerNumber);
//					}
//					if(other.GetComponent<PlayerMovement> ().dead == false){
//						other.GetComponent<PlayerMovement> ().TakeDamage (playerNumber);
//					}
//					this.Die (other.GetComponent<PlayerMovement>().playerNumber);
//				}

			}
		}
	}

    void pickUpUpgrader(HoldToWinItem upgrader)
    {
        if(upgrader.currentHolderTransform != null) { return; } // if someone's already holding this
        upgradeObject = upgrader.transform;
        upgrader.currentHolderTransform = transform;
        // upgradeObject.parent = transform;
        // upgradeObject.localPosition = Vector3.zero;
        // upgradeObject.GetComponent<Collider2D>().enabled = false;
    }

    void dropUpgradeObject()
    {
        if (upgradeObject != null)
        {
            // upgradeObject.GetComponent<Collider2D>().enabled = true;
            // upgradeObject.parent = null;
            upgradeObject.GetComponent<HoldToWinItem>().currentHolderTransform = null;
            upgradeObject = null;
        }
    }
}
