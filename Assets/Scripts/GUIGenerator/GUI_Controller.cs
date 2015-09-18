using UnityEngine;
using System.Collections;
using System.IO;

public class GUI_Controller : MonoBehaviour {
	#region VARIABLES
	public static GUI_Controller instance;

	public GameObject gui_HomeMenu1;
	public GameObject gui_HomeMenu2;
	#endregion

	#region SETUP
	void FindGameObjects(){
		gui_HomeMenu1 = GameObject.Find("Panel_HomeMenu1");
		gui_HomeMenu2 = GameObject.Find("Panel_HomeMenu2");
	}

	void InitializeClasses(){
		gui_HomeMenu1.GetComponent<RectTransform>().localPosition = Vector3.zero;
		gui_HomeMenu2.GetComponent<RectTransform>().localPosition = Vector3.zero;

		GUI_HomeMenu1.UpdateValues(gui_HomeMenu1);
		GUI_HomeMenu2.UpdateValues(gui_HomeMenu2);
		gui_HomeMenu1.gameObject.SetActive(true);
		gui_HomeMenu2.gameObject.SetActive(false);
	}

	void InitializeListeners(){
		GUI_HomeMenu1.imageSlideShow_buttons_next_Button.onClick.AddListener(() => OnButton_HomeMenu1(0));
		GUI_HomeMenu1.imageSlideShow_buttons_previous_Button.onClick.AddListener(() => OnButton_HomeMenu1(1));
		GUI_HomeMenu1.imageSlideShow_buttons_back_Button.onClick.AddListener(() => OnButton_HomeMenu1(2));

		GUI_HomeMenu2.imageSlideShow_buttons_next_Button.onClick.AddListener(() => OnButton_HomeMenu2(0));
		GUI_HomeMenu2.imageSlideShow_buttons_previous_Button.onClick.AddListener(() => OnButton_HomeMenu2(1));
		GUI_HomeMenu2.imageSlideShow_buttons_back_Button.onClick.AddListener(() => OnButton_HomeMenu2(2));

	}
	#endregion

	#region CALLBACKS
	void OnButton_HomeMenu1(int id){
		switch(id){
			case 0 : //HOMEMENU1 - NEXT
				Debug.Log("HOMEMENU1 - BUTTON NEXT");

				break;
			case 1 : //HOMEMENU1 - PREVIOUS
				Debug.Log("HOMEMENU1 - BUTTON PREVIOUS");

				break;
			case 2 : //HOMEMENU1 - BACK
				Debug.Log("HOMEMENU1 - BUTTON BACK");

				break;
			default: break;
		}
	}

	void OnButton_HomeMenu2(int id){
		switch(id){
			case 0 : //HOMEMENU2 - NEXT
				Debug.Log("HOMEMENU2 - BUTTON NEXT");

				break;
			case 1 : //HOMEMENU2 - PREVIOUS
				Debug.Log("HOMEMENU2 - BUTTON PREVIOUS");

				break;
			case 2 : //HOMEMENU2 - BACK
				Debug.Log("HOMEMENU2 - BUTTON BACK");

				break;
			default: break;
		}
	}

	#endregion

	#region UNITY_CALLBACKS
	void Awake(){
		instance = this;

		FindGameObjects();

		InitializeClasses();
		InitializeListeners();
	}

//	void Update(){
//		
//		}

//	void OnGUI(){
//		
//		}
	#endregion
}
