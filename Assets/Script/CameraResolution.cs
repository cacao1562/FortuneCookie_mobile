using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CameraResolution : MonoBehaviour {


    public Camera pixelCam; //카메라 배열.

    public float heightV; //픽셀 수
    public float widthV;

    public float x, y; // 카메라의 x,y만큼 떨어진 좌표이다.

    public float Size; // 카메라의 사이즈이다.
	public Canvas canvas1, canvas2;
	private List<string> textArr = new List<string>();
	public GameObject handObj;
	public GameObject p1, p2, p3;  //쿠키 파티클 색깔별로
    // Use this for initialization
    void Start () {

		string path = @"c:\info\info.ini";

		string[] textValue = System.IO.File.ReadAllLines(path);
     
		if (textValue.Length > 0) {

			for ( int i = 0; i < textValue.Length; i++) {

                textArr.Add(textValue[i]);
				//Debug.Log(i + 1 + " Line "+ " txt = " + textValue[i]);
			}
		}

        char st = '=';

        for(int i =0; i<textValue.Length; i++){

			string[] glassinfo = textValue[i].Split(st);

			if(glassinfo[0] == "offsetX"){

				x = float.Parse(glassinfo[1]);

			}else if(glassinfo[0] == "offsetY"){

                y = float.Parse(glassinfo[1]);

            }else if(glassinfo[0] == "glassWidth"){

                widthV = float.Parse(glassinfo[1]) ;

            }else if(glassinfo[0] == "glassHeight"){

                heightV = float.Parse(glassinfo[1]) ;
            } 
		}

        Screen.SetResolution(1024, 768, true);

		float scaleFactor; // 캔버스 스케일
		
		if(widthV > heightV) { //가로가 더 클때
			
			scaleFactor = heightV / 768;

		}else {
		
			scaleFactor = widthV / 1024;
		}
		Debug.Log("Canvas scale = " + scaleFactor );

		
		canvas1.scaleFactor = scaleFactor;
		// canvas2.scaleFactor = scaleFactor;	
        changTempCamVal();
		
        // Cursor.visible = false;
		
    }

    private void changTempCamVal()
    {
		
		pixelCam.rect = new Rect(0, ((768f - heightV) / 768f), widthV / 1024f, heightV / 768f);
		
		float rate = (1024f / widthV) * (heightV / 768f);
		// Debug.Log("rate = " + rate );
		// pixelCam[i].orthographicSize = 70f * rate;
		pixelCam.orthographicSize = 5;
		pixelCam.transform.position = new Vector3(x, y, pixelCam.transform.position.z);

		if(Size != 0)
		{

			pixelCam.orthographicSize = Size;

		}

    }


}
