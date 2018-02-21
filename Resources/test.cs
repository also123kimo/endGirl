
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class test : MonoBehaviour {
	GameObject newCanvas ;
	public Canvas startCanvas;
	Canvas c ;
	AudioSource musicObj;
	AudioSource soundObj;
	GameObject panel;
	GameObject music;
	GameObject sound;
	Image background;
	GameObject[] playerText;
	Sprite testImg;
	string path;
	string jsonString;
	Dictionary<int,object> list;

	void Start () {
		list=new Dictionary<int,object>();
		/*	
		newCanvas = new GameObject ("Canvas");
		c = newCanvas.AddComponent<Canvas> ();
		c.GetComponent<RectTransform> ().sizeDelta = new Vector2 (960, 540);
		c.renderMode = RenderMode.ScreenSpaceOverlay;
		newCanvas.AddComponent<CanvasScaler> ();
		newCanvas.AddComponent<GraphicRaycaster> ();


		panel = new GameObject ("Panel");
		panel.AddComponent<CanvasRenderer> ();
		i = panel.AddComponent<Image> ();
		i.sprite =  Resources.Load<Sprite>("a");
		//i.color = Color.red;
		i.GetComponent<RectTransform> ().sizeDelta = new Vector2 (400, 200); 
		i.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (300 ,-100); 


		Button b = panel.AddComponent<Button> ();
		b.onClick.AddListener (()=> BtnFun() );
		panel.transform.SetParent (newCanvas.transform, false);


	    playerText = new GameObject("Text");
		playerText.layer = 5;
		RectTransform uiPos = playerText.AddComponent<RectTransform>();
		Text uiText = playerText.AddComponent<Text>();
		uiPos.sizeDelta = new Vector2 (300, 100); 
		uiPos.anchoredPosition = new Vector2 (0 ,0); 
		uiText.supportRichText = true;
		uiText.fontSize = 14;
		uiText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		uiText.alignment = TextAnchor.MiddleCenter;
		uiText.horizontalOverflow = HorizontalWrapMode.Overflow;
		uiText.verticalOverflow = VerticalWrapMode.Overflow;
		uiText.color= Color.green;
		uiText.text = "WORK!!";
		playerText.transform.SetParent (newCanvas.transform, false);
*/

		//		path=Application.streamingAssetsPath+"/jsonTest.json";
		//		jsonString = File.ReadAllText (path);
		//		Debug.Log (jsonString);
		//		jsonData yamo = JsonUtility.FromJson<jsonData> (jsonString);
		//		Debug.Log (yamo);
		//		yamo.level = 101;
		//		string newYamo = JsonUtility.ToJson (yamo);
		//		Debug.Log ("2"+newYamo);

		path=Application.streamingAssetsPath+"/scene.json";
		jsonString = File.ReadAllText (path);
		//jsonString=PlayerPrefs.GetString("newYamo","DefaultValue");
		scene yamo = JsonUtility.FromJson<scene> (jsonString);
		//Debug.Log (JsonUtility.ToJson (yamo.data[0]));
		//		string t1=JsonUtility.ToJson (yamo); 
		foreach (sceneData f in yamo.data){
			list [f.id] = f;
		}
		initialGame (list[1]);
		yamo.data[0].text = "1234";

		//string newYamo = JsonUtility.ToJson (yamo);
		//PlayerPrefs.SetString("newYamo",newYamo);
		//Debug.Log ("2"+newYamo);
	}


	public void BtnFun(object para){
		//Debug.Log ("a");
		//c.enabled = false;
		//        Destroy (GameObject.Find ("Canvas"));
		//		Debug.Log (GameObject.Find ("Text"));
		//        initialGame (para);
		//Destroy (GameObject.Find("playerText"));
		//		refreshText (para);
		destroyByTag("s");
		//refreshText (para);
	}

	[System.Serializable]
	public class jsonData{
		public string name;
		public int level;
	}


	[System.Serializable]
	public class scene{
		public int id;
		public sceneData[] data;
	}

	[System.Serializable]
	public class sceneData{
		public int id;
		public int pageId ;
		public string music;
		public string sound;
		public string title;
		public string text;
		public string background;
		public int nextSceneId;
		public string nextSceneType;
		public nextSceneCondition[] nextSceneCondition;
		public sceneOptions[] sceneOptions;
		public valueControll[] valueControll;
	}

	[System.Serializable]
	public class nextSceneCondition{
		public int scendId;
		public string key;
		public string controll;
		public int value;
	}

	[System.Serializable]
	public class sceneOptions{
		public int sceneId;
		public string name;
	}

	[System.Serializable]
	public class valueControll{
		public string key;
		public string controll;
		public int valueMin;
		public int valueMax;
	}

	public void initialGame(object para){
		//Debug.Log (a);
		///sceneData f=a.text;
		//string newYamo = JsonUtility.ToJson (a);
		sceneData sceneData = JsonUtility.FromJson<sceneData> (JsonUtility.ToJson (para));
		//Debug.Log ("2"+newYamo);
		newCanvas = new GameObject ("Canvas");
		c = newCanvas.AddComponent<Canvas> ();
		c.GetComponent<RectTransform> ().sizeDelta = new Vector2 (960, 540);
		c.renderMode = RenderMode.ScreenSpaceOverlay;
		newCanvas.AddComponent<CanvasScaler> ();
		newCanvas.AddComponent<GraphicRaycaster> ();

		panel = new GameObject ("Panel");
		panel.tag = "s";
		panel.AddComponent<CanvasRenderer> ();
		background = panel.AddComponent<Image> ();
		background.sprite =  Resources.Load<Sprite>(sceneData.background);
		//i.color = Color.red;
		background.GetComponent<RectTransform> ().sizeDelta = new Vector2 (960, 540); 
		background.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0 ,0); 
		panel.transform.SetParent (newCanvas.transform, false);

		music = new GameObject ("AudioSource");
		music.tag = "s";
		musicObj=music.AddComponent<AudioSource> ();
		AudioClip musiClip= Resources.Load<AudioClip>("music/inside");
		musicObj.clip = musiClip;
		musicObj.loop = true;
		musicObj.Play ();

		sound = new GameObject ("AudioSource");
		soundObj=sound.AddComponent<AudioSource> ();
		AudioClip soundClip= Resources.Load<AudioClip>("sound/click");
		soundObj.clip = soundClip;
		soundObj.loop = false;
		soundObj.Play ();


		List<GameObject> playerText = new List<GameObject>();
		playerText.Add(new GameObject("Text"));
		playerText[0].layer = 10;
		playerText [0].tag = "s";
		RectTransform uiPos = playerText[0].AddComponent<RectTransform>();
		Text uiText = playerText[0].AddComponent<Text>();
		uiPos.sizeDelta = new Vector2 (300, 100); 
		uiPos.anchoredPosition = new Vector2 (0 ,0); 
		uiText.supportRichText = true;
		uiText.fontSize = 14;
		uiText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		uiText.alignment = TextAnchor.MiddleCenter;
		uiText.horizontalOverflow = HorizontalWrapMode.Overflow;
		uiText.verticalOverflow = VerticalWrapMode.Overflow;
		uiText.color= Color.green;
		uiText.text = sceneData.text;
		playerText[0].transform.SetParent (newCanvas.transform, false);

		Button b = playerText[0].AddComponent<Button> ();
		b.onClick.AddListener (()=> BtnFun(list [sceneData.nextSceneId]) );		
	}


	public void refreshText(object para){
		sceneData sceneData = JsonUtility.FromJson<sceneData> (JsonUtility.ToJson (para));
		List<GameObject> playerText = new List<GameObject>();
		playerText.Add(new GameObject("Text"));
		playerText[0].layer = 10;
		RectTransform uiPos = playerText[0].AddComponent<RectTransform>();
		Text uiText = playerText[0].AddComponent<Text>();
		uiPos.sizeDelta = new Vector2 (300, 100); 
		uiPos.anchoredPosition = new Vector2 (0 ,0); 
		uiText.supportRichText = true;
		uiText.fontSize = 14;
		uiText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		uiText.alignment = TextAnchor.MiddleCenter;
		uiText.horizontalOverflow = HorizontalWrapMode.Overflow;
		uiText.verticalOverflow = VerticalWrapMode.Overflow;
		uiText.color= Color.green;
		uiText.text = sceneData.text;
		playerText[0].transform.SetParent (newCanvas.transform, false);
	}

	public void destroyByTag(string tag){
		GameObject[] tags = GameObject.FindGameObjectsWithTag (tag);
		foreach (GameObject obj in tags) {
			GameObject.Destroy (obj);
		}
	}
}