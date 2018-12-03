using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveParticle : MonoBehaviour {

	
	private Vector3 v3;
	void Awake(){
		
		v3 = transform.position;
	}
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.Translate(Vector3.up * Time.deltaTime * 20);

		if(transform.position.y > 10) {

			transform.position = v3;
			gameObject.SetActive(false);
		}
	}
}
