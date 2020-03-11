using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IAPCallBackResult{
	public string function;//回调方法
	public int code = -1;//状态
	public int ownedSkuSize = -1;//
	public int IabHelerCode;//
	public string resultMsg;//

	public string googleOrderId;//getOrderId
	public string getSku;//
	public string selfOrderId;//getDeveloperPayload

	public string getPurchaseTime;
	public string getItemType;
	public string getOriginalJson;
	public string getPackageName;
	public string getPurchaseState;
	public string getSignature;
	public string getToken;

	public string key;//
	public string value;//

	//预留6个档位吧，一般4个就够用了
	public string getSku_1;//
	public string getSku_2;//
	public string getSku_3;//
	public string getSku_4;//
	public string getSku_5;//
	public string getSku_6;//
}

public enum IAP_PAY_STATE{
	PAY_STATE_CONSUME_SUCCESS = 0,//消费成功
	PAY_STATE_SERVICE_INIT_FAILED = 1998,//初始化失败
	PAY_STATE_SERVICE_READY = 1999,//初始化完毕，服务准备完毕
	PAY_STATE_PROCESS_PURCHASE_CANCELLED=2001,//用户取消
	PAY_STATE_PROCESS_PURCHASE=2000,//
	PAY_STATE_PROCESS_PURCHASE_DONE=2002,//购买完成
	PAY_STATE_PROCESS_CONSUME=3000//消费有了回调，然后判断是否成功
}

public class IAPGoogle : MonoBehaviour {


	public GameObject initpay;
	public GameObject chargeBtn;
	public GameObject querryOwned;


	public Text dateText;
	private string productID = "";

	// Use this for initialization
	void Start () {

		initpay = gameObject.transform.Find ("Button_InitPay").gameObject;
		chargeBtn = gameObject.transform.Find ("Button_Buy").gameObject;
		querryOwned = gameObject.transform.Find ("Button_QuerryOwned").gameObject;


		LogView.setViewText ("IAPGoogle,initpay=="+initpay);
		LogView.setViewText ("IAPGoogle,querryOwned=="+querryOwned);
		LogView.setViewText ("IAPGoogle,chargeBtn=="+chargeBtn);


		StartCoroutine (setEvent());
	}

	// Update is called once per frame
	void Update () {

	}
	IEnumerator setEvent(){
		
		while(UIEvent.Get (initpay).OnClick==null){
			UIEvent.Get (initpay).OnClick = InitPay;
			yield return new WaitForSeconds (0.3f);
		}
		while(UIEvent.Get (querryOwned).OnClick==null){
			UIEvent.Get (querryOwned).OnClick = QuerySkuOnwed;
			yield return new WaitForSeconds (0.3f);
		}
		while(UIEvent.Get (chargeBtn).OnClick==null){
			UIEvent.Get (chargeBtn).OnClick = ChargeByProductID;
			yield return new WaitForSeconds (0.3f);
		}

	}


//	public void SetViewLog(string str)
//	{
//		//LogView.setViewText ("from studio,IAPGoogle,SetViewLog,str=="+str);
//		dateText.text = str;
//		AndroidInterface.SetViewLog (str);
//	}
//
//	public void SetRunMode(GameObject obj)
//	{
//		LogView.setViewText ("IAPGoogle.cs,SetRunMode,Unity call Java..");
//		AndroidInterface.SetRunMode ();
//	}


	public void InitPay(UIEventObj obj)
	{
		AndroidInterface.InitPay ();
	}


	public void ChargeByProductID(UIEventObj obj)
	{
		if (productID.Equals ("")) {
			productID = "item_charge_1";
		} else if (productID.Equals ("item_charge_1")) {
			productID = "item_charge_2";
		} else if (productID.Equals ("item_charge_2")) {
			productID = "item_charge_30";
		}
		LogView.setViewText ("IAPGoogle.cs,ChargeWithProductID,Unity call Java...productID=="+productID);
		AndroidInterface.ChargeByProductID (productID);

	}

	public void QuerySkuOnwed(UIEventObj obj)
	{
		LogView.setViewText ("IAPGoogle.cs,111QuerySkuOnwed,Unity call Java...QuerySkuOnwed");
		AndroidInterface.QuerySkuOnwed ();

	}


	public static void IAPCallback(string jsonStr){
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,jsonStr=="+jsonStr);

		IAPCallBackResult iapResult = JsonUtility.FromJson<IAPCallBackResult>(jsonStr);

		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.function=="+iapResult.function);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.code=="+iapResult.code);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.ownedSkuSize=="+iapResult.ownedSkuSize);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.IabHelerCode=="+iapResult.IabHelerCode);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.resultMsg=="+iapResult.resultMsg);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.googleOrderId=="+iapResult.googleOrderId);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.getSku=="+iapResult.getSku);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.selfOrderId=="+iapResult.selfOrderId);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.key=="+iapResult.key);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.value=="+iapResult.value);


		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.getPurchaseTime=="+iapResult.getPurchaseTime);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.getItemType=="+iapResult.getItemType);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.getOriginalJson=="+iapResult.getOriginalJson);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.getPackageName=="+iapResult.getPackageName);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.getPurchaseState=="+iapResult.getPurchaseState);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.getSignature=="+iapResult.getSignature);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.getToken=="+iapResult.getToken);




		IAP_PAY_STATE st = (IAP_PAY_STATE)iapResult.code;
		switch(iapResult.function){
		case "OnCallBackPayProcess":
			
			switch (st){
			case IAP_PAY_STATE.PAY_STATE_SERVICE_INIT_FAILED:
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,PayService init failed!!");
				break;
			case IAP_PAY_STATE.PAY_STATE_SERVICE_READY:
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,PayService is ready!!");
				break;
			case IAP_PAY_STATE.PAY_STATE_PROCESS_PURCHASE_CANCELLED:
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,PayFaild,User cancelled!!");
				break;
			case IAP_PAY_STATE.PAY_STATE_PROCESS_PURCHASE:
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,Paying,purchase over,begin check...");
				break;
			case IAP_PAY_STATE.PAY_STATE_PROCESS_PURCHASE_DONE:
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,Paying,purchase success,begin consume...");
				break;
			case IAP_PAY_STATE.PAY_STATE_PROCESS_CONSUME:
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,Paying,consume over,begin check...");
				break;
			default:
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.code has no deal whih state:"+iapResult.code);
				break;

			}
			break;
		case "OnCallBackPaySuccess":
			if(st == IAP_PAY_STATE.PAY_STATE_CONSUME_SUCCESS){
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,consume success,then give the reward item...");
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,consume success,iapResult_getSku=="+iapResult.getSku);

			}

			break;
		case "OnCallBackQuerryOwnedSku":
			if(iapResult.ownedSkuSize >= 0){
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,getOwnedSku,isServiceReady==true");
			}


			break;
		case "SetCache"://一些缓存信息，比如系统版本啥的
			LogView.setViewText ("IAPGoogle.cs,IAPCallback,SetCache,key=="+iapResult.key);
			LogView.setViewText ("IAPGoogle.cs,IAPCallback,SetCache,value=="+iapResult.value);

			break;

		}


	}
		

}
