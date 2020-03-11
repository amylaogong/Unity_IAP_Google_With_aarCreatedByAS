using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
//使用说明：
外界使用接口：UIMsgTools.PopMessage(UIMsgBody msg);
msg为封装好的参数数据
*/

public class UIMsgTools {

	// Use this for initialization


	public static string alertPrefebRes = UIManager.UIPrefabPath + "Canvas_MsgAlert";
	public static string scrollPrefebRes = UIManager.UIPrefabPath +"Canvas_MsgScroll";
	public static string slipPrefebRes = UIManager.UIPrefabPath +"Canvas_MsgSlip";


	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public static void PopMessage(UIMsgBody msg){
		switch(msg.mType){
		case T_UI_MSG_TYPE.show_ui_alert_message:
			PopAlertMsg (msg);
			break;
		case T_UI_MSG_TYPE.show_ui_scroll_message:
			PopScrollMsg (msg);
			break;
		case T_UI_MSG_TYPE.show_ui_slip_message:
			PopSlipMsg (msg);
			break;
		default:
			break;
			
		}
	}

	public static void PopScrollMsg(UIMsgBody msg){
		GameObject objMsg = UIManager.LoadUI ("Canvas_MsgScroll");
		if (objMsg != null) {
			string content = msg.GetString ("content");//the message content
			float scrollSpeed = msg.GetFloat("scrollSpeed");//scroll speed of the message
			int viewType = msg.GetInt("viewType");//显示类型，0是按时间显示，1是按次数显示
			float needViewTime = msg.GetFloat("needViewTime");//
			int needViewCount = msg.GetInt("needViewCount");//

			objMsg.GetComponent<MsgScroll> ().SetContent (content);
			objMsg.GetComponent<MsgScroll> ().SetScrollSpeed (scrollSpeed);
			objMsg.GetComponent<MsgScroll> ().SetViewType (viewType);
			objMsg.GetComponent<MsgScroll> ().SetNeedViewTime (needViewTime);
			objMsg.GetComponent<MsgScroll> ().SetNeedViewCount (needViewCount);
		}
	}

	public static void PopAlertMsg(UIMsgBody msg){
		GameObject alertCanvas = UIManager.LoadUI ("Canvas_MsgAlert");
		if (alertCanvas != null) {
			string content = msg.GetString ("content");//the message content

			alertCanvas.name = "Canvas_MsgAlert00";
			alertCanvas.transform.Find("MsgAlert").GetComponent<MsgAlert> ().SetContent (content);

			string scriptFuncConfirm = msg.GetString ("scriptFuncConfirm");
			string scriptFuncParams = msg.GetString ("scriptFuncParams");
			//
			alertCanvas.transform.Find("MsgAlert").GetComponent<MsgAlert> ().SetConfirmEvemtParam (scriptFuncConfirm,scriptFuncParams);

		}
	}

	public static void PopSlipMsg(UIMsgBody msg){
		//GameObject prefeb = (GameObject)Resources.Load (slipPrefebRes);
		GameObject objMsg = UIManager.LoadUI ("Canvas_MsgSlip");
		if (objMsg != null) {

			string content = msg.GetString ("content");//the message content
			float moveSpeed = msg.GetFloat("moveSpeed");//move speed of the message
			int direction = msg.GetInt("direction");//

			objMsg.GetComponent<MsgSlip> ().SetContent (content);
			objMsg.GetComponent<MsgSlip> ().direction = direction;
		}
	}
		
}
