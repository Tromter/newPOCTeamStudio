using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FromMenuData : MonoBehaviour {

	public SubModifier[] playerSubs;


	public void SetData (SubModifier[] inputSubArray){
		playerSubs = inputSubArray;
	}

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
	}
}
