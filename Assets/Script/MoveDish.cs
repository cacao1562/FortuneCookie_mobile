using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveDish : MonoBehaviour {

	public Text x, y;
	private int moveRotation = 0;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(moveRotation == 0) {

			transform.Rotate(new Vector3(0f,0f,-2f) );

		}//else if(moveRotation == 1) {

		// 	transform.Rotate(new Vector3(0f,0f,1f) );

		// }
		
	}
	
}
