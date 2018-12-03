using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragFork : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

	private bool clickedOn;
	public LineRenderer line1;
	public LineRenderer line2;
	private Vector3 v3;
	public GameObject[] forkImg;
	private int forkNum = 0;
	public Color blueColor;
	public Color redColor;
	public SocketManager socketManager;
	// public Material red , blue;

	public void OnBeginDrag(PointerEventData eventData) {
		// Debug.Log("Begin Drag");
		clickedOn = true;
	}	
	public void OnDrag(PointerEventData eventData) {
		// Debug.Log("Drag");
		clickedOn = true;
	}

	public void OnEndDrag(PointerEventData eventData) {
		// Debug.Log("End Drag");

		clickedOn = false;
		transform.position = v3;
		forkImg[forkNum].SetActive(true);
		forkNum += 1;
		socketManager.clickButton();
		if (forkNum == 4) {
			// line1.material = red;
			// line2.material = red;
			GetComponent<Image>().color = redColor;
		}
		if (forkNum == 5) {
			forkNum = 0;
			// line1.material = blue;
			// line2.material = blue;
			GetComponent<Image>().color = blueColor;
		}
		LineRendererUpdate();
	}

	void Start()
	{
		blueColor = GetComponent<Image>().color;

		v3 = transform.position;
		
		LineRendererSetup();
	}

	void LineRendererSetup() {

		line1.startWidth = .1f;
		line1.endWidth = .1f;
		line2.startWidth = .1f;
		line2.endWidth = .1f;

		line1.SetPosition(0, new Vector3(line1.transform.position.x - 0.2f, line1.transform.position.y, line1.transform.position.z) );
		line2.SetPosition(0, new Vector3(line2.transform.position.x + 0.2f, line2.transform.position.y, line2.transform.position.z) );

		// line1.sortingLayerName = "Fork";
		// line2.sortingLayerName = "Fork";
		line1.SetPosition(1, new Vector3(v3.x, v3.y -1, 89.5f) );
		// line2.SetPosition(0, line2.transform.position );
		line2.SetPosition(1, new Vector3(v3.x, v3.y -1 , 89.5f) );
		// line1.sortingLayerName = "Fork";
		// line2.sortingLayerName = "Fork";


	}

	void LineRendererUpdate() {

		Vector3 holdPoint = transform.position;
		line1.startWidth = .1f;
		line1.endWidth = .1f;
		line2.startWidth = .1f;
		line2.endWidth = .1f;
		// line1.SetPosition(0, line1.transform.position );
		// Debug.Log("hold yyy =  " + holdPoint.y);
		line1.SetPosition(1, new Vector3(holdPoint.x, holdPoint.y - 1 , 89.5f) );
		// line2.SetPosition(0, line2.transform.position );
		line2.SetPosition(1, new Vector3(holdPoint.x, holdPoint.y - 1 , 89.5f) );
	}


	void Update()
	{
		
		
		if (clickedOn) {
			Dragging();
			LineRendererUpdate();
		}

	}

	void Dragging() {

	

		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		// Debug.Log ( "mouse = " + mouseWorldPos );
		Debug.Log( "y = "+ Mathf.Abs(mouseWorldPos.y)) ;
		socketManager.sendPower( Mathf.Abs(mouseWorldPos.y).ToString("N2") );
		if (mouseWorldPos.y > -0.3f) {
			return;
		}
		mouseWorldPos.x = 0f;
		mouseWorldPos.z = 90f;

		transform.position = mouseWorldPos;
		
		
	}
}
