using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SocketManager : MonoBehaviour
{
	private SocketIOComponent socket;
	private string roomNumber = "cookie";  //방 번호
	private JSONObject mobj;
	/** 날아가는 포크 이미지  */
	public Image[] forkImgArr = new Image[5];
	/** 왼쪽 작은 포크 이미지 */
	public GameObject[] miniForkImg = new GameObject[5];
	/** 포크 날아갈때 파티클 */
	public GameObject[] particleObj = new GameObject[5];
	/** 소켓 데이터(무엇이든) 받았을때 true */
	private bool check = false;
	private int forkNum = 0;
	private int forkCount = 5;
	/** 포크 버튼 */
	public Button shotBtn;
	public GameObject shotButton;
	/** 포크 그림자 */
	public Image shadow;
	/** 마지막 빨간 포크 색깔*/
	public Color redColor;
	/** 포크 다 날렸을때 팝업 */
	public GameObject gameOverPopup;
	/** 쿠키를 맞췄을때 소켓 result를 받았을때 true */
	private bool overCheck = false;
	/** 처음에 나오는 파란 팝업 */
	public GameObject firstPopup;
	/** first팝업 위로 올라가는 애니메이션 */
	private Animator mAnimator;
	/** first팝업 안에 텍스트 */
	public Text connectText;
	/** first팝업 안에 종료버튼이랑 모래시계이미지 */
	public GameObject exitButton, hourglassImg;
	/** 인터넷 연결안됬을때 팝업 */
	public GameObject internetPopup;
	public Image plateImg;
	private bool firstUser = false;
	private string uuid;
	public Image forkImg;
	public GameObject line1, line2;

	void Awake()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			//인터넷 연결 안되었을때
			internetPopup.SetActive(true);
		}
		uuid = SystemInfo.deviceUniqueIdentifier;
		Debug.Log("uuid = " + uuid );
		
	}

	public void Start() 
	{

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		// Screen.SetResolution(Screen.width, Screen.width / 9 * 16 , true);
		
        Debug.Log("roomNum = "+roomNumber);
		mAnimator = firstPopup.GetComponent<Animator>();
		socket = GameObject.Find ("SocketIO").GetComponent<SocketIOComponent> ();
		socket.On ("open", OnOpen);
		socket.On ("drawing", OnGetValue);
		socket.On ("error", OnError);
		socket.On ("close", OnClose);
		
		
	}

	// public void clickbtn(){

	// 	string s = NativeGallery.GetPath("Native Screenshots");
	// }

	public void clickButton() {

		if( forkNum > 4 ){

			Debug.Log("shoot end");
			return;
			
		}

		if ( forkCount == 0) {
			
			Debug.Log("end count");
			return;
		}
		// StartCoroutine( buttonInteractable() );
		
		forkCount -= 1;
		

		forkImgArr[forkNum].gameObject.SetActive(true); //포크 이미지 위로 날리기
		particleObj[forkNum].SetActive(true); //날아갈때 파티클
		miniForkImg[forkNum].SetActive(false); //왼쪽 미니포크 하나씩 꺼주기
		
		sendSocket(); 
		forkNum += 1;

		// if(forkNum == 4) { //마지막 포크 빨간색으로 바꾸기

		// 	shotButton.GetComponent<Image>().color = redColor;
		
		// }
		
	}

	/** 포크 버튼 눌렀을떄 소켓 shoot 전송 */
	public void sendSocket() {

		JsonModel jm = new JsonModel();
		jm.sendStr = "shoot";
		JSONObject jo = new JSONObject(JsonUtility.ToJson(jm) );
		socket.Emit("send", jo );
	}

	/** socket open될때 connect 소켓 보냄 */
	public void sendConnect() {

		JsonModel jm = new JsonModel();
		jm.sendStr = "connect";
		jm.result = uuid;
		JSONObject jo = new JSONObject(JsonUtility.ToJson(jm) );
		socket.Emit("send", jo );
	}

	public void sendSuccess() {

		JsonModel jm = new JsonModel();
		jm.sendStr = "success";
		JSONObject jo = new JSONObject(JsonUtility.ToJson(jm) );
		socket.Emit("send", jo );
	}


	public void sendPower( string power) {

		JsonModel jm = new JsonModel();
		jm.sendStr = "power";
		jm.result = power;
		JSONObject jo = new JSONObject(JsonUtility.ToJson(jm) );
		socket.Emit("send", jo );
	}

	/** gameOver 팝업에 ok버튼 누르면 앱 종료 */
	public void clickedOkbutton() {

		if(Application.platform == RuntimePlatform.Android) {
		
			Application.Quit();

		}else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			
			NativeGallery.AppExit();
		
		}
		
	}

	/** 홈키나 메뉴버튼 화면 껐을때 게임 다시 시작하게 state 소켓 보냄 */
	public void sendState() {

		JsonModel jm = new JsonModel();
		jm.sendStr = "state";
		JSONObject jo = new JSONObject(JsonUtility.ToJson(jm) );
		socket.Emit("send", jo );
	}

	/** 홈키, 메뉴, 화면종료했을때  씬 다시 시작 */
	void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus && firstUser ) {

			sendState();
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex );
		}
	}

	void Update()
	{

		if(overCheck) {

			gameOverPopup.SetActive(false);

		 }//else if(overCheck == false && forkNum == 5) { //쿠키를 못맞추고 포크 다 썼을때

		// 		gameOverPopup.SetActive(true);
				
		// }

		if(forkNum == 5){ //포크 5개 다 날렸을때 버튼이랑 그림자 숨김
			
			shadow.gameObject.SetActive(false);
			shotButton.SetActive(false);

		}

		if(!socket.IsConnected){

			Debug.Log("소켓 연결 안됨");
			return;
		}

		if(mobj == null){
			return;
		}

		if(mobj.Count <= 0){
			return;
		}	
   		
		

		if (check) {
			
			JsonModel jm = JsonUtility.FromJson<JsonModel>(mobj.ToString());

			if(jm.sendStr == "connect" && firstUser == false) { //글라스에서 connect받으면 카운트 하나올려서 보내줌

				if (jm.result == uuid ) { //한 사람만 접속 되게
					
					firstUser = true;
					sendSuccess();
					Debug.Log(" glass connected ");
					check = false;

				} else {

					check = false;
					Debug.Log(" uuid 다름 ");
					
				}
				check = false;
				
			}else if (jm.sendStr == "success" ) {
			
				if(firstUser) {
			
					plateImg.sprite = Resources.Load<Sprite>( "dish/" + jm.result );
					Debug.Log("dish Number = " + jm.result );
					StartCoroutine( closePopup() );
					check = false;

				}
				
			}else if(jm.sendStr == "pause" ){ //접시 맞춰서 깨졌을때 

				if (firstUser) {

					// Handheld.Vibrate();
					forkImg.enabled = false;
					// shotBtn.interactable = false; 
					StartCoroutine( waitButton() );
					check = false;
				}
			// }else if(jm.sendStr == "continue") {

			// 	// silverButton.interactable = true;
			// 	shotBtn.interactable = true;
			// 	check = false;

			}else if(jm.sendStr == "result") { //쿠키 맞췄을때

				if (firstUser) {
				
					overCheck = true;
					PlayerPrefs.SetString("result", jm.result); //글라스에서 보내준 운세 텍스트 프리퍼런스에 저장 다음 씬에서 불러올려고.
					PlayerPrefs.Save();
					StartCoroutine( nextScene() );
					check = false;

				}
				
			}else if(jm.sendStr == "gameOver") { //쿠키 못맞추고 끝났을때 
				
				if (firstUser) {
				
					check = false;
					gameOverPopup.SetActive(true);
					gameObject.SetActive(false);
				}
				
			 }//else if(jm.sendStr == "gameStart") { //글라스에서 게임오버됬을때 또는 운세 텍스트 보여주고 몇초후에 씬 다시시작할때 보내줌
				
			// 	if (firstUser) {
					
			// 		check = false;
			// 		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex );
					
			// 	}
				

			// }

		}else{

		
		}

		
	}

	/** 접시 맞춰서 깨졌을때 포크 버튼 못누르게 잠깐 disable 시키고 다시 활성화 */
	IEnumerator waitButton() {

		yield return new WaitForSeconds(1.3f);
		forkImg.enabled = true;
		// shotBtn.interactable = true;
	}

	/** 글라스와 연결 되었을 때 */
	IEnumerator closePopup() {

		connectText.text = "연결 되었습니다! \n 잠시 기다려주세요.";
		exitButton.SetActive(false);
		hourglassImg.SetActive(true);
		yield return new WaitForSeconds(5.5f);
		mAnimator.enabled = true;
		yield return new WaitForSeconds(1.0f);
		line1.SetActive(true);
		line2.SetActive(true);
		yield return new WaitForSeconds(3.0f);
		firstPopup.SetActive(false);
	}

	/** 쿠키 맞췄을때 잠깐 기다렸다 씬 넘어감 */
	IEnumerator nextScene() {

		yield return new WaitForSeconds(4.0f);
		SceneManager.LoadScene("Scene/resultScene1");

	}

	public void OnOpen(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Open(): " + e.data);
	
		socket.Emit("joinRoom", JSONObject.StringObject(roomNumber)); //방번호
		sendConnect();	
	}
	
	public void OnGetValue(SocketIOEvent e)
	{
		// Debug.Log("get_Value: " + e.data);
		
		if (e.data == null) {
			// Debug.Log(" data nulllllll ");
			return; 
		}
		mobj = e.data; 
		check = true;
		
	}
	
	public void OnError(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Error(): " + e.data);
	}
	
	public void OnClose(SocketIOEvent e)
	{	
		Debug.Log("[SocketIO] Close(): " + e.data);
	}


}

[System.Serializable]
public class JsonModel{
	public string sendStr;
	public string result;
}


