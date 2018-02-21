using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
	public Canvas startCanvas;
	public Canvas exitCanvas;
	public Button startGame;
	public Button endGame;
	public GameObject abc;
	private RectTransform mySprite;  

	// Use this for initialization
	void Start () {
		abc = GameObject.Find ("startGame");
		mySprite = gameObject.GetComponent<RectTransform> ();
		//a = GetComponent<Button> ();
	}
	
	// Update is called once per frame
	void Update () {
//		for (int i = 1; i <= 10; i++) {
//			Debug.Log (i);
//			Debug.Log (GetComponent<RectTransform> ().offsetMax);
//			Debug.Log (GetComponent<RectTransform> ().offsetMin );
//		}
		// 1.图片的Top设置为100（偏移的Max是负数）  
		//GetComponent<RectTransform> ().offsetMax = new Vector2 (GetComponent<RectTransform> ().offsetMax.x, -100);  

		// 2.图片的Bottom设置为120  
		//GetComponent<RectTransform> ().offsetMin = new Vector2 (GetComponent<RectTransform> ().offsetMin.x, 120);  

		// 3.改变RectTransform的宽和高（注：测试的时候锚点中不要选择带蓝色线的适配方式，那样会被拉伸的）  
		GetComponent<RectTransform> ().sizeDelta = new Vector2 (500, 20);  

		// 4.改变RectTransform的postion（x,y,z）  
		GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-300 ,-200); 
		//GetComponent<RectTransform> ().offsetMax = new Vector2(10,0); // top and right;
		//GetComponent<RectTransform> ().offsetMin = new Vector2(20,0); // left and bottom
		//GetComponent<RectTransform> ().anchorMin = new Vector2(0,0);
		//GetComponent<RectTransform> ().anchorMax = new Vector2(0,0);

	}
	public void goGame(){
		exitCanvas.enabled = false;
		startCanvas.enabled = true;
	}

	public void exitGame(){
		exitCanvas.enabled = true;
		startCanvas.enabled = false;
	}
}
