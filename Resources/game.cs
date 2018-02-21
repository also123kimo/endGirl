using UnityEngine;
//using UnityEditor; 
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.IO;
using System.Threading;
using UnityEngine.UI;
using LitJson;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using UnityEngine.Events;

//internal sealed class CustomAssetImporter : AssetPostprocessor {
//	#region Methods
//	//-------------Pre Processors
//	// This event is raised every time an audio asset is imported
//	private void OnPreprocessAudio() {
//		//Get the reference to the assetImporter (From the AssetPostProcessor class) and unbox it to an AudioImporter (wich is inherited and extends the AssetImporter with AudioClip-specific utilities)
//		var importer = assetImporter as AudioImporter;
//		//if the variable is empty, "do nothing"
//		if (importer == null) return;
//
//		//create a temp variable that contains everything you want to apply to the imported AudioClip (possible changes: .compressionFormat, .conversionMode, .loadType, .quality, .sampleRateOverride, .sampleRateSetting)
//		AudioImporterSampleSettings sampleSettings = importer.defaultSampleSettings; 
//		sampleSettings.loadType = AudioClipLoadType.Streaming; //alternatives: .DecompressOnLoad, .Streaming
//		sampleSettings.compressionFormat = AudioCompressionFormat.Vorbis; //alternatives: .AAC, .ADPCM, .GDADPCM, .HEVAG, .MP3, .PCM, .VAG, .XMA
//		sampleSettings.quality = 1f; //ranging from 0 (0%) to 1 (100%), currently set to 1%, wich is the smallest value that can be set in the inspector | Probably only useful when the compression format is set to Vorbis
//		importer.defaultSampleSettings = sampleSettings; //applying the temp variable values to the default settings (most important step!)
//		//platform-specific alternative:
//		//importer.SetOverrideSampleSettings ("Android", sampleSettings); //platform options: "Webplayer", "Standalone", "iOS", "Android", "WebGL", "PS4", "PSP2", "XBoxOne", "Samsung TV"
//	}
//
//	//-------------Post Processors
//
//	// This event is called as soon as the audio asset is imported successfully
//	//private void OnPostprocessAudio(AudioClip import) {}
//	#endregion
//}
	
public class game : MonoBehaviour {
	GameObject newCanvas ;
	public Canvas startCanvas;
	Canvas c ;
	AudioSource musicObj;
	AudioSource soundObj;
	AudioClip musiClip;
	GameObject panel;
	GameObject music;
	GameObject sound;
	Image background;
	GameObject[] playerText;
	GameObject[] optionText;
	optionPosObj[] posObj;
	Sprite testImg;
	string path;
	string jsonString;
	string getData;
	JsonData jsonScene;
	JsonData jsonText;
	JsonData jsonOptionText;
	JsonData jsonPage;
	JsonData jsonTable;
	JsonData jsonTextCss;
	JsonData jsonImgCss;
	JsonData jsonName;
	JsonData jsonAnimate;
	string musicSet; string soundSet ;
	int optionCount;
	float xMax ; float yMax ;
	float xBegin ; float yBegin;
	float cWidth = 0; float cHeight= 0; float cLeft = 0; float cTop = 0; int fontSize;
	string key; string textAlign ; string cText; string folder; string url;
	float mVolume;
	float sVolume;
	int loadCount;
	string fontName;
	float scale;
	float tempWidth;
	string defaultScene="1";
	string sceneType="";
	string musicNow;
	float  musicNowVolume;

	void Start () {
		//PlayerPrefs.DeleteAll();
		tempWidth = 1280;
		scale = tempWidth / 960f;  //1102:620 ,  1156:650  1024:576  1280:720
		xBegin = 960f; yBegin = 540f;
		xMax = xBegin*scale; yMax = yBegin*scale;
		loadCount = 0;
		fontName = "NotoSansCJKtc-Medium";
		//fontName = "PingFang";
		StartCoroutine(jsonStringPath ("/scene.json"));
		StartCoroutine(jsonStringPath ("/textCh.json"));
		StartCoroutine(jsonStringPath ("/nameCh.json"));
		StartCoroutine(jsonStringPath ("/optionCh.json"));
		StartCoroutine(jsonStringPath ("/page.json"));
		StartCoroutine(jsonStringPath ("/dataTable.json"));
		StartCoroutine(jsonStringPath ("/textStyle.json"));
		StartCoroutine(jsonStringPath ("/imgStyle.json"));
		StartCoroutine(jsonStringPath ("/animate.json"));
		//Screen.SetResolution(Mathf.FloorToInt(xMax),Mathf.FloorToInt(yMax),false);  
	}

	void FixedUpdate () {
		//Debug.Log("FixedUpdate time :" + Time.deltaTime);
		float nWidth = Screen.width;
		if (nWidth != tempWidth) {
			scale = nWidth / 960f;  //1102:620 ,  1156:650  1024:576  1280:720
			xBegin = 960f;
			yBegin = 540f;
			xMax = xBegin * scale;
			yMax = yBegin * scale;
			//Screen.SetResolution (Mathf.FloorToInt (xMax), Mathf.FloorToInt (yMax), false);  
			tempWidth = nWidth;
			initialGame (jsonTable ["i"] ["now"].ToString());
		}
	}

	public void initialGame(string i){
		destroyByTag("main");
		sceneType = "";
		jsonTable ["i"] ["now"] = i;
		//defaultScene = i;
		newCanvas = new GameObject ("Canvas");
		newCanvas.tag = "main";
		c = newCanvas.AddComponent<Canvas> ();
		c.GetComponent<RectTransform> ().sizeDelta = new Vector2 (xMax, yMax);
		c.renderMode = RenderMode.ScreenSpaceOverlay;
		//c.renderMode = RenderMode.ScreenSpaceCamera;
		c.pixelPerfect = true;
		newCanvas.AddComponent<CanvasScaler> ();
		newCanvas.AddComponent<GraphicRaycaster> ();

		//newCanvas.transform.localScale = new Vector2(1.2f, 1.2f);

		if (jsonScene [i] ["nextSceneType"].ToString () == "condition") {
			condition (i);
		}
		else{
			createBg(i,xBegin,yBegin);
			createImgList (i);
			createMusic (i);
			createSound (i);
			if (((IDictionary)jsonPage[i]).Contains ("conditionState")) {
				stateBar ();
			}
			if (jsonPage [i] ["contextCs"].ToString ()!= "0") {
				JsonData contextCs = jsonTextCss [jsonPage [i] ["contextCs"].ToString ()];
				cWidth = float.Parse (contextCs ["width"].ToString ());
				cHeight = float.Parse (contextCs ["height"].ToString ());
				cLeft = float.Parse (contextCs ["left"].ToString ());
				cTop = float.Parse (contextCs ["top"].ToString ());
				fontSize = int.Parse (contextCs ["fontSize"].ToString ());
				textAlign = contextCs ["textAlign"].ToString ();
				string textSpeed = jsonPage [i] ["textSpeed"].ToString ();
				string color =contextCs ["color"].ToString ();
				cText = textFunc(i);
				createText (cWidth, cHeight, cLeft, cTop, fontSize, textAlign, cText ,color ,"contextCs",i,textSpeed);
				if ((IDictionary)jsonName [i] != null) {
					createText (150, 70, 415, 390, 22, "center", jsonName [i].ToString (),color);
				}	
			}
			patchData (i);
			if (jsonScene [i] ["nextSceneType"].ToString () == "option") {
				if(jsonPage [i] ["contextCs"].ToString ()== "0"){
					createOption (i);
				}
			}
			else {
				Button b = newCanvas.AddComponent<Button> ();
				b.onClick.AddListener (() => BtnFun (i));	
			}
		}

		GameObject setting=createImg (50f, 50f, 900f, 5f, "ui", "setting_ch");
		Button setB = setting.AddComponent<Button> ();
		setB.onClick.AddListener (() => {
			settingPopup();
		});
	}

	public void textAnimateCallBack(string i){
		if (jsonScene [i] ["nextSceneType"].ToString () == "option") {
			StartCoroutine(optionWait(i,0.2f));
		}
	}

	IEnumerator optionWait(string i, float sec)
	{
		yield return new WaitForSeconds(sec);
		createOption (i);
	}

	public IEnumerator jsonStringPath(string path){
		string filePath = Application.streamingAssetsPath + path;
		if (filePath.Contains ("://") || filePath.Contains (":///")) {
			var request = UnityWebRequest.Get(filePath);
			var download = new DownloadHandlerBuffer();
			request.downloadHandler = download;
			yield return request.Send();
			jsonString=download.text.ToString();
			jsonDataMapping (path, jsonString);
		} else {
			jsonString = File.ReadAllText (filePath);
			jsonDataMapping (path, jsonString);
		}
	}
		

	public void jsonDataMapping(string path,string jsonString){
		loadCount++;
		switch (path){
		case "/scene.json":
			jsonScene = new JsonData() ;
			jsonScene=JsonMapper.ToObject<JsonData> (jsonString);
			break;
		case "/textCh.json":
			jsonText = new JsonData() ;
			jsonText= JsonMapper.ToObject<JsonData> (jsonString);
			break;
		case "/nameCh.json":
			jsonName = new JsonData() ;
			jsonName= JsonMapper.ToObject<JsonData> (jsonString);
			break;
		case "/optionCh.json":
			jsonOptionText = new JsonData() ;
			jsonOptionText= JsonMapper.ToObject<JsonData> (jsonString);
			break;
		case "/page.json":
			jsonPage = new JsonData() ;
			jsonPage= JsonMapper.ToObject<JsonData> (jsonString);
			break;
		case "/dataTable.json":
			jsonTable = new JsonData() ;
			jsonTable= JsonMapper.ToObject<JsonData> (jsonString);
			break;
		case "/textStyle.json":
			jsonTextCss = new JsonData() ;
			jsonTextCss= JsonMapper.ToObject<JsonData> (jsonString);
			break;
		case "/imgStyle.json":
			jsonImgCss = new JsonData() ;
			jsonImgCss= JsonMapper.ToObject<JsonData> (jsonString);
			break;
		case "/animate.json":
			jsonAnimate = new JsonData() ;
			jsonAnimate= JsonMapper.ToObject<JsonData> (jsonString);
			break;
		}
		if(loadCount==9){
			initialGame (defaultScene);
			//initialGame ("638");
		}
	}

	public void createText(float cWidth,float cHeight,float cLeft,float cTop,int fontsize,string TextAlign,string cText,string color="255,255,255" ,string type="default",string i="0",string speed="0" ){
		cWidth = cWidth * scale;  cHeight = cHeight * scale;  
		cLeft = cLeft * scale;  cTop = cTop * scale;  
		fontsize = Mathf.FloorToInt(float.Parse (fontsize.ToString())* scale);
		float speedNum = 0;
		string[] sArray;
		sArray = color.Split(',');
		List<GameObject> playerText = new List<GameObject>();
		playerText.Add(new GameObject("Text"));
		playerText [0].tag = "text";
		RectTransform uiPos = playerText[0].AddComponent<RectTransform>();
		Text uiText = playerText[0].AddComponent<Text>();
		uiPos.sizeDelta = new Vector2 (cWidth, cHeight); 
		uiPos.anchoredPosition = new Vector2 (xPosConvert(cLeft,cWidth) ,yPosConvert(cTop,cHeight)); 
		uiText.supportRichText = true;
		uiText.fontSize = fontsize;
		uiText.font = (Font)Resources.Load(fontName) ;
		switch (TextAlign){
			case "left":
				uiText.alignment = TextAnchor.UpperLeft;
				break;
			case "center":
				uiText.alignment = TextAnchor.UpperCenter;
				break;
			case "absCenter":
				uiText.alignment = TextAnchor.MiddleCenter;
				break;
			case "right":
				uiText.alignment = TextAnchor.UpperRight;
				break;
		}
		uiText.horizontalOverflow = HorizontalWrapMode.Wrap;
		uiText.verticalOverflow = VerticalWrapMode.Truncate;
		uiText.color= new Color(int.Parse(sArray[0])/255.0f , int.Parse(sArray[1])/255.0f, int.Parse(sArray[2])/255.0f);
		uiText.lineSpacing = 1.2f;
		if (type == "contextCs") {
			if(speed=="0"){
				uiText.text = cText;
			}
			else{
				if(speed=="3"){
					speedNum = 0.3f;
				}
				else if(speed=="2"){
					speedNum = 0.5f;
				}
				else if(speed=="1"){
					speedNum = 0.7f;
				}

				uiText.DOText (cText, speedNum).OnComplete(()=>textAnimateCallBack(i));
			}
		} 
		else {
			uiText.text = cText;
		}
		playerText[0].transform.SetParent (newCanvas.transform, false);
	}

	public void createImgList(string i){
		for (int j = 1; j <= 10; j++) {
			if (!((IDictionary)jsonPage[i]).Contains ("img"+j)) {
				break;
			} 
			if( jsonPage [i] ["img" + j].ToString ()=="0"){
				break;
			}
			string animateKey = "0";
			
			if (((IDictionary)jsonPage[i]).Contains ("img"+j+"Animate")) {
				animateKey = jsonPage [i] ["img"+j+"Animate"].ToString ();
			} 
			key = jsonPage [i] ["img" + j].ToString ();
			cWidth = float.Parse (jsonImgCss[key] ["width"].ToString ());
			cHeight= float.Parse (jsonImgCss[key] ["height"].ToString ());
			cLeft = float.Parse (jsonImgCss[key] ["left"].ToString ());
			cTop = float.Parse (jsonImgCss[key] ["top"].ToString ());
			folder = jsonImgCss[key]["folder"].ToString ();
			url = jsonImgCss [key] ["url"].ToString ();
			createImg (cWidth, cHeight, cLeft, cTop, folder, url, animateKey);	
		}
	}
		
	public GameObject createImg(float cWidth,float cHeight,float cLeft,float cTop, string folder,string url,string animateKey="0",string tag="img"){
		cWidth = cWidth * scale;  cHeight = cHeight * scale;  
		cLeft = cLeft * scale;  cTop = cTop * scale; 
		RectTransform rect;
		panel = new GameObject ("Panel");
		panel.tag = tag;
		panel.AddComponent<CanvasRenderer> ();
		background = panel.AddComponent<Image> ();
		background.sprite =  Resources.Load<Sprite>(folder+"/"+url);	
		background.GetComponent<RectTransform> ().sizeDelta = new Vector2 (cWidth, cHeight); 
		background.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPosConvert(cLeft,cWidth) ,yPosConvert(cTop,cHeight)); 
		panel.transform.SetParent (newCanvas.transform, false);

		rect = background.GetComponent<RectTransform> ();
		if (animateKey != "0") {
			
			if (((IDictionary)jsonAnimate[animateKey]).Contains ("scaleBegin")) {
				float scaleBegin =  float.Parse (jsonAnimate [animateKey] ["scaleBegin"].ToString ());
				float scaleEnd =  float.Parse (jsonAnimate [animateKey] ["scaleEnd"].ToString ());
				float scaleTime =  float.Parse (jsonAnimate [animateKey] ["scaleTime"].ToString ());
				rect.DOScale(scaleBegin,0); 
				rect.DOScale(scaleEnd,scaleTime); 
			} 
			if (((IDictionary)jsonAnimate[animateKey]).Contains ("showTime")) {
				float showTime =  float.Parse (jsonAnimate [animateKey] ["showTime"].ToString ());
				background.DOFade(0,0);
				background.DOFade(1,showTime);
			} 
			if (((IDictionary)jsonAnimate[animateKey]).Contains ("hideTime")) {
				float hideTime =  float.Parse (jsonAnimate [animateKey] ["hideTime"].ToString ());
				background.DOFade(0,hideTime);
			} 
			if (((IDictionary)jsonAnimate[animateKey]).Contains ("shakePower")) {
				float shakePower =  float.Parse (jsonAnimate [animateKey] ["shakePower"].ToString ());
				float shakeTime =  float.Parse (jsonAnimate [animateKey] ["shakeTime"].ToString ());
				rect.DOShakePosition(shakeTime,new Vector2(shakePower,0),100,80);
			}
			Sequence mySequence = DOTween.Sequence();
			for (int h = 1; h <= 10; h++) {
				if (((IDictionary)jsonAnimate[animateKey]).Contains ("move"+h+"Time")) {
					float moveTime =  float.Parse (jsonAnimate [animateKey] ["move"+h+"Time"].ToString ());
					float moveX = float.Parse (jsonAnimate [animateKey] ["move" + h + "X"].ToString ()) * scale;
					float moveY =  float.Parse (jsonAnimate [animateKey] ["move"+h+"Y"].ToString ()) * scale;
					mySequence.Append(rect.DOLocalMove(new Vector2(xPosConvert(moveX,cWidth),yPosConvert(moveY,cHeight)), moveTime));
				} 
			}
			if (((IDictionary)jsonAnimate[animateKey]).Contains ("rotate")) {
				float rotate =  float.Parse (jsonAnimate [animateKey] ["rotate"].ToString ());
				float rotateTime =  float.Parse (jsonAnimate [animateKey] ["rotateTime"].ToString ());
				rect.DORotate(new Vector3(0,0,rotate), rotateTime);
			} 
		}
		return panel;

	}
	public void createBg(string i,float x,float y){
		string animateKey = "0";
		if (((IDictionary)jsonPage[i]).Contains ("bgAnimate")) {
			animateKey = jsonPage [i] ["bgAnimate"].ToString ();
		} 
		createImg(x,y,0,0, "Background", jsonPage [i] ["background"].ToString () ,animateKey,"background");
	}

	public void createMusic(string i){
		musicSet = jsonScene [i] ["music"].ToString();
		mVolume = float.Parse (jsonScene [i] ["mVolume"].ToString())/10;
		if(musicSet!="0"){
			playMusic (musicSet, mVolume);
		}
	}

	public void playMusic(string musicSet,float mVolume=10){
		destroyByTag("music");
		music = new GameObject ("AudioSource");
		music.tag = "music";
		musicObj=music.AddComponent<AudioSource> ();
		AudioClip musiClip= Resources.Load<AudioClip>("music/"+musicSet);
		musicObj.clip = musiClip;
		musicObj.loop = true;
		musicObj.volume = mVolume;
		musicObj.Play ();
		musicNow=musicSet;
		musicNowVolume=mVolume;
	}

	public void createSound(string i){
		soundSet = jsonScene [i] ["sound"].ToString();
		sVolume = float.Parse (jsonScene [i] ["sVolume"].ToString())/10;
		if(soundSet!="0"){
			playSound (soundSet,sVolume);
		}
	}

	public void playSound(string soundSet,float sVolume=10){
		sound = new GameObject ("AudioSource");
		soundObj=sound.AddComponent<AudioSource> ();
		AudioClip soundClip= Resources.Load<AudioClip>("sound/"+soundSet);
		soundObj.clip = soundClip;
		soundObj.volume = sVolume;
		soundObj.loop = false;
		soundObj.Play ();
	}

	public void settingPopup(){
		playSound ("click");
		destroyByTag("text");
		destroyByTag("img");
		createOption ("setting");
		sceneType = "popup";
	}

	public void stateBar(){
		int top = 5; //3
		int barHeight = 40;
		int fontSize = 18;  //20
		int fixNumLeft = 12;
		int fixTextLeft = 8;
		int foodDaily = int.Parse (jsonTable ["foodDaily"] ["now"].ToString ());
		createImg (640, barHeight, 0, 0, "UI", "stateBar");	
		int leftDay = 10 - int.Parse (jsonTable ["dayTimes"] ["now"].ToString ());
		createText (50, barHeight, 20, top, fontSize, "left", "體力:");
		createText (30, barHeight, 60+(fixNumLeft*1), top, fontSize, "left", jsonTable["tp"]["now"].ToString());
		createText (50, barHeight, 95+(fixTextLeft*1), top, fontSize, "left", "糧食:");
		string FColor = "255,255,255";
		if(int.Parse (jsonTable ["food"] ["now"].ToString ()) < foodDaily){
			FColor = "255,246,0";
		}
		createText (30, barHeight, 135+(fixNumLeft*2), top, fontSize, "left", jsonTable["food"]["now"].ToString(),FColor);
		createText (50, barHeight, 175+(fixTextLeft*2), top, fontSize, "left", "建材:");
		createText (30, barHeight, 210+(fixNumLeft*3), top, fontSize, "left", jsonTable["material"]["now"].ToString());
		createText (50, barHeight, 255+(fixTextLeft*3), top, fontSize, "left", "手槍:");
		createText (30, barHeight, 285+(fixNumLeft*4), top, fontSize, "left", jsonTable["gun"]["now"].ToString());
		createText (80, barHeight, 320+(fixTextLeft*5), top, fontSize, "left", "崩潰值:");	
		int num = int.Parse (jsonTable ["disappear"] ["now"].ToString ());
		string DColor = "255,255,255";
		if(num>=5){
			DColor = "255,24,0";
		}
		else if(num>=2 && num<=4){
			DColor = "255,246,0";
		}

		string leftDayText = "?";
		int i = int.Parse (jsonTable ["i"] ["now"].ToString ());
		if(i>=102){
			leftDayText = leftDay.ToString ();
		}
		else if(i>=73){
			leftDayText ="1";
		}
		createText (30, barHeight, 375+(fixNumLeft*5), top, fontSize, "left", jsonTable["disappear"]["now"].ToString(),DColor);
		createText (110, barHeight, 410+(fixTextLeft*7), top, fontSize, "left", "剩餘救援天數:");
		createText (30, barHeight, 515+(fixNumLeft*5), top, fontSize, "left", leftDayText);
		createText (110, barHeight, 540+(fixTextLeft*7), top, fontSize, "left", "天");
	}

	public void condition(string i){
		string cState=  "false";
		string[] sArray;
		string[] dataArray;
		string conditionScene = "";
		for (int j = 1; j <= 10; j++) {
			if (!((IDictionary)jsonScene[i]).Contains ("c"+j)) {
				break;
			} 
			string c=  jsonScene [i]["c"+j].ToString();
			conditionScene=  jsonScene [i]["cScene"+j].ToString();
			sArray = c.Split('&');
			string valueState = "true";
			for (int k = 0; k < sArray.Length; k++) {
				string cData = sArray [k];
				if (cData.LastIndexOf (">") >= 0) {
					if (cData.LastIndexOf (">=") >= 0) {
						dataArray = cData.Split(new string[] {">="}, StringSplitOptions.None);
						float userValue = float.Parse (jsonTable [tableValue (dataArray[0])]["now"].ToString());
						float cValue = newValueFunc(dataArray[1]);
						Debug.Log (dataArray [0] + ">=/" + userValue + "/" + cValue);
						if (userValue >= cValue) {
						} else {
							valueState = "false";
						}
					} else {
						dataArray = cData.Split(new string[] {">"}, StringSplitOptions.None);
						float userValue = float.Parse (jsonTable [tableValue (dataArray[0])]["now"].ToString());
						float cValue = newValueFunc(dataArray[1]);
						Debug.Log (dataArray [0] + ">/" + userValue + "/" + cValue);
						if (userValue > cValue) {
						} else {
							valueState = "false";
						}
					}
				} 
				else if (c.LastIndexOf ("<") >= 0) {
					if (c.LastIndexOf ("<=") >= 0) {
						dataArray = cData.Split(new string[] {"<="}, StringSplitOptions.None);
						float userValue = float.Parse (jsonTable [tableValue (dataArray[0])]["now"].ToString());
						float cValue = newValueFunc(dataArray[1]);
						if (userValue <= cValue) {
						} else {
							valueState = "false";
						}
					} else {
						dataArray = cData.Split(new string[] {"<"}, StringSplitOptions.None);
						float userValue = float.Parse (jsonTable [tableValue (dataArray[0])]["now"].ToString());
						float cValue = newValueFunc(dataArray[1]);
						if (userValue < cValue) {
						} else {
							valueState = "false";
						}
					}
				} 
				else if (c.LastIndexOf ("=") >= 0) {
					dataArray = cData.Split(new string[] {"="}, StringSplitOptions.None);
					float userValue = float.Parse (jsonTable [tableValue (dataArray[0])]["now"].ToString());
					float cValue = newValueFunc(dataArray[1]);
					if (userValue == cValue) {
					} else {
						valueState = "false";
					}
				}
				else if (c.LastIndexOf ("!=") >= 0) {
					dataArray = cData.Split(new string[] {"!="}, StringSplitOptions.None);
					float userValue = float.Parse (jsonTable [tableValue (dataArray[0])]["now"].ToString());
					float cValue = newValueFunc(dataArray[1]);
					if (userValue != cValue) {
					} else {
						valueState = "false";
					}
				} 
			}
			if (valueState == "true") {
				cState = "true";
				break;
			}
		}
		Debug.Log (cState);
		if (cState == "false") {
			patchData (i);
			initialGame (jsonScene [i] ["else"].ToString ());
		} else {
			patchData (i);
			initialGame (conditionScene);
		}
	}

	public void patchData(string i){
		//Debug.Log ("patchData:"+i);
		float patchValue;
		float newValue;
		string[] sArray;
		string patchTable;

		for (int j = 1; j <= 10; j++) {
			if (!((IDictionary)jsonScene[i]).Contains ("patch"+j)) {
				break;
			} 
			string patch=  jsonScene [i]["patch"+j].ToString();
			patchTable = "";
			if (patch.LastIndexOf ("+") >= 0) {
				sArray = patch.Split('+');
				patchTable = tableValue (sArray [0]);
				patchValue = float.Parse (jsonTable [patchTable]["now"].ToString());
				newValue=newValueFunc (sArray [1]);
				jsonTable [patchTable] ["now"] = patchValue +  newValue;
			}
			if (patch.LastIndexOf ("-") >= 0) {
				sArray = patch.Split('-');
				patchTable = tableValue (sArray [0]);
				patchValue = float.Parse (jsonTable [patchTable]["now"].ToString());
				newValue=newValueFunc (sArray [1]);
				jsonTable [patchTable] ["now"] = patchValue -  newValue;
			}
			if (patch.LastIndexOf ("*") >= 0) {
				sArray = patch.Split('*');
				patchTable = tableValue (sArray [0]);
				patchValue = float.Parse (jsonTable [patchTable]["now"].ToString());
				newValue=newValueFunc (sArray [1]);
				jsonTable [patchTable] ["now"] = patchValue *  newValue;
			}
			if (patch.LastIndexOf ("/") >= 0) {
				sArray = patch.Split('/');
				patchTable = tableValue (sArray [0]);
				patchValue = float.Parse (jsonTable [patchTable]["now"].ToString());
				newValue=newValueFunc (sArray [1]);
				jsonTable [patchTable] ["now"] = patchValue /  newValue;
			}
			if (patch.LastIndexOf ("=") >= 0) {
				sArray = patch.Split('=');
				patchTable = tableValue (sArray [0]);
				newValue=newValueFunc (sArray [1]);
				jsonTable [patchTable] ["now"] = newValue;
			}

			float nowValue = float.Parse (jsonTable [patchTable]["now"].ToString());
			float maxValue = float.Parse (jsonTable [patchTable]["max"].ToString());
			float minValue = float.Parse (jsonTable [patchTable]["min"].ToString());

			if(nowValue > maxValue){
				jsonTable [patchTable] ["now"] = jsonTable [patchTable] ["max"];
			}
			if(nowValue < minValue){
				jsonTable [patchTable] ["now"] = jsonTable [patchTable] ["min"];
			}

			Debug.Log (patchTable+"/"+jsonTable [patchTable] ["now"]);
		}
	}

	public string tableValue(string table){
		return table.Substring(table.LastIndexOf("[")+1,table.LastIndexOf("]")-1);
	}

	public float newValueFunc(string data){;
		float newValue = 0;
		if (data.LastIndexOf ("rand") >= 0) {
			string radomData =data.Substring (data.LastIndexOf ("(") + 1, data.LastIndexOf (")") - 5);
			string[] sArray = radomData.Split(',');
			newValue = UnityEngine.Random.Range(int.Parse(sArray [0]),int.Parse(sArray [1])+1);
		}
		else if(data.LastIndexOf ("[") >= 0){
			string nwePatchTable = tableValue (data);
			newValue= float.Parse (jsonTable [nwePatchTable]["now"].ToString());
		}
		else{
			newValue = float.Parse(data);
		}
		return newValue;
	}

	public string textFunc(string i){;
		string text = jsonText [i].ToString ();
		if (text.LastIndexOf ("/[") >= 0) {
			string[] tArray = text.Split(new string[] {"/["}, StringSplitOptions.None);
			string table = tArray[1].Substring (tArray[1].LastIndexOf ("/[") + 1, tArray[1].LastIndexOf ("]"));
			string tableData = jsonTable [table] ["now"].ToString();
			string newText=text.Replace("/["+table+"]", tableData);
			text = newText;
		}
		return text;
	}

	public float xPosConvert(float x, float width){
		float newX = 0;

		if (x  > (xMax / 2)) {
			newX = x - (xMax / 2)+(width/2);
		} else {
			newX = ((xMax / 2)-x)*-1+(width/2);
		}
		return newX;
	}

	public float yPosConvert(float y, float height){
		float newY = 0;

		if (y  > (yMax / 2)) {
			newY = (y-(yMax/2))*-1-(height/2);
		} else {
			newY = (yMax / 2)-y-(height/2);
		}
		return newY;
	}

	public void createOption(string i){
		List<optionPosObj> posObj = new List<optionPosObj>();
		float xPos = 0;
		float yPos = 30;
		posObj.Add(new optionPosObj());
		posObj.Add(new optionPosObj());
		posObj.Add(new optionPosObj());
		posObj.Add(new optionPosObj());
		posObj.Add(new optionPosObj());
		optionCount = 0;
		if (jsonScene [i]["option1"].ToString()!="0") {
			optionCount++;
		}
		if (jsonScene [i]["option2"].ToString()!="0") {
			optionCount++;
		}
		if (jsonScene [i]["option3"].ToString()!="0") {
			optionCount++;
		}
		if (jsonScene [i]["option4"].ToString()!="0") {
			optionCount++;
		}
		if (jsonScene [i]["option5"].ToString()!="0") {
			optionCount++;
		}

		if (optionCount == 1) {
			posObj[0].x = xPos;  posObj[0].y = yPos;
		}
		if (optionCount == 2) {
			posObj[0].x =  xPos;  posObj[0].y = yPos+40;
			posObj[1].x =  xPos;  posObj[1].y = yPos-40;
		}
		if (optionCount == 3) {
			posObj[0].x = xPos;  posObj[0].y = yPos+80;
			posObj[1].x = xPos;  posObj[1].y = yPos;
			posObj[2].x = xPos;  posObj[2].y = yPos-80;
		}
		if (optionCount == 4) {
			posObj[0].x = xPos;  posObj[0].y = yPos+120;
			posObj[1].x = xPos;  posObj[1].y = yPos+40;
			posObj[2].x = xPos;  posObj[2].y = yPos-40;
			posObj[3].x = xPos;  posObj[3].y = yPos-120;
		}
		if (optionCount == 5) {
			posObj[0].x = xPos;  posObj[0].y = yPos+160;
			posObj[1].x = xPos;  posObj[1].y = yPos+80;
			posObj[2].x = xPos;  posObj[2].y = yPos;
			posObj[3].x = xPos;  posObj[3].y = yPos-80;
			posObj[4].x = xPos;  posObj[4].y = yPos-160;
		}
		List<GameObject> optionText = new List<GameObject>();
		for (int k = 0; k < optionCount; k++) {
			panel = new GameObject ("Panel");
			panel.tag = "img";
			panel.AddComponent<CanvasRenderer> ();
			background = panel.AddComponent<Image> ();
			background.sprite =  Resources.Load<Sprite>("UI/talkWin5");	
			background.GetComponent<RectTransform> ().sizeDelta = new Vector2 (480f*scale, 50*scale); 
			background.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (posObj[k].x*scale, posObj[k].y*scale); 
			panel.transform.SetParent (newCanvas.transform, false);	

			optionText.Add (new GameObject ("Text"));
			RectTransform uiPos = optionText [k].AddComponent<RectTransform> ();
			optionText [k].tag = "text";
			Text uiText = optionText [k].AddComponent<Text> ();
			uiPos.sizeDelta = new Vector2 (480*scale, 50*scale); 
			uiPos.anchoredPosition = new Vector2 (posObj[k].x*scale, posObj[k].y*scale); 
			uiText.supportRichText = true;
			uiText.fontSize =  Mathf.FloorToInt(24f* scale);
			uiText.font = (Font)Resources.Load(fontName);
			uiText.alignment = TextAnchor.MiddleCenter;
			uiText.horizontalOverflow = HorizontalWrapMode.Wrap;
			uiText.verticalOverflow = VerticalWrapMode.Truncate;
			uiText.color = Color.white;
			uiText.text = jsonOptionText[jsonScene [i]["option"+(k+1)].ToString ()].ToString ();
			optionText [k].transform.SetParent (newCanvas.transform, false);
			string t = jsonScene [i] ["option" + (k + 1) + "Scene"].ToString ();
			Button b = optionText[k].AddComponent<Button> ();
			b.onClick.AddListener (()=> optionBtnFun(t));	
		}
	}

	public void destroyByTag(string tag){
		GameObject[] tags = GameObject.FindGameObjectsWithTag (tag);
		foreach (GameObject obj in tags) {
			GameObject.Destroy (obj);
		}
    }

	public void BtnFun(string i){
		if (sceneType != "popup") {
			destroyByTag ("main");
			initialGame (jsonScene [i] ["nextSceneId"].ToString ());
		}
	}
	public void optionBtnFun(string i){
		playSound ("click");
		switch (i){
			case "saving":
				save_and_loadData ("saving");
				break;
			case "loading":
				save_and_loadData ("loading");
				break;
			case "cg":
				break;
			case "continue":
				initialGame (jsonTable ["i"] ["now"].ToString ());
				break;
			case "quit":
				break;
			default:
				initialGame (i);
				break;
		}
	}

	public void save_and_loadData(string type="saving"){
		string iTemp = "";
		string imgName = "";
		string timeText= "";
		destroyByTag("text");
		destroyByTag("img");
		createImg (930f, 460f, 15f, 40f, "UI", "backUi");
		createImg (250f, 170f, 50f, 80f, "UI", "redBorder");
		createImg (250f, 170f, 350f, 80f, "UI", "redBorder");
		createImg (250f, 170f, 650f, 80f, "UI", "redBorder");
		createImg (250f, 170f, 50f, 280f, "UI", "redBorder");
		createImg (250f, 170f, 350f, 280f, "UI", "redBorder");
		createImg (250f, 170f, 650f, 280f, "UI", "redBorder");
		string saving1=PlayerPrefs.GetString("s1");
		string saving2=PlayerPrefs.GetString("s2");
		string saving3=PlayerPrefs.GetString("s3");
		string saving4=PlayerPrefs.GetString("s4");
		string saving5=PlayerPrefs.GetString("s5");
		string saving6=PlayerPrefs.GetString("s6");
		string saveText = "";
		if(type=="saving"){
			saveText=jsonOptionText ["89"].ToString();
		}
		else{
			saveText=jsonOptionText ["90"].ToString();
		}
		if (saving1 == "") {
			createText (250, 170, 50, 80, 22, "absCenter", saveText);		
		} else {
			JsonData jsonTableTemp= JsonMapper.ToObject<JsonData> (saving1);
			iTemp = jsonTableTemp ["i"] ["now"].ToString();
			imgName = jsonPage [iTemp] ["background"].ToString();
			timeText= jsonTableTemp["time"]["now"].ToString();
			createImg (240f, 135f, 55f, 85f, "Background", imgName);
			createText (250, 30, 50, 220, 20, "center", timeText);		
		}
		if (saving2 == "") {
			createText (250, 170, 350, 80, 22, "absCenter", saveText);	
		} else {
			JsonData jsonTableTemp= JsonMapper.ToObject<JsonData> (saving2);
			iTemp = jsonTableTemp ["i"] ["now"].ToString();
			imgName = jsonPage [iTemp] ["background"].ToString();
			timeText= jsonTableTemp["time"]["now"].ToString();
			createImg (240f, 135f, 355f, 85f, "Background", imgName);
			createText (250, 30, 350, 220, 20, "center", timeText);	
		}
		if (saving3 == "") {
			createText (250, 170, 650, 80, 22, "absCenter", saveText);	
		} else {
			JsonData jsonTableTemp= JsonMapper.ToObject<JsonData> (saving3);
			iTemp = jsonTableTemp ["i"] ["now"].ToString();
			imgName = jsonPage [iTemp] ["background"].ToString();
			timeText= jsonTableTemp["time"]["now"].ToString();
			createImg (240f, 135f, 655f, 85f, "Background", imgName);
			createText (250, 30, 650, 220, 20, "center", timeText);	
		}
		if (saving4 == "") {
			createText (250, 170, 50, 280, 22, "absCenter", saveText);	
		} else {
			JsonData jsonTableTemp= JsonMapper.ToObject<JsonData> (saving4);
			iTemp = jsonTableTemp ["i"] ["now"].ToString();
			imgName = jsonPage [iTemp] ["background"].ToString();
			timeText= jsonTableTemp["time"]["now"].ToString();
			createImg (240f, 135f, 55f, 285f, "Background", imgName);
			createText (250, 30, 50, 420, 20, "center", timeText);	
		}
		if (saving5 == "") {
			createText (250, 170, 350, 280, 22, "absCenter", saveText);	
		} else {
			JsonData jsonTableTemp= JsonMapper.ToObject<JsonData> (saving5);
			iTemp = jsonTableTemp ["i"] ["now"].ToString();
			imgName = jsonPage [iTemp] ["background"].ToString();
			timeText= jsonTableTemp["time"]["now"].ToString();
			createImg (240f, 135f, 355f, 285f, "Background", imgName);
			createText (250, 30, 350, 420, 20, "center", timeText);	
		}
		if (saving6 == "") {
			createText (250, 170, 650, 280, 22, "absCenter", saveText);	
		} else {
			JsonData jsonTableTemp= JsonMapper.ToObject<JsonData> (saving6);
			iTemp = jsonTableTemp ["i"] ["now"].ToString();
			imgName = jsonPage [iTemp] ["background"].ToString();
			timeText= jsonTableTemp["time"]["now"].ToString();
			createImg (240f, 135f, 655f, 285f, "Background", imgName);
			createText (250, 30, 650, 420, 20, "center", timeText);	
		}

		GameObject redBorder1=createImg (250f, 170f, 50f, 80f, "UI", "transparent");
		GameObject redBorder2=createImg (250f, 170f, 350f, 80f, "UI", "transparent");
		GameObject redBorder3=createImg (250f, 170f, 650f, 80f, "UI", "transparent");
		GameObject redBorder4=createImg (250f, 170f, 50f, 280f, "UI", "transparent");
		GameObject redBorder5=createImg (250f, 170f, 350f, 280f, "UI", "transparent");
		GameObject redBorder6=createImg (250f, 170f, 650f, 280f, "UI", "transparent");

		Button redBorderBtn1 = redBorder1.AddComponent<Button> ();
		redBorderBtn1.onClick.AddListener (() => {
			save_and_loadBtnFunc("s1",saving1,type);
		});

		Button redBorderBtn2 = redBorder2.AddComponent<Button> ();
		redBorderBtn2.onClick.AddListener (() => {
			save_and_loadBtnFunc("s2",saving2,type);
		});
		Button redBorderBtn3 = redBorder3.AddComponent<Button> ();
		redBorderBtn3.onClick.AddListener (() => {
			save_and_loadBtnFunc("s3",saving3,type);
		});
		Button redBorderBtn4 = redBorder4.AddComponent<Button> ();
		redBorderBtn4.onClick.AddListener (() => {
			save_and_loadBtnFunc("s4",saving4,type);
		});
		Button redBorderBtn5 = redBorder5.AddComponent<Button> ();
		redBorderBtn5.onClick.AddListener (() => {
			save_and_loadBtnFunc("s5",saving5,type);
		});
		Button redBorderBtn6 = redBorder6.AddComponent<Button> ();
		redBorderBtn6.onClick.AddListener (() => {
			save_and_loadBtnFunc("s6",saving6,type);
		});

		GameObject close=createImg (45f, 45f, 910f, 30f, "UI", "close");
		Button setClose = close.AddComponent<Button> ();
		setClose.onClick.AddListener (() => {
			settingPopup();
		});
	}

	public void save_and_loadBtnFunc(string btn ,string saving , string type="saving" ){
		if(type=="saving"){
			jsonTable ["time"] ["now"] = System.DateTime.Now.ToString ();
			jsonTable ["musicNow"] = musicNow;
			jsonTable ["musicNowVolume"] = musicNowVolume;
			string tableTemp=JsonMapper.ToJson(jsonTable);
			PlayerPrefs.SetString(btn, tableTemp);
			initialGame (jsonTable ["i"] ["now"].ToString());
			//CaptureScreen (btn);
		}
		else{
			if (saving != ""){
				jsonTable= JsonMapper.ToObject<JsonData> (saving);
				string iTemp = jsonTable ["i"] ["now"].ToString();
				if(jsonScene[iTemp]["music"].ToString()=="0"){
					musicNow=jsonTable["musicNow"].ToString();
					musicNowVolume=float.Parse(jsonTable["musicNowVolume"].ToString());
					playMusic(musicNow,musicNowVolume);
				}
				initialGame (iTemp);
			}
		}
	}

//	public void CaptureScreen(string name)
//	{
//		ScreenCapture.CaptureScreenshot("Assets/Resources/screen/"+name+".jpg");
//		Debug.Log("CaptureScreen");
//	}
	[System.Serializable]
	public class optionPosObj{
		public float x;
		public float y;
	}
}