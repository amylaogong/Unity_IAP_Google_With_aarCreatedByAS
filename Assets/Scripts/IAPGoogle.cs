using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


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


	public int sku_detail_count = -1;//
	public string sku_detail_1;//id|price
	public string sku_detail_2;//
	public string sku_detail_3;//
	public string sku_detail_4;//
	public string sku_detail_5;//
	public string sku_detail_6;//

	public string purchase_verify_info;//独立请求返回的结果，googleOrder|verifyState
	public string purchase_isVailid;//伴随订单回调的结果

}

public enum IAP_PAY_STATE{
	PAY_STATE_CONSUME_SUCCESS = 0,//消费成功
	PAY_STATE_QUERRY_OWNEDSKU_SUCCESS = 1,
	PAY_STATE_QUERRY_SKU_DETAILS= 2,

	PAY_STATE_SERVICE_INIT_FAILED = 1998,//初始化失败
	PAY_STATE_SERVICE_READY = 1999,//初始化完毕，服务准备完毕
	PAY_STATE_PROCESS_PURCHASE_CANCELLED=2001,//用户取消
	PAY_STATE_PROCESS_PURCHASE=2000,//
	PAY_STATE_PROCESS_PURCHASE_DONE=2002,//购买完成
	PAY_STATE_PROCESS_CONSUME=3000,//消费有了回调，然后判断是否成功
	PAY_STATE_VERIFY_CONSUME= 3001 // verify data
}

public class IAPGoogle : MonoBehaviour {


	public GameObject initpay;
	public GameObject chargeBtn;
	public GameObject querryOwned;
	public GameObject btnConsumedOwnedItem;
	public GameObject btnQuerrySkuDetail;


	public Text dateText;
	private string productID = "";


	private string pubKey = "";
	private string priKey = "";

	// Use this for initialization
	void Start () {

		initpay = gameObject.transform.Find ("Button_InitPay").gameObject;
		chargeBtn = gameObject.transform.Find ("Button_Buy").gameObject;
		querryOwned = gameObject.transform.Find ("Button_QuerryOwned").gameObject;
		btnConsumedOwnedItem = gameObject.transform.Find ("ConsumedOwnedItem").gameObject;

		btnQuerrySkuDetail = gameObject.transform.Find ("QuerrySkuDetail").gameObject;


		LogView.setViewText ("IAPGoogle,initpay=="+initpay);
		LogView.setViewText ("IAPGoogle,querryOwned=="+querryOwned);
		LogView.setViewText ("IAPGoogle,chargeBtn=="+chargeBtn);


		StartCoroutine (setEvent());


		Dictionary<string,string> dic = RSAEncrypt.GenerateKeys ();


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

		while(UIEvent.Get (btnConsumedOwnedItem).OnClick==null){
			UIEvent.Get (btnConsumedOwnedItem).OnClick = ConsumedOwnedItem;
			yield return new WaitForSeconds (0.3f);
		}
		while(UIEvent.Get (btnQuerrySkuDetail).OnClick==null){
			UIEvent.Get (btnQuerrySkuDetail).OnClick = QuerrySkuDetail;
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

		DateTime dt = DateTime.Now; 
		String dateStr = dt.ToString ("yyyyMMdd_HHmmss_fff");
		string gameOrderID = "unity_"+dateStr;

		AndroidInterface.ChargeByProductID (productID,gameOrderID);

	}

	public void QuerySkuOnwed(UIEventObj obj)
	{
		RSAEncrypt.TestRSA ();

		LogView.setViewText ("IAPGoogle.cs,111QuerySkuOnwed,Unity call Java...QuerySkuOnwed");
		AndroidInterface.QuerySkuOnwed ();

	}


	public void ConsumedOwnedItem(UIEventObj obj)
	{
		AndroidInterface.ConsumedOwnedItem ();
	}

	public void QuerrySkuDetail(UIEventObj obj)
	{
		AndroidInterface.QuerrySkuDetail ();
	}


	public void CheckGoogleSign(){
		
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


		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.sku_detail_count=="+iapResult.sku_detail_count);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.purchase_verify_info=="+iapResult.purchase_verify_info);
		LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.purchase_isVailid=="+iapResult.purchase_isVailid);



		IAP_PAY_STATE st = (IAP_PAY_STATE)iapResult.code;
		switch(iapResult.function){
		case "OnCallBackQuerryOwnedSku":
			if(iapResult.ownedSkuSize >= 0){
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,getOwnedSku,isServiceReady==true");
			}
			break;
		case "OnServiceCallBack":
			//记录每个档位的价格：换算成当地的价格 eg:1US$ = 8HK$

			switch (st){
			case IAP_PAY_STATE.PAY_STATE_QUERRY_SKU_DETAILS:
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,PAY_STATE_QUERRY_SKU_DETAILS...");
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,OnServiceCallBack,iapResult.sku_detail_2=="+iapResult.sku_detail_2);
				break;
			case IAP_PAY_STATE.PAY_STATE_VERIFY_CONSUME:
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,PAY_STATE_VERIFY_CONSUME...");
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,iapResult.purchase_verify_info=="+iapResult.purchase_verify_info);
				break;
			default:
				LogView.setViewText ("IAPGoogle.cs,IAPCallback,OnServiceCallBack,iapResult.code has no deal whih state:"+iapResult.code);
				break;

			}
			break;

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

				String publicKeyGoogle = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAh+LmGYNVmHE2oQD/nLseF1n0if5evGkc7/K+fAFFTUHXKTcHpJrmexlJ+4rg2TUa5be0o21VTFipy8oBfCbrek0eIEf3vzf1LwEfunA9SRljmhoBZ41vv5IxVLl1opS7kM9vFcF3ov2PzbngP1lI9Iy/5QQXCGcVmP4ohnJMQvgCsgE0LhFlaGSPZ5hZi5vzg7hDO6wdpAg9pyYJTPc3oOyeGTPZUTgWsj8RAIQBegaSnmkOYFQvi5e17SsDiYgs3awgtWFQJEcMcko8P3BAGKuuolwDDKyMxtBqkHz+rYNeEHqApWa1DDfu5SLaYCva8qaiacCU4wteP9d19Pn7EwIDAQAB";
				bool isCheckSucc = RSAEncrypt.VerifySignature (iapResult.getOriginalJson,publicKeyGoogle,iapResult.getSignature);
				LogView.setViewText ("IAPGoogle,IAPCallback,isCheckSucc=="+isCheckSucc);


				AndroidInterface.VerifyPurchase (iapResult.googleOrderId,iapResult.getOriginalJson,iapResult.getSignature);


			}

			break;
		case "SetCache"://一些缓存信息，比如系统版本啥的
			LogView.setViewText ("IAPGoogle.cs,IAPCallback,SetCache,key=="+iapResult.key);
			LogView.setViewText ("IAPGoogle.cs,IAPCallback,SetCache,value=="+iapResult.value);

			break;

		}


	}
		

}
