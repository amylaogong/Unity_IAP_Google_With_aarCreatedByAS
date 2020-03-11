using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MsgAlert : MonoBehaviour {
	//弹板提示消息，可点击确定取消

	Text text_title;
	Text text_content;
	GameObject btn_close;
	GameObject btn_cancle;
	GameObject btn_comfirm;

	string scriptFuncConfirm = "";
	string scriptFuncParams = "";

	// Use this for initialization

	void Awake(){
		text_title = gameObject.transform.Find ("Image_title/Text_title").GetComponent<Text> ();
		text_content = gameObject.transform.Find ("Text_Content").GetComponent<Text> ();
		btn_close = gameObject.transform.Find ("Button_Close").gameObject;
		btn_cancle = gameObject.transform.Find ("Button_Cancle").gameObject;
		btn_comfirm = gameObject.transform.Find ("Button_Confirm").gameObject;

		UIEvent.Get (btn_comfirm).OnClick = ClickConfirm;
		UIEvent.Get (btn_cancle).OnClick = ClickCancle;
		UIEvent.Get (btn_close).OnClick = ClickClose;

		if(false){
			LogView.setViewText ("MsgAlert.cs,Awake(),text_title=="+text_title);
			LogView.setViewText ("MsgAlert.cs,Awake(),text_content=="+text_content);
			LogView.setViewText ("MsgAlert.cs,Awake(),btn_close=="+btn_close);
			LogView.setViewText ("MsgAlert.cs,Awake(),btn_cancle=="+btn_cancle);
			LogView.setViewText ("MsgAlert.cs,Awake(),btn_comfirm=="+btn_comfirm);
		}

	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetTitle(string t){
		if(text_title==null){
			text_title = gameObject.transform.Find ("Image_title/Text_title").GetComponent<Text> ();
		}
		if(text_title!=null){
			text_title.text = t;
		}
	}
	public void SetContent(string c){
		LogView.setViewText ("MsgAlert.cs,Start(),c=="+c);

		if(text_content==null){
			text_content = gameObject.transform.Find ("Text_Content").GetComponent<Text> ();
		}
		LogView.setViewText ("MsgAlert.cs,setContent(),text_content=="+text_content);
		if(text_content!=null){
			text_content.text = c;
		}
	}

	public void SetConfirmEvemtParam(string func,string param){
		if(!func.Equals("error")){
			scriptFuncConfirm = func;
		}
		if(!param.Equals("error")){
			scriptFuncParams = param;
			if(btn_comfirm==null){
				btn_comfirm = gameObject.transform.Find ("Button_Confirm").gameObject;
			}
			UIEvent.Get (btn_comfirm).SetEventParam (scriptFuncParams);
			//UIEvent.Get (btn_comfirm).OnLongPressed = OnEvent_LongPressed;
			//UIEvent.Get (btn_comfirm).OnDoubleClick = OnEvent_DoubleClick;
		}
	}
	public void SetConfirmText(string text){
		if(btn_comfirm==null){
			btn_comfirm = gameObject.transform.Find ("Button_Confirm").gameObject;
		}
		btn_comfirm.transform.Find("Text").GetComponent<Text> ().text = text;
	}
	public void HideConfirm(){
		if(btn_comfirm==null){
			btn_comfirm = gameObject.transform.Find ("Button_Confirm").gameObject;
		}
		btn_comfirm.SetActive (false);
	}
	public void HideCancle(){
		if(btn_cancle==null){
			btn_cancle = gameObject.transform.Find ("Button_Cancle").gameObject;
		}
		btn_cancle.SetActive (false);
	}
	public void HideClose(){
		if(btn_close==null){
			btn_close = gameObject.transform.Find ("Button_Close").gameObject;
		}
		btn_close.SetActive (false);
	}


	IEnumerator SetButtonOnClickEvent(UIEvent.VoidDelegate confirm=null,UIEvent.VoidDelegate cancle = null,UIEvent.VoidDelegate close = null){

		yield return new WaitForSeconds(0.2f);

		if(confirm==null){
			confirm = ClickConfirm;
		}
		if(cancle==null){
			cancle = ClickCancle;
		}
		if(close==null){
			close = ClickClose;
		}
		UIEvent.Get (btn_comfirm).OnClick = confirm;
		UIEvent.Get (btn_cancle).OnClick = cancle;
		UIEvent.Get (btn_close).OnClick = close;

		LogView.setViewText ("MsgAlert.cs,setButtonOnClickEvent()..confirm=="+confirm);
		yield return 0;
	}

	public void ClickClose(UIEventObj obj){
		if(false){
			LogView.setViewText ("MsgAlert.cs,clickClose()..");
			LogView.setViewText ("MsgAlert.cs,clickClose()..obj.gameObject.transform.parent.gameObject.name=="+obj.gameObject.transform.parent.gameObject.name);

			LogView.setViewText ("MsgAlert.cs,clickClose()..obj.gameObject.transform.parent.parent.gameObject.name=="+obj.gameObject.transform.parent.parent.gameObject.name);
		}

		string rootName = obj.gameObject.transform.parent.parent.gameObject.name;//Canvas_MsgAlert (Clone);
		string finalName = rootName.Substring(0,"Canvas_MsgAlert".Length);

		LogView.setViewText ("MsgAlert.cs,clickClose()..finalName=="+finalName);
		if(finalName=="Canvas_MsgAlert"){
			Destroy (obj.gameObject.transform.parent.parent.gameObject); 
			return;
		}
		Destroy (obj.gameObject.transform.parent.gameObject); 
	}
	public void ClickCancle(UIEventObj obj){
		LogView.setViewText ("MsgAlert.cs,clickCancle()..");
		ClickClose (obj);
	}
	public void ClickConfirm(UIEventObj obj){
		LogView.setViewText ("MsgAlert.cs,clickConfirm()..");

		if(scriptFuncConfirm.Length>0 && !scriptFuncConfirm.Equals("error")){

			//UIEventFuncs.funcObj.transform.SendMessage(scriptFuncConfirm,scriptFuncParams);
			UIEventFuncs.InvokeFunc (scriptFuncConfirm,obj.GetParam());

		}

		ClickClose (obj);
	}

	public void OnEvent_LongPressed(UIEventObj obj){
		LogView.setViewText ("MsgAlert.cs,OnEvent_LongPressed()..");

	}
	public void OnEvent_DoubleClick(UIEventObj obj){
		LogView.setViewText ("MsgAlert.cs,OnEvent_DoubleClick()..");

	}

	//for test 
	public void ChangeText(UIEventObj obj){
		SetContent ("你点击了确定，你看看能显示不能？？");
	}
}
