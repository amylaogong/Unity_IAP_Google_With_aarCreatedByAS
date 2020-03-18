using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IAP_SKU_ID_JSON{
	public int google_sku_count = 3;//数量
	//预留6个档位吧，一般4个就够用了
	public string google_sku_index_1 = "item_charge_1";//
	public string google_sku_index_2 = "item_charge_2";//
	public string google_sku_index_3 = "item_charge_30";//
	public string google_sku_index_4;//
	public string google_sku_index_5;//
	public string google_sku_index_6;//
}


public class AndroidInterface : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private static AndroidJavaObject mainActivity;
	private static string googlePayKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAh+LmGYNVmHE2oQD/nLseF1n0if5evGkc7/K+fAFFTUHXKTcHpJrmexlJ+4rg2TUa5be0o21VTFipy8oBfCbrek0eIEf3vzf1LwEfunA9SRljmhoBZ41vv5IxVLl1opS7kM9vFcF3ov2PzbngP1lI9Iy/5QQXCGcVmP4ohnJMQvgCsgE0LhFlaGSPZ5hZi5vzg7hDO6wdpAg9pyYJTPc3oOyeGTPZUTgWsj8RAIQBegaSnmkOYFQvi5e17SsDiYgs3awgtWFQJEcMcko8P3BAGKuuolwDDKyMxtBqkHz+rYNeEHqApWa1DDfu5SLaYCva8qaiacCU4wteP9d19Pn7EwIDAQAB";
	private static int runMode = 0;// 0 正常；1测试,测试状态下会打开log
	private static string googleSingInKey = "328571750760-gfgle265k00ou5hqp1knegsjcc5desbf.apps.googleusercontent.com";


	public static void SetViewLog(string str)
	{
		LogView.setViewText ("SetViewLog,str=="+str);
	}


	#if UNITY_ANDROID


	public static void SetRunMode()
	{
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		mainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");

		if (runMode.Equals(0)) {
			runMode = 1;
		} else {
			runMode = 0;
		}
		LogView.setViewText ("AndroidInterface.cs,SetRunMode,Unity call Java...runMode=="+runMode);
		mainActivity.Call("SetRunMode",runMode);
	}


	public static void InitPay()
	{
		
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		mainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");

		IAP_SKU_ID_JSON skuObj = new IAP_SKU_ID_JSON ();
		string skuJson = JsonUtility.ToJson(skuObj);
		LogView.setViewText ("AndroidInterface.cs,InitPay,Unity call Java...InitPay,skuJson=="+skuJson);
		mainActivity.Call("InitPay",googlePayKey,skuJson);
	}


	public static void ChargeByProductID(string productID,string gameOrderId)
	{
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		mainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");

		LogView.setViewText ("AndroidInterface,ChargeWithProductID,Unity call Java...productID=="+productID);
		LogView.setViewText ("AndroidInterface,ChargeWithProductID,Unity call Java...gameOrderId=="+gameOrderId);
		mainActivity.Call("ChargeByProductID", productID,gameOrderId);

	}

	public static void QuerySkuOnwed()
	{
		LogView.setViewText ("AndroidInterface.cs,222QuerySkuOnwed,Unity call Java...QuerySkuOnwed");

		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		mainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");

		mainActivity.Call("QuerySkuOnwed");
	}


	public static void ConsumedOwnedItem()
	{
		LogView.setViewText ("AndroidInterface.cs,ConsumedOwnedItem,Unity call Java...ConsumedOwnedItem");

		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		mainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");

		mainActivity.Call("ConsumedOwnedItem");
	}

	public static void VerifyPurchase(string goolgeOrder,string purchaseData,string signature)
	{
		LogView.setViewText ("AndroidInterface.cs,VerifyPurchase,Unity call Java...VerifyPurchase");

		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		mainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");

		mainActivity.Call("VerifyPurchase",goolgeOrder,purchaseData,signature);
	}

	public static void QuerrySkuDetail()
	{
		LogView.setViewText ("AndroidInterface.cs,QuerrySkuDetail,Unity call Java...QuerrySkuDetail");

		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		mainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");

		IAP_SKU_ID_JSON skuObj = new IAP_SKU_ID_JSON ();
		string skuJson = JsonUtility.ToJson(skuObj);
		LogView.setViewText ("AndroidInterface.cs,QuerrySkuDetail,Unity call Java...QuerrySkuDetail,skuJson=="+skuJson);
		mainActivity.Call("QuerrySkuDetail",skuJson);

	}



	public static void DoLogin(string account,string passwd){
		LogView.setViewText ("AndroidInterface.cs,222DoLogin,Unity call Java...DoLogin");
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		mainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");

		mainActivity.Call("DoLogin",account,passwd);

	}

	public static void Test(){
		LogView.setViewText ("AndroidInterface.cs,Test...");
		AndroidJavaClass jc = new AndroidJavaClass("com.example.helloworld.MainActivity");
		jc.CallStatic("TestStaticCall","AndroidInterface.cs_com.example.helloworld.MainActivity");

		jc = new AndroidJavaClass("com.unity.callback.AndroidUnityInterface");
		jc.CallStatic("SetUnityCache","fromWhere","com.unity.callback.AndroidUnityInterface");

	}


	public static void SDKLogin(int mode)
	{
		LogView.setViewText ("AndroidInterface.cs,SDKLogin,Unity call Java...SDKLogin");

		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		mainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");

		mainActivity.Call("SDKLogin",mode,googleSingInKey);
	}

	public static void SDKLogout(int mode)
	{
		LogView.setViewText ("AndroidInterface.cs,SDKLogout,Unity call Java...SDKLogout");

		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		mainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");

		mainActivity.Call("SDKLogout",mode);
	}

	#endif

	//被安卓回调的方法。。。
	public void OnNotifyWithJson(string jsonStr)
	{
		//通过返回的function字段判定走相应的逻辑，
		//OnCallBackPayProcess,OnCallBackPaySuccess,OnCallBackQuerryOwnedSku,SetCache


		IAPGoogle.IAPCallback (jsonStr);
	}

}
