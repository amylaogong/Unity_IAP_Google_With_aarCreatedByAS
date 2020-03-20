using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : UISinggleton<UIManager> {
	/*
		功能：ui的获取，打开（通过csv或者对象），检测主面板上敌人信息可见性
	*/
    [SerializeField]
	public GameObject mainCanvas;//
    [SerializeField]
    public GameObject adaptCanvas;//需要做iPhone X适配的ui就加挂到这里
    [SerializeField]
    public GameObject minimapCameraObj;

	// Use this for initialization

	public static string UIPrefabPath = "Prefabs/";
	public static string UIImagePath = "Images/";
	public static bool isUIInitDone = false;

	private static Dictionary<string,GameObject> UIPrefabPools= new Dictionary<string,GameObject>();
	private static Dictionary<string,GameObject> UIObjPools= new Dictionary<string,GameObject>();

	public bool isIphoneX = false;
	public float edgeIphoneX = 40f;

	private float timePrint = 0;
	void Start () {
		isUIInitDone = true;
	}
	
	// Update is called once per frame
	void Update () {
		justTest ();

    }

	public void startMainUI(){
		StartCoroutine (OpenMainUI());
	}

	IEnumerator OpenMainUI(){
		timePrint = Time.time;
		yield return new WaitForSeconds (0.5f);

		string mainUIName = "Main";//"Main";

		GameObject mainUI = GetUI (mainUIName);//mainCanvas.transform.Find ("Main").gameObject;
		if (mainUI != null) {
			mainUI.SetActive (true);
		} else {
			LoadUI(mainUIName);
		}
  

    }



    void Reset(){
		foreach(string key in UIPrefabPools.Keys){
			Destroy (UIPrefabPools[key]);
		}
		UIPrefabPools.Clear ();
		Resources.UnloadUnusedAssets();
	}


	public static void ChangeLayer(Transform trans, int targetLayer)
	{
		//遍历更改所有子物体layer
		trans.gameObject.layer = targetLayer;
		foreach (Transform child in trans)
		{
			ChangeLayer(child, targetLayer);
		}
	}


	public static void SetRenderInMinimap(GameObject obj){
		if(obj!=null){
//			obj.AddComponent(typeof(Minimap_ViewObj));
		}
	}

	public static bool GetUIIsNeedAdaptIphoneX(string uiName){
		//根据配置决定是否适配，某些不需要，比如loading图

		return false;
	}

	public static GameObject GetUI(string uiName){
		GameObject canvasObj = null;
		if(UIManager.getInstance ().mainCanvas!=null){
			Transform ts = UIManager.getInstance ().mainCanvas.transform.Find (uiName);
			if (ts == null) {
				ts = UIManager.getInstance ().adaptCanvas.transform.Find (uiName);
			}
			if (ts != null) {
				canvasObj = ts.gameObject;	
			}
			//LogView.setViewText ("getUI,canvasObj=="+canvasObj);
		}
		return canvasObj;
	}

	public static GameObject GetPrefab(string key){
		string prefabName = UIPrefabPath + key;//prefab
		GameObject canvasPrefeb = null;
		if(UIPrefabPools.ContainsKey(key)){
			canvasPrefeb = UIPrefabPools[key];
		}
		if(canvasPrefeb==null){
			canvasPrefeb = (GameObject)Resources.Load (prefabName);
			UIPrefabPools.Add (key,canvasPrefeb);
		}
		return canvasPrefeb;
	}

	public static void CloseUI(string name){
		if(UIObjPools.ContainsKey(name)){
			Destroy (UIObjPools[name]);
		}
	}

	public static GameObject LoadUI(string name){
		if (UIManager.getInstance ().mainCanvas == null) {
			return null;
		}
		GameObject canvasPrefeb = GetPrefab(name);

		LogView.setViewText ("loadUI,canvasPrefeb11=="+canvasPrefeb);
		GameObject canvasObj = UIManager.Instantiate (canvasPrefeb) as GameObject;
		if (canvasObj != null) {
			Transform setParent = UIManager.getInstance().mainCanvas.transform;
			if(GetUIIsNeedAdaptIphoneX(name)){
				setParent = UIManager.getInstance().adaptCanvas.transform;
			}
			canvasObj.transform.parent = setParent;
			canvasObj.transform.localPosition = new Vector3 (0, 0, 0);
			canvasObj.transform.localScale = Vector3.one;
			canvasObj.name = name;

			RectTransform rt = canvasObj.GetComponent<RectTransform> ();
			if(rt!=null){
				rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
				rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
				rt.anchorMin = Vector2.zero;
				rt.anchorMax = Vector2.one;
			}

			UIObjPools [name] = canvasObj;
		}
		return canvasObj;
	}


	public static void OpenUIByCsvData(UICsvData data){
		LogView.setViewText ("OpenUIByCsvData,UICsvData,begin...");
		if(data==null || data.uiName.Length<=0 || data.dataArray.Length<=0){
			return;
		}
		GameObject canvasObj = GetUI(data.uiName);
		LogView.setViewText ("OpenUIByCsvData,firstFind_canvasObj=="+canvasObj);

		if(canvasObj==null){//找不到就加载
			string prefebName = data.prefabName;//
			LogView.setViewText ("OpenUIByCsvData,prefebName=="+prefebName);
			canvasObj = LoadUI (prefebName);
		}

		if (canvasObj != null) {
			canvasObj.name = data.uiName;
			foreach(UIControlProperty prop in data.props){
				SetObjUIControlByProp (canvasObj,prop);
			}
		}
	}

	public static void OpenUIByCsvStr(string csvStr){
		LogView.setViewText ("OpenUIByCsvData,csvStr=="+csvStr);
		UICsvData data = new UICsvData ();
		data.SetData (csvStr);
		if(data.uiName.Length<=0 || data.dataArray.Length<=0){
			return;
		}
		OpenUIByCsvData (data);
	}


	public static void SetObjUIControlByProp(GameObject obj, UIControlProperty prop){
		if(obj==null || prop==null){
			LogView.setViewText ("setObjUIControlByProp,error:obj=="+obj+",prop=="+prop);
			return;
		}
		if(prop.isLegal == false){
			return;
		}

		Transform ts = obj.transform.Find (prop.controlName_0);
		if(ts==null){
			LogView.setViewText ("setObjUIControlByProp,control not exist!controlName=="+prop.controlName_0);
			return;
		}

		GameObject control = ts.gameObject;
		if(prop.type_1=="0"){//"0"赋值,"1"复制,"2"缓存,"3"删除
			SetControlProperty(control,prop);
		}
	}

	public static GameObject CopyControlByProperty(GameObject srcObj,GameObject parentObj,UIControlProperty prop){
		//以srcObj为模板，复制一个控件，添加到parentObj下面
		GameObject cloneObj = Instantiate (srcObj,srcObj.transform.position,srcObj.transform.rotation);
		cloneObj.transform.parent = parentObj.transform;
		cloneObj.transform.localPosition = new Vector3 (srcObj.transform.localPosition.x,srcObj.transform.localPosition.y,0);
		cloneObj.transform.localScale = Vector3.one;

		return cloneObj;
	}

	public static void SetControlProperty(GameObject control,UIControlProperty prop){
		//对具体某个控件进行赋值，文本，皮肤，可见性。可用性，新的name等

		if(prop.text_2.Length>0){//设置wenben
			Text tx = control.GetComponent<Text> ();
			if(tx!=null){
				tx.text = prop.text_2;
			}
		}
		if(prop.skin_3.Length>0){//设置皮肤
			Image im = control.GetComponent<Image> ();
			if (im != null) {
				im.sprite = Resources.Load (prop.skin_3, typeof(Sprite)) as Sprite;
			}
		}
		if(prop.visibleType_4!=2){
			control.SetActive (prop.visibleType_4==1);
		}
		if(prop.enableType_5!=2){
			Button btn = control.GetComponent<Button> ();
			if(btn!=null){
				btn.interactable = (prop.enableType_5==1);
			}
		}
		if(prop.newNameTotalStr_6.Length>0){
			string[] sts =  prop.newNameTotalStr_6.Split ('%');//
			if(sts.Length>0){
				control.name = sts[0];
			}
			string eventType = "";
			if(sts.Length>1){
				eventType = sts[1];
			}
			LogView.setViewText ("SetControlProperty(),prop.newNameTotalStr_6=="+prop.newNameTotalStr_6);
			switch(eventType){
			case "pop"://设置控件事件 点击该控件后弹出提示板 并为ok按钮赋服务器所需的命令 newName1%pop%des%cmd%okText%cancelText

				UIEvent.Get (control).SetEventParam (prop.newNameTotalStr_6);
				UIEvent.Get (control).OnClick = ShowPopMsg;
				break;

			case "csp"://设置控件点击事件 csp@script@argString
				
				
				break;

			case "snd":
				
				break;
			default:
				break;
			}


		}

	}

	public static void ShowPopMsg(UIEventObj obj){
		UIEventFuncs.InvokeFunc (UIEventFuncs.alertMsgFunc,obj.GetParam());
		//UIEventFuncs.funcObj.transform.SendMessage(UIEventFuncs.alertMsgFunc,obj.GetParam());
	}


    public enum E_UI_MSG
    {
        open_role_dead,//开启 主角 死亡UI
        close_role_relive,//关闭 主角复活 UI
        open_boss_dead,//
    };
    public static void ReceiveMsg(E_UI_MSG msg)
    {
        
    }


	public void justTest(){

		if(Time.time - timePrint > 2.0f){
			timePrint = Time.time;
//			Debug.Log ("justTest(),Camera.main.pos=="+Camera.main.gameObject.transform.position);
//			Debug.Log ("justTest(),Camera.main.rot=="+Camera.main.gameObject.transform.rotation.eulerAngles);
			//GameObject UICam = gameObject.transform.Find ("Camera").gameObject;
			//Debug.Log ("justTest(),UICam=="+UICam);
//			GameObject MiniMapCam = gameObject.transform.Find ("CameraMiniMap").gameObject;
//			Debug.Log ("justTest(),MiniMapCam.pos=="+MiniMapCam.transform.position);
//			Debug.Log ("justTest(),MiniMapCam.rot=="+MiniMapCam.transform.rotation.eulerAngles);


		}



		return;


		if(Input.GetKeyDown(KeyCode.O)){
			string testCsv = @"Canvas_MsgAlert
MsgAlert/Text_Content|0|花费1000金币可以再抽1个宝箱|
MsgAlert/Image_title/Text_title|0|警告||
MsgAlert/Button_Close|0||UI/Images/Main/DirCN|
MsgAlert/Button_Cancle|0|||0|
MsgAlert/Button_Confirm|0||||0|
MsgAlert/Button_Confirm|0||||1|newName1%pop%descomfirm%rc:test:0987:hhh%okText%cancelText|
";

			OpenUIByCsvStr (testCsv);
		}

		if (Input.GetKeyDown (KeyCode.J)) {
			UIMsgBody msg = new UIMsgBody();
			msg.mType = T_UI_MSG_TYPE.show_ui_alert_message;
			string content = "hello,i am a alert message";
			msg.AddParam ("content",content);
			msg.AddParam ("scriptFuncConfirm","SendCmd");
			msg.AddParam ("scriptFuncParams","rc:0456:testSendCmd");
			UIMsgTools.PopMessage(msg);
		}
		if (Input.GetKeyDown (KeyCode.K)) {
			UIMsgBody msg = new UIMsgBody();
			msg.mType = T_UI_MSG_TYPE.show_ui_scroll_message;
			string content = "hello,i am a scroll message,hello world!";
			msg.AddParam ("content",content);
			msg.AddParam("viewType",1);
			msg.AddParam("needViewCount",3);
			msg.AddParam ("scrollSpeed",20.0f);
			msg.AddParam("needViewTime",5.0f);
			UIMsgTools.PopMessage(msg);
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			string content = "hello,i am a slip message";
			int direction = 0;
			UIMsgBody msg = new UIMsgBody();
			msg.mType = T_UI_MSG_TYPE.show_ui_slip_message;
			msg.AddParam ("content",content);
			msg.AddParam("direction",direction);
			UIMsgTools.PopMessage(msg);

		}
		if (Input.GetKeyDown (KeyCode.P)) {
			
//			GameSpriteRole role = GameCommon.GetRole();
//			if (role != null) {
//				SetRenderInMinimap(role.Get_Object());
//			}
		}



	}


}
