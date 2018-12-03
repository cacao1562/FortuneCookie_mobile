using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoveFork : MonoBehaviour {

	private Vector3 v3;
	private RectTransform rt;
	void Start () {
		
		v3 = transform.localPosition;
		rt = GetComponent<RectTransform>();

	}
	
	// Update is called once per frame
	void Update () {
		
		if( rt.localPosition.y > 2000 ){

			transform.localPosition = v3;
			gameObject.SetActive(false);
		}
		transform.Translate(Vector3.up * Time.deltaTime * 20 );
	}

	
}
