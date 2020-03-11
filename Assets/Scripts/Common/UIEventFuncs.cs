using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class UIEventFuncs : MonoBehaviour {
	/*
	功能：动态设置UI控件的事件
	*/

	public static GameObject funcObj;
	public static string alertMsgFunc = "ShowAlertMsg";

	private static UIEventFuncs instance = null;


	// Use this for initialization
	void Start () {
		instance = this;
		funcObj = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SendCmd(string cmd){
		LogView.setViewText ("UIEventFuncs.cs,SendCmd(),cmd=="+cmd);

	}

	public void ShowServerMsg(string param){
		LogView.setViewText ("UIEventFuncs.cs,ShowServerMsg(),param=="+param);

	}
	public void ShowAlertMsg(string param){
		//设置控件事件 为弹出提示板ok按钮赋服务器所需的命令 newName1%pop%des%cmd%okText%cancelText
		LogView.setViewText ("UIEventFuncs.cs,ShowAlertMsg(),param=="+param);

	}



	public static void InvokeFunc(string funcName,string param){
		LogView.setViewText ("UIEventFuncs.cs,InvokeFunc(),funcName=="+funcName+"...param=="+param);

		Type t = typeof(UIEventFuncs);
		MethodInfo method = t.GetMethod (funcName);
		if(method!=null){
			System.Object[] obj = new System.Object[]{param};
			method.Invoke (UIEventFuncs.instance,obj);
		}
	}



}
