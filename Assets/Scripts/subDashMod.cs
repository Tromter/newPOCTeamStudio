using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "SubModifiers/subDashMod")]
public class subDashMod : SubModifier {
	
	public float multiplier;
	public int mylevel;
	public int myID;

	public override void runSubAction (PlayerMovement xXx_pla_Move_xXx)
	{
		mylevel = xXx_pla_Move_xXx.GetComponent<ShipSpriteManager> ().GetPlayerLvl ();
		//myID = xXx_pla_Move_xXx.GetComponent<PlayerMovement> ()
		Rigidbody2D rbody = xXx_pla_Move_xXx.rbody;
		Vector2 realLookDir = new Vector2 (xXx_pla_Move_xXx.lookDir.x, xXx_pla_Move_xXx.lookDir.y);
		rbody.MovePosition (rbody.position + realLookDir * xXx_pla_Move_xXx.speed * multiplier * Time.deltaTime);

	}

//	void OnCollisionEnter2D(Collider2D other){
////		int levelconsider = other.GetComponent<ShipSpriteManager> ().GetPlayerLvl ();
////		if(other.tag == "Player"){
////			if(levelconsider == 1 && mylevel > levelconsider)
////			{
////				other.GetComponent<PlayerMovement>().TakeDamage ()
////			}
////		}
//	}
}

