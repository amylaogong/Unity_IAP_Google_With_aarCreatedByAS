using UnityEngine;
using System.Collections;
using System.IO;  
using System.Collections.Generic;



public class LogView : MonoBehaviour {

	public static bool isTestMode = true;//是否是调试模式，该模式下会有一些变量写死成特殊值，比如登录用户名，跳过下载服等等

	public static string viewText;
	public static StreamWriter swFileLog;  
	public static bool isShowLog = true;
	public static bool isViewLogOnScreen = false;
	public static bool isTestLogin = false;
	public static string logFileName = "aa_unity_log.txt";

	public static string logFilePath;
	public static int count = 0;
	public static ArrayList textList = new ArrayList();
	public static void setViewText(object text){

		ScrollLog.AddLog (text);
		return;

		if(isShowLog){
			Debug.Log (text);
		}

		count++;
		textList.Add (count+":"+text);
		WriteFileByLine (count+":"+"["+System.DateTime.Now+"]"+text);
		updateText ();
	}

	public static void updateText(){
		if(textList.Count > 15){
			textList.RemoveAt(0);
		}
		viewText = "currentLog,"+":";
		foreach(string each in textList){
			viewText = viewText + "\n" + each;
		}
	}

	public static void WriteFileByLine(string str_info)  
	{  
		if (swFileLog != null) {
			swFileLog.WriteLine (str_info);//以行为单位写入字符串
			swFileLog.Flush();
		} 
	}  

	public static void deleteFile(string fullpath){
		if (File.Exists (fullpath)) {  
			File.Delete (fullpath);//
			setViewText ("文件删除成功:" + fullpath);
		} else {
			setViewText ("删除失败,文件不存在:" + fullpath);
		}

	}

	public static void deleteDirectory(string fullpath){
		if (Directory.Exists (fullpath)) {  
			DirectoryInfo di = new DirectoryInfo(fullpath);
			di.Delete(true);
			setViewText ("目录删除成功:" + fullpath);
		} else {
			setViewText ("删除失败,目录不存在:" + fullpath);
		}

	}

	public static void deleteAllFileInDir(string dirPath,List<string> noDeleteFiles){

		if (dirPath [dirPath.Length - 1] != Path.DirectorySeparatorChar) {
			dirPath += Path.DirectorySeparatorChar;
		}

		try
		{
			// 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
			// 如果你指向Delete目标文件下面的文件而不包含目录请使用下面的方法
			string[] fileList = Directory.GetFiles(dirPath);
			// 遍历所有的文件和目录
			foreach(string file in fileList)
			{
				string curFileName = Path.GetFileName(file);

				LogView.setViewText("LogView.cs,deleteAllFileInDir(),curFileName=="+curFileName);

				if(noDeleteFiles.Contains(curFileName)){
					continue;
				}
				// 先当作目录处理如果存在这个目录就递归Delete该目录下面的文件
				if(Directory.Exists(file))
				{
					deleteDirectory(dirPath + curFileName);
				}
				// 否则直接Delete文件
				else
				{
					deleteFile(dirPath + curFileName);
				}
			}
			//删除文件夹
			setViewText ("目录删除成功:" + dirPath);
		}
		catch (IOException e)
		{
			LogView.setViewText ("目录删除Failed:dirPath==" + dirPath+",e.Message=="+e.Message);
		}


	}


	public static void updateLogState(){
		string logSwitchFile = Application.persistentDataPath + "//" + "user.prop";
		if (File.Exists (logSwitchFile)) {//log开关
			isShowLog = true;
		} else {
			isShowLog = false;
		}

		isShowLog = true;//for test

		string logScreenSwitchFile = Application.persistentDataPath + "//" + "screenlog.prop";
		if (File.Exists (logScreenSwitchFile)) {//log开关
			isViewLogOnScreen = true;
			isShowLog = true;
		} else {
			isViewLogOnScreen = false;
		}

		//isViewLogOnScreen = true;//for test

		string loginTestFile = Application.persistentDataPath + "//" + "loginTest.prop";
		if (File.Exists (loginTestFile)) {//开关
			isTestLogin = true;
		} else {
			isTestLogin = false;
		}



	}

	void OnGUI()
	{
		//setViewText("test..woshuinishuoshasya,helloworlkd,w shuopasddfjadhf;ashfsah;sahsah,你好啊"+count);
		if(isViewLogOnScreen){
			GUI.Label(new Rect(10,10,900,600),viewText);
		}
		//Debug.Log ("LogView.cs,this.viewText=="+this.viewText);
	}

	void Start()  
	{

		string file_path = Application.persistentDataPath;
		string file_name = logFileName;

		string fullpath = file_path + "//" + file_name;
		logFilePath = fullpath;

		Debug.Log ("LogView.cs,Start(),logFilePath=="+logFilePath);

		updateLogState();


		swFileLog = new StreamWriter(logFilePath,false);
		/*一个是文件名，另一个是布尔值。如果此值为false，则创建一个新文件，如果存在原文件，则覆盖。如果此值为true，则打开文件保留原来数据，如果找不到文件，则创建新文件。*/
		//= File.CreateText(fullpath);//创建一个用于写入 UTF-8 编码的文本 

		setViewText ("LogView.cs,Start(),swFileLog=="+swFileLog);

		WriteFileByLine ("LogView.cs,Start(),begin:");
	}  

}



