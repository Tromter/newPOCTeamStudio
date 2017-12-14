using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDragonStuff : MonoBehaviour {

    public static BloodDragonStuff Instance;
    public Transform bloodDragonPrefab;

    public float interval;
    public bool running = false;

	// Use this for initialization
	void Start () {
        Instance = this;
        StartCoroutine(bloodDragonSpawning());
    }
	
	// Update is called once per frame
	void Update () {

	}

    IEnumerator bloodDragonSpawning()
    {
        while (true)
        {
            Instantiate(bloodDragonPrefab, transform.position, Quaternion.Euler(90, 0, 0));
            yield return new WaitForSeconds(interval);
        }
    }
}
