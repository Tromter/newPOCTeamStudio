using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour {

    public static EndScreenManager instance;
    public List<playerVote> players = new List<playerVote>();
    public Transform stats;

    public Text thingy;
    public Canvas canvas;
    bool reloading = false;

    public class playerVote
    {
        public PlayerInput playerInput;
        public int playerNumber;
        public int voteType = 0;
        // 0 = not voted yet
        // 1 = rematch
        // 2 = return to match settings
        // 3 = return to main menu
    }

	// Use this for initialization
	void Awake () {
        instance = this;
        stats = WinManager.instance.WinText.transform.parent.Find("Stats");
	}
	
	// Update is called once per frame
	void Update () {
        if(players.Count == 0 || reloading) { return; }
        bool readyUp = true;
        for(int i = 0; i < players.Count; i++)
        {
            if (players[i].playerInput.weaponSwapButtonPressed)
            {
                Debug.Log("Player " + players[i].playerNumber + " wants more bullshit!");
                Transform statPage = stats.Find("Player " + players[i].playerNumber);
                statPage.Find("Ready").gameObject.SetActive(true);
                players[i].voteType = 1;
            }
            // if (players[i].voteType == 2) { /* return to main menu*/ }
            if(players[i].voteType != 1) { readyUp = false; }
        }
        if (readyUp) { StartCoroutine(beginAgain());/*SceneManager.LoadScene(SceneManager.GetActiveScene().name);*/ }
	}
    
    IEnumerator beginAgain()
    {
        reloading = true;
        Text newThingy = Instantiate(thingy, canvas.transform);
        for(int i = 0; i < 3; i++)
        {
            newThingy.text = (3 - i).ToString();
            yield return new WaitForSeconds(1f);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
