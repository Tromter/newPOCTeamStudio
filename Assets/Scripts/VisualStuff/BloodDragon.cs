using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDragon : MonoBehaviour {

    public static float speed = 4.5f;
    public static float lifeTime = 30f;

    float startTime;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.forward * speed * Time.deltaTime;
        if(Time.time - startTime > lifeTime) { Destroy(gameObject); }
	}
}
