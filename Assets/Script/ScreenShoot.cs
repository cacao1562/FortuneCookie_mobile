using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class ScreenShoot : MonoBehaviour {

	// private int resWidth = 150;
	// private int resHeight = 150;
	// private Camera _camera;
	// private Sprite TempImage;
	// public Image image;
	

	public GameObject shotButton, exitButton;
	/** 운세 텍스트 저장된거 가져옴 */
	private string getResult;
	/** 운세 텍스트 보여주는 ui */
	public Text textUi;
	/** 스샷찍을때 하얀배경 반짝 */
	public GameObject backWhite;
	/** 스샷 버튼 누르고 저장완료 팝업 */
	public GameObject savePopup;
	/** 카메라 권한 허용 안했을때 팝업 */
	public GameObject permissionPopup;
	void Awake()
	{
		
		// NativeGallery.CheckPermission();
		NativeGallery.RequestPermission();
		getResult = PlayerPrefs.GetString("result","null");
		
	}
	void Start () {

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		textUi.text = getResult;

		
		// NativeGallery.Permission permission = NativeGallery.CheckPermission();
		// if (permission == NativeGallery.Permission.Granted) 
		// {
		// 	Debug.Log("May proceed");
		// }
		// else 
		// {
		// 	Debug.Log("Not allowed");
		// 	// You do not break out of the function here so it will attempt to save anyways
		// }
	

		// NativeGallery.Permission permission = NativeGallery.CheckPermission();
		// if (permission == NativeGallery.Permission.ShouldAsk) 
		// {
		// 	permission = NativeGallery.RequestPermission();
		// 	Debug.Log("Asking");
		// }
		// // If we weren't denied but told to ask, this will handle the case if the user denied it.
		// // otherwise if it was denied then we return and do not attempt to save the screenshot
		// if (permission == NativeGallery.Permission.Denied) 
		// {
		// 	Debug.Log("Not allowed");
		// 	return;
		// }
		// Debug.Log("Path is "+NativeGallery.GetSavePath("GalleryTest","My_img_{0}.png"));
		// //Output ==>  /storage/emulated/0/DCIM/GalleryTest/My_img_1.png

		// Texture2D ss = new Texture2D( Screen.width, Screen.height, TextureFormat.RGB24, false );
		// ss.ReadPixels( new Rect( 0, 0, Screen.width, Screen.height ), 0, 0 );
		// ss.Apply();

		// Debug.Log("Secondlast");
		// permission = NativeGallery.SaveImageToGallery( ss, "GalleryTest", "My_img_{0}.png" ) ;

		// Debug.Log("Done screenshot");
	}
	
	// Update is called once per frame
	
	/** 종료 버튼 클릭했을때 */
	public void ExitButtonClicked(){

		if (Application.platform == RuntimePlatform.Android) {
			
			Application.Quit();

		}else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			
			NativeGallery.AppExit();
		
		}
	
	}

	/** 스샷 버튼 클릭했을때 */
	public void ScreenshotButtonClicked()
	{
		if(NativeGallery.CheckPermission() != NativeGallery.Permission.Granted){ //카메라 권한 허용 안했을때
		
			permissionPopup.SetActive(true);
			// StartCoroutine( showPopup() );
			return;

		}
   	 	StartCoroutine(TakeScreenshot());
	}

	private IEnumerator TakeScreenshot()
	{
		SetUIActive(false); //버튼 숨김

		yield return new WaitForEndOfFrame();

		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		NativeGallery.SaveImageToGallery(ss, "Native Screenshots", System.DateTime.Now.ToString().Replace("/", "-"));
		backWhite.SetActive(true);
		backWhite.GetComponent<FadeOut>().showFadeOut();
		
		yield return new WaitForSeconds(0.7f);
		savePopup.SetActive(true);
	
		// SetUIActive(true);
	}


	public void saveOKbuttonClicked() {

		savePopup.SetActive(false);
		SetUIActive(true);

	}

	public void permissionOKbuttonClicked() {

		permissionPopup.SetActive(false);
		
	}

	/** 스샷 찍을때 버튼 2개 잠깐 숨김 */
	private void SetUIActive(bool active)
	{
		
		shotButton.SetActive(active);
		exitButton.SetActive(active);
		
	}
	

}
	
// 	IEnumerator Shoot() {
		 
// 		//  string date = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
//         //  string myFilename = "myScreenshot_"+date+".png" ;
// 		//  string path = "/storage/emulated/0/Cookie/" + myFilename;

//    		// if(!System.IO.Directory.Exists("/storage/emulated/0/Cookie/")){
//         //     System.IO.Directory.CreateDirectory("/storage/emulated/0/Cookie/");
//         // }
// 		 yield return new WaitForEndOfFrame();
// 		 byte[] imageByte;
// 		 Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
// 		 tex.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0,true );
// 		 tex.Apply();
// 		 imageByte = tex.EncodeToPNG();
// 		 DestroyImmediate(tex);
// 		//  File.WriteAllBytes("/storage/emulated/0/MyPhoneAPP/test.png", imageByte);
// 		 Application.CaptureScreenshot("/storage/emulated/0/MyPhoneAPP/test.png");
// 		//  File.WriteAllBytes("../../../../DCIM/Camera/test.png", imageByte);
		
// 	}

// 	public void StartShoot(){
// 		StartCoroutine( Shoot() );
// 	}



// 	public void TakeHiResShot(){

//         string date = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
//         string myFilename = "myScreenshot_"+date+".png" ;
//         // debugText.text = ": " + myFilename;
//         string myDefaultLocation = Application.persistentDataPath + "/" + myFilename;
//         //EXAMPLE OF DIRECTLY ACCESSING THE Camera FOLDER OF THE GALLERY
//         //string myFolderLocation = "/storage/emulated/0/DCIM/Camera/";
//         //EXAMPLE OF BACKING INTO THE Camera FOLDER OF THE GALLERY
//         //string myFolderLocation = Application.persistentDataPath + "/../../../../DCIM/Camera/";
//         //EXAMPLE OF DIRECTLY ACCESSING A CUSTOM FOLDER OF THE GALLERY
//         string myFolderLocation = "/storage/emulated/0/Cookie/";
//         string myScreenshotLocation = myFolderLocation + myFilename;
//         //ENSURE THAT FOLDER LOCATION EXISTS
//         if(!System.IO.Directory.Exists(myFolderLocation)){
//             System.IO.Directory.CreateDirectory(myFolderLocation);
//         }
        
        
//         //TAKE THE SCREENSHOT AND AUTOMATICALLY SAVE IT TO THE DEFAULT LOCATION.
        
//         //  캔버스 포함 전체 스크린샷!!
//         //  Application.CaptureScreenshot(myScreenshotLocation);
//         //
//         //
//         //
        
        
        
//         //  캔버스 제외 카메라에 보이는 부분만 스크린 샷!!
//         RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
//         _camera.targetTexture = rt;
//         Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);        
//         Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);        
//         _camera.Render();
//         RenderTexture.active = rt;
		
//         screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
//         screenShot.Apply();
        
//         TempImage = Sprite.Create(screenShot, rec, new Vector2(0,0),.01f);
//         image.sprite = TempImage;

//         _camera.targetTexture = null;
//         RenderTexture.active = null; // JC: added to avoid errors
//         Destroy(rt);
        
//         byte[] bytes = screenShot.EncodeToPNG();
//         System.IO.File.WriteAllBytes(myScreenshotLocation, bytes);
        
        
        
//         // 안드로이드 갤러리, 사진첩 업데이트 부분
//         // 요거 안하면 "내파일" 에서는 보이지만 갤러리 및 사진첩 어플에서는 보이지 않는 문제가 생김!!!  
 
//         //MOVE THE SCREENSHOT WHERE WE WANT IT TO BE STORED
//         //  System.IO.File.Move(myDefaultLocation, myScreenshotLocation);
//         //REFRESHING THE ANDROID PHONE PHOTO GALLERY IS BEGUN
//         // AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//         // AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
//         // AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
//         // // "android.intent.action.MEDIA_SCANNER_SCAN_FILE" <--- 요거 햇갈림.. 원래 찾은건 "android.intent.action.MEDIA_MOUNTED" 요렇게 하라고 나와있는데 안되서 저렇게 하니 됨.
//         // AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2]{"android.intent.action.MEDIA_SCANNER_SCAN_FILE", classUri.CallStatic<AndroidJavaObject>("parse", "file://" + myScreenshotLocation)});
//         // objActivity.Call ("sendBroadcast", objIntent);
//         // debugText.text = "Complete! - " + myScreenshotLocation;
//         //REFRESHING THE ANDROID PHONE PHOTO GALLERY IS COMPLETE
//         //AUTO LAUNCH/VIEW THE SCREENSHOT IN THE PHOTO GALLERY!!!
//         // Application.OpenURL(myScreenshotLocation);
//         //AFTERWARDS IF YOU MANUALLY GO TO YOUR PHOTO GALLERY,
//         //YOU WILL SEE THE FOLDER WE CREATED CALLED "myFolder"
//         // count++;
        
//         // return TempImage;
		
//     }


