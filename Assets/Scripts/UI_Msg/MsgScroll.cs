using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//滚屏消息，可按照次数或时间配置消失条件
//功能可以扩充，比如设置文本颜色，位置等等

public class MsgScroll : MonoBehaviour {

	public static int scrollMsgCount = 0;


	public float scrollSpeed = 20.5f;
	public int viewType = 1;//显示类型，0是按时间显示，1是按次数显示
	public float needViewTime = 20.0f;
	public int needViewCount = 3;
	int scrollCount;//滚动次数
	float scrollTime;//滚动时间

	float beginTime;//
	int textImageLen = 0;//文本最终生成的图片长度，字符所占的图片长度，字符长度
	// Use this for initialization
	Text msg;
	string oldMsgText;
	float beginx = 0.0f;//计算好的x初始位置
	float curX = 0.0f;
	float moveLen = 0.0f;//字符串移动的距离
	bool isDead;
	bool isInitDone = false;

	GameObject scrollBack;

	void Start () {
		oldMsgText = "";
		scrollBack = gameObject.transform.Find ("MsgScroll").gameObject;
		GameObject obj = gameObject.transform.Find ("MsgScroll/msg").gameObject;//找到对象
		LogView.setViewText("ScrollMsg.cs,Start (),msg222=="+obj.name);
		msg = obj.GetComponent<Text>();//控件只是在对象上增加的挂件属性，需要获取挂件
		LogView.setViewText("ScrollMsg.cs,Start (),msg444=="+msg);
		//float msgUIWidth = gameObject.GetComponent<RectTransform>().rect.width;//换算之后的控件逻辑宽高，计算大小用这个
		//msg.gameObject.transform.position = new Vector3(msgUIWidth,msg.transform.position.y,msg.transform.position.z);
		beginTime = Time.time;
		scrollTime = 0.0f;
		scrollCount = 0;
		isDead = false;
		isInitDone = true;
		//setText ("hello,wakaka,hello,nibushiwonizazhidta,nishuohuaa,nizhidwlobe");
		//setText ("lalal");
	}
	
	// Update is called once per frame 
	void Update () {
		scroll ();
	}

	public void SetContent(string c){//此时可能start函数还没有执行而获取不到控件，在此等待执行完start之后再设置内容
		StartCoroutine(SetContentTillStartDone(c));
	}
	public void SetScrollSpeed(float v){
		scrollSpeed = v;
	}
	public void SetViewType(int tp){
		viewType = tp;
	}
	public void SetNeedViewTime(float t){
		needViewTime = t;
	}
	public void SetNeedViewCount(int count){
		needViewCount = count;
	}

	IEnumerator SetContentTillStartDone(string c){
		while(msg==null){
			yield return new WaitForEndOfFrame ();

		}
		if(msg){
			msg.text = c;
		}
		yield return new WaitForEndOfFrame ();
	}



	public void killed(){
		isDead = true;
		scrollMsgCount--;
		Destroy(gameObject);
	}

	public void scroll(){
		scrollTime = Time.time - beginTime;

		if(viewType==0){
			if(scrollTime > needViewTime){
				killed ();
			}
		}else if(scrollCount > needViewCount){
			killed ();
		}
		if(isDead){
			return;
		}


		calucateMsgImageLen();
		/*
		LogView.setViewText("ScrollMsg.cs,scroll (),textImageLen=="+textImageLen);
		LogView.setViewText("ScrollMsg.cs,scroll (),getWidth=="+getWidth());
		LogView.setViewText("ScrollMsg.cs,scroll (),getHeight=="+getHeight());
		LogView.setViewText("ScrollMsg.cs,scroll (),getX=="+getX());
		LogView.setViewText("ScrollMsg.cs,scroll (),getY=="+getY());

		LogView.setViewText("ScrollMsg.cs,scroll (),msgX=="+msg.gameObject.transform.position.x);
		*/


		if (textImageLen < getWidth()) {//文本没有超出显示控件范围
			viewType = 0;
			float msgControlWidth = msg.GetComponent<RectTransform> ().rect.width;//text控件宽度
			curX = 0;

		} else {
			float curMoveLen = scrollSpeed * Time.deltaTime;
			moveLen += curMoveLen;
			curX -= curMoveLen;

			if(moveLen > textImageLen + getWidth()){
				initMsgBeginX ();
			}
		}

		//msg设置的锚点是左侧，即最终结果是text的最左侧坐标是所设置的坐标x
		//msg.gameObject.transform.position = new Vector3(curX,msg.transform.position.y,msg.transform.position.z);

		msg.gameObject.transform.localPosition = new Vector3 (curX,0,0);

	}

	public int calucateMsgImageLen(){//计算字符串产生的绘制图的宽度		
		string messgae = msg.text.ToString();
		if(textImageLen>0 && messgae.Equals(oldMsgText)){
			//LogView.setViewText("ScrollMsg.cs,calucateMsgImageLen (),text NotChanged");
			return textImageLen;
		}
		//文本变化时才需要重新计算
		textImageLen = 0;
		Font curFont = msg.font;
		curFont.RequestCharactersInTexture (messgae,msg.fontSize,msg.fontStyle);
		CharacterInfo cinfo= new CharacterInfo ();
		char[] arr = messgae.ToCharArray ();
		foreach (char c in arr) {
			curFont.GetCharacterInfo (c,out cinfo,msg.fontSize);
			textImageLen += cinfo.advance;
		}
		oldMsgText = msg.text.ToString ();

		initMsgBeginX ();
		return textImageLen;
	}

	public float getWidth(){//脚本所在控件的宽度
		RectTransform rectTransform = scrollBack.GetComponent<RectTransform>();
		float w = rectTransform.rect.width;
		return w;
	}
	public float getHeight(){//脚本所在控件的高度
		RectTransform rectTransform = scrollBack.GetComponent<RectTransform>();
		float h = rectTransform.rect.height;
		return h;
	}

	public float getX(){//脚本所在控件的x坐标,跟锚点有关，当前锚点是中心位置，所以得到的坐标是控件中心所处的坐标
		RectTransform rectTransform = scrollBack.GetComponent<RectTransform>();
		float x = rectTransform.position.x;
		return x;
	}
	public float getY(){//脚本所在控件的y坐标
		RectTransform rectTransform = scrollBack.GetComponent<RectTransform>();
		float y = rectTransform.position.y;
		return y;
	}

	void initMsgBeginX(){
		scrollCount++;
		if (scrollCount > needViewCount) {
			return;
		}
		float msgControlWidth = msg.GetComponent<RectTransform> ().rect.width;//text控件宽度
		beginx = getWidth()/2 - (msgControlWidth-textImageLen)/2;//使得开始时第一个字符从Image控件右侧进入
		curX = beginx;
		moveLen = 0.0f;

		LogView.setViewText("ScrollMsg.cs,initMsgBeginX (),beginx=="+beginx);
		LogView.setViewText("ScrollMsg.cs,initMsgBeginX (),scrollCount=="+scrollCount);
	}



}
