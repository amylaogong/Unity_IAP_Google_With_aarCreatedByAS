using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//UIMsgHandler.cs


public enum T_UI_MSG_TYPE
{
	//////////////
	none,//无，初始默认值。。以下将定于游戏使用的事件类型。
	show_ui_scroll_message,//消息太长的话就滚屏，没超过控件长度就定格屏幕上【string 参数】
	show_ui_slip_message,//消息整体从边缘滑动到屏幕里，可以指定滑动方向，上下左右[string参数是消息,int参数是方向]
	show_ui_alert_message,//普通的弹板消息，【string0是消息，string1是标题】
	///////////////
	end//结束标志。
	///////////////
};

public class UIMsgBody{
	public const string UI_MSG_ERROR_STR = "error";
	public T_UI_MSG_TYPE mType = T_UI_MSG_TYPE.none;
	//可以扩展的参数列表。
	private Dictionary<string,int> mParamDictionary_Int =new Dictionary<string,int>();//
	private Dictionary<string,float> mParamDictionary_Float =new Dictionary<string,float>();//
	private Dictionary<string,string> mParamDictionary_String =new Dictionary<string,string>();//

	public void AddParam(string key,int value){
		mParamDictionary_Int[key] = value;
	}
	public void AddParam(string key,string value){
		mParamDictionary_String[key] = value;
	}
	public void AddParam(string key,float value){
		mParamDictionary_Float[key] = value;
	}


	public int GetInt(string key){
		if(mParamDictionary_Int.ContainsKey(key)){
			return mParamDictionary_Int[key];
		}
		return -1;
	}
	public float GetFloat(string key){
		if(mParamDictionary_Float.ContainsKey(key)){
			return mParamDictionary_Float[key];
		}
		return -1.0f;
	}
	public string GetString(string key){
		if(mParamDictionary_String.ContainsKey(key)){
			return mParamDictionary_String[key];
		}
		return UI_MSG_ERROR_STR;
	}
}

public class UICsvData{//一段完整的csv数据
	public string prefabName = "";
	public string uiName = "";

	public string[] dataArray = null;
	public ArrayList props = new ArrayList();

	public void SetData(string str){
		if(str.Length<=0){
			return;
		}
		LogView.setViewText ("UIMsgHandler.cs,UICsvData,parseData,str=="+str);

		dataArray = str.Split ('\n');
		if(dataArray.Length>0){
			prefabName = dataArray[0].Replace("\r","");
			uiName = prefabName;
			LogView.setViewText ("UIMsgHandler.cs,UICsvData,parseData,uiName=="+uiName);

			for(int i=1;i<dataArray.Length;i++){
				LogView.setViewText ("i=="+i+",UIMsgHandler.cs,UICsvData,parseData,dataArray[i]=="+dataArray[i]);
				UIControlProperty prop = new UIControlProperty();
				prop.SetData (dataArray[i]);
				props.Add (prop);
			}
		}
	}
}


public class UIControlProperty{//单条csv数据控制控件属性
	public string propContent = "";
	public bool isLegal = true;//是否合法

	public string controlName_0;//Text_Content  	Button_Cancle/Text
	public string type_1;//"0"赋值,"1"复制,"2"缓存,"3"删除
	public string text_2="";//
	public string skin_3="";
	public byte visibleType_4=2;//0不可见,1可见,2不处理
	public byte enableType_5=2;

	public string newNameTotalStr_6="";//重命名的整体字符串
	public string newName="";//重命名


	public byte blinkType_7=2;
	public string command="";//点击之后发回服务器的指令

	public int propCount = 5;

	public string[] propArray = null;

	public void SetData(string str){
		propContent = str.Replace("\r","").Replace("\n","");
		ParseProp ();
	}

	public void ParseProp(){
		if(propContent.Length<=0){
			return;
		}

		propArray = propContent.Split ('|');
		if (propArray.Length > 2) {
			controlName_0 = propArray [0];
			type_1 = propArray [1];
			if (propArray [2].Length > 0) {
				text_2 = propArray [2];
			}
		} else {
			isLegal = false;
		}

		if(propArray.Length > 3  && propArray [3].Length>0){
			skin_3 = propArray [3];
		}

		if(propArray.Length > 4 && propArray [4].Length>0){
			visibleType_4 = byte.Parse (propArray [4]);
		}

		if(propArray.Length > 5 && propArray [5].Length>0){
			enableType_5 = byte.Parse (propArray [5]);
		}

		if(propArray.Length > 6 && propArray [6].Length>0){
			newNameTotalStr_6 = propArray [6];
			SetControlCsvName (newNameTotalStr_6);
		}
		if(propArray.Length > 7 && propArray [7].Length>0){
			blinkType_7 = byte.Parse (propArray [7]);
		}
		if(propArray.Length > 8 && propArray [8].Length>0){
			command = propArray [8];
		}
	}

	public void SetControlCsvName(string str){
		string[] sts =  str.Split ('%');//
		if(sts.Length>0){
			newName = sts[0];
		}
	}


}





