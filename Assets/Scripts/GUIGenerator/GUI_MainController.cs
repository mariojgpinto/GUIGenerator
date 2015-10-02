using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class ButtonPressedEventArgs : EventArgs {
	public int id { get; set; }
	public string description { get; set; }
}

public class TogglePressedEventArgs : EventArgs {
	public int id { get; set; }
	public string description { get; set; }

	public bool value { get; set; }
}

public class GUI_MainController : MonoBehaviour {
	#region VARIABLES
	public static GUI_MainController instance;

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
		GUI_HomeMenu1.imageSlideShow_buttons_next_Button.onClick.AddListener(() => Event_OnButton_HomeMenu1(0));
		GUI_HomeMenu1.imageSlideShow_buttons_previous_Button.onClick.AddListener(() => Event_OnButton_HomeMenu1(1));
		GUI_HomeMenu1.imageSlideShow_buttons_back_Button.onClick.AddListener(() => Event_OnButton_HomeMenu1(2));

		GUI_HomeMenu2.imageSlideShow_buttons_back_Button.onClick.AddListener(() => Event_OnButton_HomeMenu2(0));

		GUI_HomeMenu2.imageSlideShow_buttons_next_Toggle.onValueChanged.AddListener((bool value) => Event_OnToggle_HomeMenu2(0, value));
		GUI_HomeMenu2.imageSlideShow_buttons_previous_Toggle.onValueChanged.AddListener((bool value) => Event_OnToggle_HomeMenu2(1, value));

	}

	protected static void OnButtonPressed(int id, string desc, EventHandler<ButtonPressedEventArgs> handler){
		ButtonPressedEventArgs args = new ButtonPressedEventArgs();
		args.id = id;
		args.description = desc;

		if(handler != null){
			handler(GUI_MainController.instance, args);
		}
	}

	protected static void OnTogglePressed(int id, string desc, bool value, EventHandler<TogglePressedEventArgs> handler){
		TogglePressedEventArgs args = new TogglePressedEventArgs();
		args.id = id;
		args.description = desc;

		args.value = value;

		if(handler != null){
			handler(GUI_MainController.instance, args);
		}
	}
	#endregion

	#region CALLBACKS
	void Event_OnButton_HomeMenu1(int id){
		switch(id){
			case 0 : //HOMEMENU1 - NEXT
				OnButtonPressed(id, "HOMEMENU1 - BUTTON NEXT",GUI_HomeMenu1_ButtonPressed);
				break;
			case 1 : //HOMEMENU1 - PREVIOUS
				OnButtonPressed(id, "HOMEMENU1 - BUTTON PREVIOUS",GUI_HomeMenu1_ButtonPressed);
				break;
			case 2 : //HOMEMENU1 - BACK
				OnButtonPressed(id, "HOMEMENU1 - BUTTON BACK",GUI_HomeMenu1_ButtonPressed);
				break;
			default: break;
		}
	}

	void Event_OnButton_HomeMenu2(int id){
		switch(id){
			case 0 : //HOMEMENU2 - BACK
				OnButtonPressed(id, "HOMEMENU2 - BUTTON BACK",GUI_HomeMenu2_ButtonPressed);
				break;
			default: break;
		}
	}

	void Event_OnToggle_HomeMenu2(int id, bool value){
		switch(id){
			case 0 : //HOMEMENU2 - NEXT
				OnTogglePressed(id, "HOMEMENU2 - TOGGLE NEXT", value,GUI_HomeMenu2_TogglePressed);
				break;
			case 1 : //HOMEMENU2 - PREVIOUS
				OnTogglePressed(id, "HOMEMENU2 - TOGGLE PREVIOUS", value,GUI_HomeMenu2_TogglePressed);
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

	#region COMMENTS
//	void OnButton_HomeMenu1(object sender, ButtonPressedEventArgs e){
//		switch(e.id){
//			case 0 : //HOMEMENU1 - NEXT
//				Debug.Log(e.id + " - HOMEMENU1 - BUTTON NEXT");

//				break;
//			case 1 : //HOMEMENU1 - PREVIOUS
//				Debug.Log(e.id + " - HOMEMENU1 - BUTTON PREVIOUS");

//				break;
//			case 2 : //HOMEMENU1 - BACK
//				Debug.Log(e.id + " - HOMEMENU1 - BUTTON BACK");

//				break;
//			default: break;
//		}
//	}

//	void OnButton_HomeMenu2(object sender, ButtonPressedEventArgs e){
//		switch(e.id){
//			case 0 : //HOMEMENU2 - BACK
//				Debug.Log(e.id + " - HOMEMENU2 - BUTTON BACK");

//				break;
//			default: break;
//		}
//	}

//	void OnToggle_HomeMenu2(object sender, TogglePressedEventArgs e){
//		switch(e.id){
//			case 0 : //HOMEMENU2 - NEXT
//				Debug.Log(e.id + " - HOMEMENU2 - TOGGLE NEXT (" + e.value + ")");
//				
//				if(e.value){
//					
//				} else{ 
//					
//				}

//				break;
//			case 1 : //HOMEMENU2 - PREVIOUS
//				Debug.Log(e.id + " - HOMEMENU2 - TOGGLE PREVIOUS (" + e.value + ")");
//				
//				if(e.value){
//					
//				} else{ 
//					
//				}

//				break;
//			default: break;
//		}
//	}

//	void AssignEvents(){
//		GUI_MainController.GUI_HomeMenu1_ButtonPressed += OnButton_HomeMenu1;
//		GUI_MainController.GUI_HomeMenu2_ButtonPressed += OnButton_HomeMenu2;

//		GUI_MainController.GUI_HomeMenu2_TogglePressed += OnToggle_HomeMenu2;
//	}
	#endregion
}
