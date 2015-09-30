using UnityEngine;
using System.Collections;

public class MyController : MonoBehaviour {
	#region VARIABLES
	
	#endregion

	void ButtonEventReceived(object sender, ButtonPressedEventArgs e){
		Debug.Log("Event Received - ");
	}

	#region UNITY_CALLBACKS
	// Use this for initialization
	void Start () {
		GUI_Controller.GUI_HomeMenu1_ButtonPressed += ButtonEventReceived;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	#endregion
}