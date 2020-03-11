using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScrollLog : MonoBehaviour {

	public static int logIndex = 0;
	static GameObject content;
	static GameObject txtPrefeb;
	static GameObject scrollObj;

	// Use this for initialization
	void Start () {
		scrollObj = gameObject;
		content = gameObject.transform.Find ("Viewport/Content").gameObject;
		txtPrefeb = (GameObject)Resources.Load ("Prefabs/Text_Log");//(GameObject)Instantiate();

		Debug.Log ("ScrollLog.cs,Start(),content=="+content);
		Debug.Log ("ScrollLog.cs,Start(),txtPrefeb=="+txtPrefeb);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public static void AddLog(object str){
		logIndex++;
		DateTime nowTime = DateTime.Now.ToLocalTime();
		string dateStr = nowTime.ToString("[yyyyMMdd_HHmmss]");

		if(txtPrefeb == null){
			Debug.Log (str);
			return;
		}

		string logview = logIndex+"_<color=green>" + dateStr + "</color>: " + str;
		GameObject textObj = (GameObject)Instantiate(txtPrefeb);
		textObj.transform.SetParent (content.transform,false);
		textObj.GetComponent<Text> ().text = logview;
		Debug.Log (logview);

		Canvas.ForceUpdateCanvases();
		scrollObj.GetComponent<ScrollRect> ().verticalNormalizedPosition = 0f;
		Canvas.ForceUpdateCanvases();
	}


}
