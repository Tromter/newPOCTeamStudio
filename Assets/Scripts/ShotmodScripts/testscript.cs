using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour {

	public Sprite[] my_sprites;
	public GameObject background;
	public bool end = false;
	private int my_X = 0;

	// Use this for initialization
	void Start () {
		end = true;
	}
	
	// Update is called once per frame
	void Update () {
		background.GetComponent<SpriteRenderer> ().sprite = my_sprites[background_switch ()];
	}


	int background_switch(){
		int curr_sprite = my_X % 80;
		my_X++;
		example ();
		return curr_sprite;
	}

	IEnumerator example(){
		yield return new WaitForSeconds(0.03086419753f);
	}
}
