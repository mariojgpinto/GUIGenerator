using UnityEngine;
using System.Collections;

public class MyController : MonoBehaviour {
	#region VARIABLES
	
	#endregion

		void OnButton_HomeMenu1(object sender, ButtonPressedEventArgs e){
			switch(e.id){
				case 0 : //HOMEMENU1 - NEXT
					Debug.Log(e.id + " - HOMEMENU1 - BUTTON NEXT");
	
					break;
				case 1 : //HOMEMENU1 - PREVIOUS
					Debug.Log(e.id + " - HOMEMENU1 - BUTTON PREVIOUS");
	
					break;
				case 2 : //HOMEMENU1 - BACK
					Debug.Log(e.id + " - HOMEMENU1 - BUTTON BACK");
	
					break;
				default: break;
			}
		}
	
		void OnButton_HomeMenu2(object sender, ButtonPressedEventArgs e){
			switch(e.id){
				case 0 : //HOMEMENU2 - BACK
					Debug.Log(e.id + " - HOMEMENU2 - BUTTON BACK");
	
					break;
				default: break;
			}
		}
	
		void OnToggle_HomeMenu2(object sender, TogglePressedEventArgs e){
			switch(e.id){
				case 0 : //HOMEMENU2 - NEXT
					Debug.Log(e.id + " - HOMEMENU2 - TOGGLE NEXT (" + e.value + ")");
					
					if(e.value){
						
					} else{ 
						
					}
	
					break;
				case 1 : //HOMEMENU2 - PREVIOUS
					Debug.Log(e.id + " - HOMEMENU2 - TOGGLE PREVIOUS (" + e.value + ")");
					
					if(e.value){
						
					} else{ 
						
					}
	
					break;
				default: break;
			}
		}
	
		void AssignEvents(){
			GUI_MainController.GUI_HomeMenu1_ButtonPressed += OnButton_HomeMenu1;
			GUI_MainController.GUI_HomeMenu2_ButtonPressed += OnButton_HomeMenu2;
	
			GUI_MainController.GUI_HomeMenu2_TogglePressed += OnToggle_HomeMenu2;
		}

	#region UNITY_CALLBACKS
	// Use this for initialization
	void Start () {
//		GUI_MainController.GUI_HomeMenu1_ButtonPressed += ButtonEventReceived;
		AssignEvents();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	#endregion
}