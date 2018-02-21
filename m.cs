using UnityEngine;  
using System.Collections;  
using UnityEngine.UI;  

/// <summary>  
/// 脚本位置：UGUI的图片  
/// </summary>  
public class m : MonoBehaviour  
{  
	// 你的图片  
	private RectTransform mySprite;  

	void Start ()  
	{  
		mySprite = gameObject.GetComponent<RectTransform> ();  
	}  

	void Update ()  
	{  

		// 1.图片的Top设置为100（偏移的Max是负数）  
		GetComponent<RectTransform> ().offsetMax = new Vector2 (GetComponent<RectTransform> ().offsetMax.x, -100);  

		// 2.图片的Bottom设置为120  
		GetComponent<RectTransform> ().offsetMin = new Vector2 (GetComponent<RectTransform> ().offsetMin.x, 120);  

		// 3.改变RectTransform的宽和高（注：测试的时候锚点中不要选择带蓝色线的适配方式，那样会被拉伸的）  
		GetComponent<RectTransform> ().sizeDelta = new Vector2 (20, 50);  

		// 4.改变RectTransform的postion（x,y,z）  
		GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (20   ,40, 0);  

		// 5.改变锚点的位置  
		//GetComponent<RectTransform> ().anchoredPosition = new Vector2 (posx, posy);  

	}  
}  