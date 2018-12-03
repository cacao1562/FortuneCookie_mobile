using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {

	private float animTime = 0.8f;
	private float start = 1f; //fadeout 일때 1~0  a > 0f
							  //fadein 일때 0~1   a < 1f
	private float end = 0f;	
	private float time = 0f;
	Image image;
	void Awake()
	{
		
		image = GetComponent<Image>();

	}

	public void showFadeOut() {
		StartCoroutine( fadeOut() );
	}

	public void showFadeIn() {
		StartCoroutine( fadeIn() );
	}

	public void setAlpa() {
		image.color = new Color(image.color.r,image.color.g,image.color.b,1f);
	}


	IEnumerator fadeOut() {

		Color color = image.color;
		time = 0f;
		color.a = Mathf.Lerp(start, end, time);

		while ( color.a > 0f ) {
			
			time += Time.deltaTime / animTime;
			color.a = Mathf.Lerp(start, end, time);
			image.color = color;

			yield return null;
		}
		gameObject.SetActive(false);
	}


	IEnumerator fadeIn() {

		image.color = new Color(image.color.r,image.color.g,image.color.b,1f);

		Color color = image.color;
		time = 0f;
		color.a = Mathf.Lerp(start, end, time);

		while ( color.a < 1f ) {
			
			time += Time.deltaTime / animTime;
			color.a = Mathf.Lerp(0f, 1f, time);
			image.color = color;

			yield return null;
		}
		
	}


	void OnEnable()
	{	
		if(gameObject.name == "White") {
			image.color = new Color(image.color.r,image.color.g,image.color.b,image.color.a);
			return;
		}
		image.color = new Color(image.color.r,image.color.g,image.color.b,1f);
	}


}
