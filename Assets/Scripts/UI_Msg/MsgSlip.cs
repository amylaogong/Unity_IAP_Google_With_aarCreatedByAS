using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//从四周划入屏幕，上下左右可以配置
using UnityEngine.UI;

public class MsgSlip : MonoBehaviour {

	public static int topMsgCount = 0;

	public float moveSpeed = 200.0f;//移动速度
	public float waitTime = 2.2f;//目标位置停留一段时间之后消失
	public int direction = 0;//上下左右->0,1,2,3


	private float msgUIWidth;
	private float msgUIHeight;
	Text text;
	Image image;
	string oldMsgText;
	static int msgIndex = 0;

	float startTime = 0;
	float beginX = 0;//开始时相对父容器的偏移位置
	float beginY = 0;
	float moveYSpeed = 0;
	float moveXSpeed = 0;

	float outFromScreen = 0;//初始位置在屏幕外离屏幕边界的距离

	GameObject msgControl = null;


	// Use this for initialization
	void Start () {
		msgIndex++;
		msgControl = gameObject.transform.Find ("MsgSlip").gameObject;
		image = msgControl.GetComponent<Image>();
		text = msgControl.transform.Find ("text").GetComponent<Text>();
		LogView.setViewText ("MsgSlip.cs,Start (),image.name=="+image.name);
		LogView.setViewText ("MsgSlip.cs,Start (),text.name=="+text.name);
		LogView.setViewText ("MsgSlip.cs,Start (),gameObject.name=="+gameObject.name);
		LogView.setViewText ("MsgSlip.cs,Start (),Screen.width=="+Screen.width);//绝对的屏幕像素值，硬件提供
		LogView.setViewText ("MsgSlip.cs,Start (),Screen.height=="+Screen.height);


		msgUIWidth = gameObject.GetComponent<RectTransform>().rect.width;//换算之后的控件逻辑宽高，计算大小用这个
		msgUIHeight = gameObject.GetComponent<RectTransform> ().rect.height;
		LogView.setViewText ("MsgSlip.cs,Start (),msgUIWidth=="+msgUIWidth);
		LogView.setViewText ("MsgSlip.cs,Start (),msgUIHeight=="+msgUIHeight);
//		float rootCanvasWidth = UIManager.getInstance().mainCanvas.GetComponent<RectTransform>().rect.width;
//		float rootCanvasHeight = UIManager.getInstance().mainCanvas.GetComponent<RectTransform> ().rect.height;
//		LogView.setViewText ("MsgSlip.cs,Start (),rootCanvasWidth=="+rootCanvasWidth);
//		LogView.setViewText ("MsgSlip.cs,Start (),rootCanvasHeight=="+rootCanvasHeight);

		float xx = outFromScreen + msgUIWidth / 2;
		float yy = outFromScreen + msgUIHeight / 2;
		beginX = direction<=1 ? 0:(direction==2?-1*xx:xx);//左右
		beginY = direction >1 ? 0:(direction==0?yy:-1*yy);//上下
		moveXSpeed = direction<=1 ? 0:(direction==2?moveSpeed:-1*moveSpeed);
		moveYSpeed = direction >1 ? 0:(direction==0?-1*moveSpeed:moveSpeed);//上下

		LogView.setViewText ("MsgSlip.cs,Start (),direction=="+direction);
		LogView.setViewText ("MsgSlip.cs,Start (),beginX=="+beginX);
		LogView.setViewText ("MsgSlip.cs,Start (),beginY=="+beginY);

		msgControl.transform.localPosition = new Vector3(beginX,beginY,0);

		startTime = Time.time;
	}

	// Update is called once per frame
	void Update () {
		moveX ();
		moveY ();
	}

	IEnumerator moveFinish(){
		//LogView.setViewText ("moveFinish,curTime=="+Time.time);

		yield return new WaitForSeconds (waitTime);
		topMsgCount--;
		Destroy(gameObject);
		yield return 0;
	}

	void moveY(){
		if(direction>1){//左右
			return;
		}
		float y = msgControl.transform.localPosition.y + moveYSpeed * Time.deltaTime;
//		LogView.setViewText ("MsgSlip.cs,moveY (),Time.deltaTime=="+Time.deltaTime);
//		LogView.setViewText ("MsgSlip.cs,moveY (),y=="+y);
//		LogView.setViewText ("MsgSlip.cs,moveY (),image.color.a=="+image.color.a);
		if(moveYSpeed>0 && y>-1*msgUIHeight/4){//up
			y = -1*msgUIHeight/4;
			StartCoroutine (moveFinish());
		}else if(moveYSpeed<0 && y < msgUIHeight/4){//down
			y = msgUIHeight/4;
			StartCoroutine (moveFinish());
		}
		Vector3 pos = new Vector3(beginX,y,0);
		msgControl.transform.localPosition = pos;
	}

	void moveX(){
		if(direction<=1){//上下
			return;
		}
		float x = msgControl.transform.localPosition.x + moveXSpeed * Time.deltaTime;

//		LogView.setViewText ("MsgSlip.cs,moveX (),x=="+x);
//		LogView.setViewText ("MsgSlip.cs,moveX (),image.color.a=="+image.color.a);
//		LogView.setViewText ("MsgSlip.cs,moveX (),moveXSpeed=="+moveXSpeed);
//		LogView.setViewText ("MsgSlip.cs,moveX (),msgUIWidth/4=="+msgUIWidth/4);

		if(moveXSpeed>0 && x > -1*msgUIWidth/4){//left
			x = -1*msgUIWidth/4;
			StartCoroutine (moveFinish());
		}else if(moveXSpeed<0 && x < msgUIWidth/4){//right
			x = msgUIWidth/4;
			StartCoroutine (moveFinish());
		}
		Vector3 pos = new Vector3(x,beginY,0);
		msgControl.transform.localPosition = pos;
	}


	public void SetContent(string c){//此时可能start函数还没有执行而获取不到控件，在此等待执行完start之后再设置内容
		StartCoroutine(SetContentTillStartDone(c));
	}

	IEnumerator SetContentTillStartDone(string c){
		while(msgControl==null){
			yield return new WaitForEndOfFrame ();

		}
		if (text == null) {
			text = msgControl.transform.Find ("text").GetComponent<Text>();
		}
		text.text = c;
		yield return new WaitForEndOfFrame ();
	}




}
