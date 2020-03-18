using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Security.Cryptography;
using System.Text;
using System;


public sealed class RSAEncrypt{
	///
	/// RSA 非对称加密
	///
	#region GenerateKeys 生成密钥对
	///
	/// 生成密钥对 key:PublicKey PrivateKey
	///
	///

	public static string publicKey;
	public static string privateKey;


	public static Dictionary<string,string> GenerateKeys()
	{
		Dictionary<string,string> dic = new Dictionary<string,string>();
		RSACryptoServiceProvider objdsa = new RSACryptoServiceProvider();

		publicKey = objdsa.ToXmlString(false);//公钥
		privateKey = objdsa.ToXmlString(true);//私钥


		LogView.setViewText ("RSAEncrypt,publicKey=="+publicKey);
		LogView.setViewText ("RSAEncrypt,privateKey=="+privateKey);

		return dic;
	}
	#endregion

	#region GenerateSignature 生成签名
	///
	/// 生成签名
	///
	/// 原文
	/// 私钥
	/// 签名
	public static string GenerateSignature(string content, string privateKey)
	{
		try
		{
			RSACryptoServiceProvider oRSA3 = new RSACryptoServiceProvider();
			oRSA3.FromXmlString(privateKey);
			byte[] messagebytes = Encoding.UTF8.GetBytes(content);
			byte[] AOutput = oRSA3.SignData(messagebytes, "SHA1");
			return Convert.ToBase64String(AOutput);
		}
		catch {
			return string.Empty;
		}
	}
	#endregion

	#region VerifySignature 验证签名
	///
	/// 验证签名
	///
	/// 内容
	/// 公钥
	/// 签名
	/// 是否正确
	public static bool VerifySignature(string content, string publicKey, string signature)
	{
		try
		{
			string[] strSplit = signature.Split('-');
			byte[] SignedHash = Convert.FromBase64String(signature);

			RSACryptoServiceProvider oRSA4 = new RSACryptoServiceProvider();
			oRSA4.FromXmlString(publicKey);
			return oRSA4.VerifyData(Encoding.UTF8.GetBytes(content), "SHA1", SignedHash);
		}
		catch {
			return false;
		}
	}
	#endregion




	public static void TestRSA(){
		string content = "hello rsa!!";
		string sigature = GenerateSignature (content,privateKey);
		LogView.setViewText ("RSAEncrypt,TestRSA(),content=="+content);
		LogView.setViewText ("RSAEncrypt,TestRSA(),sigature=="+sigature);

		bool isCheckSucc = VerifySignature (content,publicKey,sigature);
		LogView.setViewText ("RSAEncrypt,TestRSA(),isCheckSucc=="+isCheckSucc);



		String getSignature = "LkLpNH0X0d1KxIe4VQv7cIhcBw4ppFqKh7nZqX5SYb0rdv6dNno7QcCijH5RMA+YTd1Iml20C3koLPomQtRz7ll4WiQD6uoO/wWe4arCxeDxCW5sRAgC0/2xxG21ttVRhFEaCOeya2am3hIwJY6O0n/Qu3NYjroCpBV18hPPZXV7NtOu5eh/QrPgHq2LqLh7KTkbaOpA9ruh8bu43hz/Q9+kxWZEb47ZVUHFtkbwtq\\/hfudmJvO9nAVdiirCjb5CaJUIOv/okMpiwdCuEty9lvhAeNWt1KU18EfxrfuGSJ7sKeSgt4uOAqnmyNUcWeoa6C51D+Q+4ubLTgg8u16rDw==";
		String getOriginalJson = "{\"orderId\":\"GPA.3361-8302-5980-00378\",\"packageName\":\"com.ourgame.test.charge\",\"productId\":\"item_charge_2\",\"purchaseTime\":1584068138014,\"purchaseState\":0,\"developerPayload\":\"unity_20200313_105520_991\",\"purchaseToken\":\"jelanmnbofndbodjcjbifmjj.AO-J1OyttVKWY-jp7gM08dB8IscR08FfxKaD8PPPbV0kHOXFdHnd0Y4oy2befRNktT3pSbe3Q36rbEDYNCKxHpI3WrQ_CCbSshMI9zSR-ODMilWCOSMV7BL2Jg1ZrGWGnKMxixOGcRwp\"}";
		String publicKeyGoogle = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAh+LmGYNVmHE2oQD/nLseF1n0if5evGkc7/K+fAFFTUHXKTcHpJrmexlJ+4rg2TUa5be0o21VTFipy8oBfCbrek0eIEf3vzf1LwEfunA9SRljmhoBZ41vv5IxVLl1opS7kM9vFcF3ov2PzbngP1lI9Iy/5QQXCGcVmP4ohnJMQvgCsgE0LhFlaGSPZ5hZi5vzg7hDO6wdpAg9pyYJTPc3oOyeGTPZUTgWsj8RAIQBegaSnmkOYFQvi5e17SsDiYgs3awgtWFQJEcMcko8P3BAGKuuolwDDKyMxtBqkHz+rYNeEHqApWa1DDfu5SLaYCva8qaiacCU4wteP9d19Pn7EwIDAQAB";

		isCheckSucc = VerifySignature (getOriginalJson,publicKeyGoogle,getSignature);
		LogView.setViewText ("RSAEncrypt,TestRSA(),22222,isCheckSucc=="+isCheckSucc);



	}


}
