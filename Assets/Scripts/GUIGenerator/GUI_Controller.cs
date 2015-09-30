using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class ButtonPressedEventArgs : EventArgs {
	public int id { get; set; }
}

public class TogglePressedEventArgs : EventArgs {
	public int id { get; set; }
	public bool value { get; set; }
}

public class GUI_Controller : MonoBehaviour {
	#region VARIABLES
	public static GUI_Controller instance;

	public GameObject gui_HomeMenu1;
	public GameObject gui_HomeMenu2;

	public static event EventHandler<ButtonPressedEventArgs> GUI_HomeMenu1_ButtonPressed;

	public static event EventHandler<ButtonPressedEventArgs> GUI_HomeMenu2_ButtonPressed;
	public static event EventHandler<TogglePressedEventArgs> GUI_HomeMenu2_TogglePressed;

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

		GUI_HomeMenu2.imageSlideShow_buttons_back_Button.onClick.AddListener(() => OnButton_HomeMenu2(0));

		GUI_HomeMenu2.imageSlideShow_buttons_next_Toggle.onValueChanged.AddListener((bool value) => OnToggle_HomeMenu2(0, value));
		GUI_HomeMenu2.imageSlideShow_buttons_previous_Toggle.onValueChanged.AddListener((bool value) => OnToggle_HomeMenu2(1, value));

	}

	protected static void OnButtonPressed(int id, EventHandler<ButtonPressedEventArgs> handler){
		ButtonPressedEventArgs args = new ButtonPressedEventArgs();
		args.id = id;

		if(handler != null){
			handler(GUI_Controller.instance, args);
		}
	}

	protected static void OnTogglePressed(int id, bool value, EventHandler<TogglePressedEventArgs> handler){
		TogglePressedEventArgs args = new TogglePressedEventArgs();
		args.id = id;
		args.value = value;

		if(handler != null){
			handler(GUI_Controller.instance, args);
		}
	}
	#endregion

	#region CALLBACKS
	void OnButton_HomeMenu1(int id){
		switch(id){
			case 0 : //HOMEMENU1 - NEXT
				Debug.Log("HOMEMENU1 - BUTTON NEXT");

				OnButtonPressed(id,GUI_HomeMenu1_ButtonPressed);

				break;
			case 1 : //HOMEMENU1 - PREVIOUS
				Debug.Log("HOMEMENU1 - BUTTON PREVIOUS");

				OnButtonPressed(id,GUI_HomeMenu1_ButtonPressed);

				break;
			case 2 : //HOMEMENU1 - BACK
				Debug.Log("HOMEMENU1 - BUTTON BACK");

				OnButtonPressed(id,GUI_HomeMenu1_ButtonPressed);

				break;
			default: break;
		}
	}

	void OnButton_HomeMenu2(int id){
		switch(id){
			case 0 : //HOMEMENU2 - BACK
				Debug.Log("HOMEMENU2 - BUTTON BACK");

				OnButtonPressed(id,GUI_HomeMenu2_ButtonPressed);

				break;
			default: break;
		}
	}

	void OnToggle_HomeMenu2(int id, bool value){
		switch(id){
			case 0 : //HOMEMENU2 - NEXT
				Debug.Log("HOMEMENU2 - TOGGLE NEXT (" + value + ")");
				
				OnTogglePressed(id, value,GUI_HomeMenu2_TogglePressed);

				break;
			case 1 : //HOMEMENU2 - PREVIOUS
				Debug.Log("HOMEMENU2 - TOGGLE PREVIOUS (" + value + ")");
				
				OnTogglePressed(id, value,GUI_HomeMenu2_TogglePressed);

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
