using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasManager : MonoBehaviour {

	public GameObject popupPrefab;
	public Text healthText;
    public Image healthBar;
    public Image healthBarInner;

	private PlayerMovement myPlayer;
    Vector3 worldScale;

    public float alpha = 0.7f;
	// Use this for initialization
	void Start () {
		myPlayer = transform.parent.GetComponent<PlayerMovement>();
        worldScale = transform.lossyScale;
        healthBar.color = new Color(healthBar.color.r, healthBar.color.g, healthBar.color.b, alpha);
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.eulerAngles = Vector3.zero;
        // healthText.text = "" + myPlayer.health;
        // healthText.enabled = myPlayer.GetComponent<SpriteRenderer>().enabled;
        healthBarInner.fillAmount = (float)myPlayer.health / myPlayer.max_health;
        healthBar.gameObject.SetActive(myPlayer.GetComponent<SpriteRenderer>().enabled);

		if (myPlayer.health == myPlayer.max_health && myPlayer.max_health != 1){
            // healthText.color = Color.green;
            healthBarInner.color = Color.green;
		}
		else if (myPlayer.health == 1 && myPlayer.max_health != 1){
            // healthText.color = Color.red;
            healthBarInner.color = Color.red;
        }
		else {
            // healthText.color = Color.white;
            healthBarInner.color = Color.white;
        }
        healthBarInner.color = new Color(healthBarInner.color.r, healthBarInner.color.g, healthBarInner.color.b, alpha);
    }

    public void updateHPBarSize()
    {
        Vector3 parentScale = transform.lossyScale;
        this.transform.localScale = new Vector3(worldScale.x / parentScale.x, worldScale.y / parentScale.y, worldScale.z / parentScale.z);
    }

	public void PopupMessage(string contents, float travelDur, float lingerDur, float distanceToTravel, float scaleOverTravel){
		GameObject newPopup = Instantiate(popupPrefab, this.transform);
		newPopup.GetComponent<Text>().text = contents;
		PopupScript newPPS = newPopup.GetComponent<PopupScript>();
		newPPS.travelDuration = travelDur;
		newPPS.lingerDuration =  lingerDur;
		newPPS.scaleToReach = scaleOverTravel;
		newPPS.relDistanceToTravel = distanceToTravel;
		newPPS.execute = true;
	}
}
