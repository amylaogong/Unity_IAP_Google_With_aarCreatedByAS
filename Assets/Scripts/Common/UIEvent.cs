using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class EventIgnore : MonoBehaviour, ICanvasRaycastFilter
{
	public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
	{
		return false;
	}

	static public void AddIgnore (GameObject go)
	{
		EventIgnore listener = go.GetComponent<EventIgnore>();
		if (listener == null) {
			listener = go.AddComponent<EventIgnore> ();
		}
	}

}



public class UIEventObj{
	public GameObject gameObject;
	string param;
	PointerEventData eventData;

	public UIEventObj(GameObject obj, string str)
	{
		this.gameObject = obj;
		this.param = str;
		this.eventData = null;
	}

	public GameObject GetObj(){
		return gameObject;
	}
	public string GetParam(){
		return param;
	}
	public PointerEventData GetEventData(){
		return eventData;
	}
	public void SetParam(string str){
		param = str;
	}
	public void SetEventData(PointerEventData data){
		eventData = data;
	}

}

public class UIEvent : UnityEngine.EventSystems.EventTrigger {

	public delegate void VoidDelegate (UIEventObj eventObj);
	public VoidDelegate OnClick;
	public VoidDelegate OnDoubleClick;
	public VoidDelegate OnPressed;
	public VoidDelegate OnLongPressed;
	public VoidDelegate OnEnter;
	public VoidDelegate OnExit;
	public VoidDelegate OnReleased;
	public VoidDelegate OnSelectEvent;
	public VoidDelegate OnUpdateSelect;

	public VoidDelegate OnDragBegin;
	public VoidDelegate OnDragging;
	public VoidDelegate OnDragEnd;


	public UIEventObj eventObj = null;

	private bool isPointerDown = false;
	private bool longPressTriggered = false;
	private float timePressStarted = 0.0f;
	private float timeLongPressLimit = 0.3f;//按下持续该时间即认为长按

	private float timeDoubleClickStarted = 0.0f;
	private float timeDoubleClickLimit = 0.2f;//该时间内点击两次即认为双击
	private bool isFirstClicked = false;

	static public UIEvent Get (GameObject go)
	{
		UIEvent listener = go.GetComponent<UIEvent>();
		//LogView.setViewText("UIEvent.cs,Get()000,listener=="+listener);
		if (listener == null) {
			listener = go.AddComponent<UIEvent> ();
		}

		if (listener!=null) {
			listener.InitEventObj ();
		} 

		//LogView.setViewText("UIEvent.cs,Get()111,listener=="+listener);
		return listener;
	}


	void Update(){
		if (isPointerDown && !longPressTriggered)
		{//长按
			if (Time.time - timePressStarted > timeLongPressLimit)
			{
				OnPointerLongPressed ();
			}
		}

		if (isFirstClicked)
		{//单击和双击事件并存状态下的单击检测
			if (Time.time - timeDoubleClickStarted > timeDoubleClickLimit)
			{//超过指定时长没有产生双击事件即认为是单击
				OnPointerSingleClick ();
			}
		}
	}

	public void InitEventObj(){
		if (eventObj == null) {
			eventObj = new UIEventObj (gameObject, "");
		} 
	}

	public void SetEventParam(string param){
		if (eventObj == null) {
			eventObj = new UIEventObj (gameObject, param);
		} else {
			eventObj.SetParam (param);
		}
	}
	public void SetEventData(PointerEventData data){
		if (eventObj != null) {
			eventObj.SetEventData (data);
		}
	}

	public bool GetInteractable(){//获取可用性
		Button btn = gameObject.GetComponent<Button> ();
		if(btn!=null){
			if(!btn.interactable){
				//LogView.setViewText("UIEvent.cs,getInteractable(),btn.interactable=="+btn.interactable);
				return false;
			}
		}
		return true;
	}

	public void OnPointerLongPressed(){//长按
		if(OnLongPressed != null && GetInteractable()) {
			longPressTriggered = true;
			OnLongPressed(eventObj);
		}
	}
	public void OnPointerDoubleClick(){//双击
//		LogView.setViewText("UIEvent.cs,OnPointerDoubleClick(),begin...");
		if(OnDoubleClick != null && GetInteractable ()){
			OnDoubleClick(eventObj);
		}
	}
	public void OnPointerSingleClick(){//单击
//		LogView.setViewText("UIEvent.cs,OnPointerSingleClick(),begin...OnClick=="+OnClick);
		isFirstClicked = false;
		if (OnClick != null && GetInteractable ()) {
			OnClick(eventObj);
		}
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		//LogView.setViewText("UIEvent.cs,OnPointerClick(),begin...eventData.clickCount=="+eventData.clickCount);
		SetEventData (eventData);
		if(longPressTriggered){//如果长按事件存在就不触发点击事件了
			return;
		}
		if(OnDoubleClick==null){//双击事件不存在的时候就直接执行单击
			OnPointerSingleClick ();
			return;
		}
		//双击事件也存在的情况下，单击事件生效会延迟，时长是timeDoubleClickLimit
		if(eventData.clickCount==1){//first click
			timeDoubleClickStarted = Time.time;
			isFirstClicked = true;
		}
		if(eventData.clickCount==2){
			isFirstClicked = false;
			OnPointerDoubleClick ();
		}
	}
	public override void OnPointerDown (PointerEventData eventData){
		LogView.setViewText ("UIEvent.cs,OnPointerDown(),onDown,eventData=="+eventData);
		timePressStarted = Time.time;
		isPointerDown = true;
		longPressTriggered = false;
		SetEventData (eventData);
		if(OnPressed != null && GetInteractable()) OnPressed(eventObj);
	}


	public override void OnBeginDrag(PointerEventData eventData) {
//		Debug.Log("UIEvent.cs,OnBeginDrag(555).eventData=="+eventData);

		if (OnDragBegin != null && GetInteractable ()) {
			SetEventData (eventData);
			OnDragBegin (eventObj);
		}

	}

	public override void OnDrag(PointerEventData eventData) {
//		Debug.Log("UIEvent.cs,OnDrag(666).eventData=="+eventData);
		if (OnDragging != null && GetInteractable ()) {
			SetEventData (eventData);
			OnDragging (eventObj);
		}
	
	}
	public override void OnEndDrag(PointerEventData eventData) {
		//Debug.Log("UIEvent.cs,OnEndDrag(777).");

		if (OnDragEnd != null && GetInteractable ()) {
			SetEventData (eventData);
			OnDragEnd (eventObj);
		}
	}


	public override void OnPointerEnter (PointerEventData eventData){
		if(OnEnter != null && GetInteractable()) OnEnter(eventObj);
	}
	public override void OnPointerExit (PointerEventData eventData){
		if(OnExit != null && GetInteractable()) OnExit(eventObj);
	}
	public override void OnPointerUp (PointerEventData eventData){
		//LogView.setViewText("UIEvent.cs,OnPointerUp(),OnReleased,begin...eventObj=="+eventObj.gameObject.name);
		isPointerDown = false;
		if(OnReleased != null && GetInteractable()) OnReleased(eventObj);
	}
	public override void OnSelect (BaseEventData eventData){
		if(OnSelectEvent != null && GetInteractable()) OnSelectEvent(eventObj);
	}
	public override void OnUpdateSelected (BaseEventData eventData){
		if(OnUpdateSelect != null && GetInteractable()) OnUpdateSelect(eventObj);
	}

}
