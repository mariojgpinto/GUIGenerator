using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class GUI_Animation : MonoBehaviour {
	#region VARIABLES
	public static GUI_Animation instance;
	public static float animationTime = .25f;
	#endregion

	#region SHOW_HIDE
	public static void ShowMenu(GameObject panel)
	{
		panel.SetActive(true);
		panel.GetComponent<RectTransform>().localPosition = Vector3.zero;
	}

	public static void HideMenu(GameObject panel)
	{
		panel.SetActive(false);
		panel.GetComponent<RectTransform>().localPosition = new Vector3(Screen.width,0,0);
	}
	#endregion

	#region SLIDE
	public static IEnumerator SlidePanel_Routine(GameObject panel, Vector3 initialPosition, Vector3 finalPosition, bool removeElement = false)
	{
		float progress = 0; 

		if(!removeElement){
			panel.SetActive(true);
		}

		RectTransform panelPos = panel.GetComponent<RectTransform>();

		Vector3 deltaPosition = finalPosition - initialPosition;
		Vector3 endPosition = finalPosition;

		while(progress < 1)
		{
			panelPos.localPosition = initialPosition + deltaPosition * progress;

			progress += Time.deltaTime/animationTime;
			yield return true;
		}

		panelPos.localPosition = endPosition;

		if(removeElement){
			panel.SetActive(false);
		}

		yield break;
	}

	public static void SlidePanel(GameObject panel, Vector3 initialPosition, Vector3 finalPosition, bool removeElement = false)
	{
		instance.SlidePanel_Instance(panel, initialPosition, finalPosition, removeElement);
	}

	public static void BringBackFromRight(GameObject panel)
	{
		instance.SlidePanel_Instance(panel, new Vector3(Screen.width,0,0), Vector3.zero);
	}

	public static void BringBackFromLeft(GameObject panel)
	{
		instance.SlidePanel_Instance(panel, new Vector3(-Screen.width,0,0), Vector3.zero);
	}

	public static void RemoveToRight(GameObject panel)
	{
		instance.SlidePanel_Instance(panel, Vector3.zero, new Vector3(Screen.width,0,0), true);
	}

	public static void RemoveToLeft(GameObject panel)
	{
		instance.SlidePanel_Instance(panel, Vector3.zero, new Vector3(-Screen.width,0,0), true);
	}

	public static void BringBackFromTop(GameObject panel)
	{
		instance.SlidePanel_Instance(panel, new Vector3(0,Screen.height,0), Vector3.zero);
	}

	public static void BringBackFromBottom(GameObject panel)
	{
		instance.SlidePanel_Instance(panel, new Vector3(0,-Screen.height,0), Vector3.zero);
	}

	public static void RemoveToTop(GameObject panel)
	{
		instance.SlidePanel_Instance(panel, Vector3.zero, new Vector3(0,Screen.height,0), true);
	}

	public static void RemoveToBottom(GameObject panel)
	{
		instance.SlidePanel_Instance(panel, Vector3.zero, new Vector3(0,-Screen.height,0), true);
	}

	void SlidePanel_Instance(GameObject panel, Vector3 initialPosition, Vector3 finalPosition, bool removeElement = false)
	{
		//StartCoroutine(SlidePanel_Routine(panel, Vector3.zero, new Vector3(0,-Screen.height,0), removeElement));
		StartCoroutine(SlidePanel_Routine(panel, initialPosition, finalPosition, removeElement));
	}

	#endregion

	#region UNITY_CALLBACKS
	void Awake()
	{
		GUI_Animation.instance = this;
	}
	#endregion
}
