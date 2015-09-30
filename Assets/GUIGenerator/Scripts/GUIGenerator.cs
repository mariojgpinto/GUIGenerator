using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[RequireComponent (typeof (Canvas))]
public class GUIGenerator : MonoBehaviour {
	#region VARIABLES
	string directory_mainScripts = "";
	string directory_GUIScripts = "";
	#endregion

	#region DIRECTORIES
	void CreatePaths(){
		directory_mainScripts = Application.dataPath + GUIGenerator_Macros.directory_mainScripts;
		directory_GUIScripts = directory_mainScripts + GUIGenerator_Macros.directory_GUIScripts;
	}

	void CreateDirectories(){
		Debug.Log ("directory_mainScripts: " + directory_mainScripts);
		if(!Directory.Exists(directory_mainScripts)){
			Directory.CreateDirectory(directory_mainScripts);
		}

		Debug.Log ("directory_GUIScripts: " + directory_GUIScripts);
		if(!Directory.Exists(directory_GUIScripts)){
			Directory.CreateDirectory(directory_GUIScripts);
		}
	}

	#endregion

	#region CANVAS_ANALYSIS
	void GetAllComponents(GameObject obj, ref List<GUIGenerator_Elem_Base> list, GUIGenerator_Elem_Base parent_elem = null){
		for(int i = 0 ; i < obj.transform.childCount  ; ++i){
			GUIGenerator_Elem_Base elem = new GUIGenerator_Elem_Base();
			elem.parent = parent_elem;

			elem.GenerateInfo(obj.transform.GetChild(i).gameObject);

			if(elem.parent == null){
				list.Add(elem);
			}
			else{
				elem.parent.children.Add(elem);
			}

			GetAllComponents(elem.obj, ref list, elem);
		}
	}
	#endregion

	#region FILES
	void CreateAll(){
		List<GUIGenerator_Elem_Base> list = new List<GUIGenerator_Elem_Base>();
		
		GetAllComponents(this.gameObject, ref list, null);
		
		//CHECK FOR REPEATED NAMES
		FindAndResolveRepeatedElements(list);

		//Create one file for each main panel
		for(int i = 0 ; i < list.Count ; ++i){
			CreateClassFile(list[i]);
		}

		//Create the Controller File
		CreateControllerFile(list);
		
		//Create the Animation File
		CreateAnimationFile();

		//Print Info
		string str = "";
		for(int i = 0 ; i < list.Count ; ++i){
			list[i].PrintElement(ref str, 0);
		}
		
		Debug.Log(str);
	}

	void FindAndResolveRepeatedElements(List<GUIGenerator_Elem_Base> list){
		for(int k = 0 ; k < list.Count ; ++k){
			List<GUIGenerator_Elem_Base> full_list = new List<GUIGenerator_Elem_Base>();

			if(list[k].children.Count > 0){
				GetAllElements(list[k].children, ref full_list);

				int ac = 1;

				for(int i = 0 ; i < full_list.Count ; ++i){
					GUIGenerator_Elem_Base elem1 = full_list[i];

					for(int j = 0 ; j < full_list.Count ; ++j){
						if(i != j){
							GUIGenerator_Elem_Base elem2 = full_list[j];

							if(elem1.variableName == elem2.variableName){
								elem1.obj.name += "_" + ac;
								elem1.name += "_" + ac;
								elem1.UpdateVariableNames();
								ac++;

								elem2.obj.name += "_" + ac;
								elem2.name += "_" + ac;
								elem2.UpdateVariableNames();
								ac++;
							}
						}
					}
				}
			}
		}
	}

	void GetAllElements(List<GUIGenerator_Elem_Base> main_list, ref List<GUIGenerator_Elem_Base> full_list){
		for(int i = 0 ; i < main_list.Count ; ++i){
			full_list.Add(main_list[i]);

			if(main_list[i].children.Count > 0){
				GetAllElements(main_list[i].children, ref full_list);
			}
		}
	}

	#region FILES_CLASSES
	void CreateClassFile(GUIGenerator_Elem_Base class_elem){
		class_elem.className = GUIGenerator_Macros.text_classPrefix + class_elem.name;
		class_elem.classInstanceName = GUIGenerator_Macros.text_classPrefix.ToLower() + class_elem.name;
		class_elem.classButtonListenerFunction = GUIGenerator_Macros.text_function_OnButtonCallback_Prefix.Replace(GUIGenerator_Macros.replacement_name,class_elem.name);
		class_elem.classToggleListenerFunction = GUIGenerator_Macros.text_function_OnToggleCallback_Prefix.Replace(GUIGenerator_Macros.replacement_name,class_elem.name);

		string classContent = "";
		classContent += GUIGenerator_Macros.text_includes;
		classContent += GUIGenerator_Macros.text_classDeclaration.Replace(GUIGenerator_Macros.replacement_name,class_elem.className);

		classContent += GUIGenerator_Macros.text_function_InitializeValues_Method_Comment.
			Replace(GUIGenerator_Macros.replacement_variable, class_elem.name.ToUpper());
		classContent += GUIGenerator_Macros.text_variableDeclaration.
			Replace(GUIGenerator_Macros.replacement_type, GUIGenerator_Macros.typeFormated_gameObject).
			Replace(GUIGenerator_Macros.replacement_name, class_elem.variableName_obj);
		
		if(class_elem.type == GUIGenerator_Elem_Base.TYPE.OTHER){
			
		} else{
			classContent += GUIGenerator_Macros.text_variableDeclaration.
				Replace(GUIGenerator_Macros.replacement_type, class_elem.classTypeFormated).
				Replace(GUIGenerator_Macros.replacement_name, class_elem.variableName);
		}
		classContent += "\n\n";


		for(int i = 0 ; i < class_elem.children.Count ; ++i){
			classContent += GUIGenerator_Macros.text_function_InitializeValues_Method_Comment.
				Replace(GUIGenerator_Macros.replacement_variable, class_elem.children[i].name.ToUpper());

			CreateVariablesNames(class_elem.children[i], ref classContent);

			classContent += "\n";
		}


		classContent += "\n\n";
		classContent += GUIGenerator_Macros.text_function_UpdateValues_Header;

		CreateFunctionUpdateValues(class_elem, ref classContent);
		classContent = classContent.Substring(0, classContent.Length-1);
		classContent += GUIGenerator_Macros.text_function_End;

		classContent += GUIGenerator_Macros.text_classEnd;

		File.WriteAllText(directory_GUIScripts + class_elem.className + GUIGenerator_Macros.file_end, classContent);
	}

	void CreateVariablesNames(GUIGenerator_Elem_Base elem, ref string str){
		str += GUIGenerator_Macros.text_variableDeclaration.Replace(GUIGenerator_Macros.replacement_type, GUIGenerator_Macros.typeFormated_gameObject).
															Replace(GUIGenerator_Macros.replacement_name, elem.variableName_obj);

		if(elem.type == GUIGenerator_Elem_Base.TYPE.OTHER){

		} else{
			str += GUIGenerator_Macros.text_variableDeclaration.Replace(GUIGenerator_Macros.replacement_type, elem.classTypeFormated).
																Replace(GUIGenerator_Macros.replacement_name, elem.variableName);
		}

		for(int i = 0 ; i < elem.children.Count ; ++i){
			str += "\n";
			CreateVariablesNames(elem.children[i], ref str);
		}
	}

	void CreateFunctionUpdateValues(GUIGenerator_Elem_Base class_elem, ref string str){
		string parent = "";

		if(class_elem.parent == null){
			parent = GUIGenerator_Macros.text_function_UpdateValues_ReplacementParent;

			str += GUIGenerator_Macros.text_function_UpdateValues_GetGameObjectParent.
					Replace(GUIGenerator_Macros.replacement_variable,class_elem.variableName_obj).
					Replace(GUIGenerator_Macros.replacement_variableParent,parent);

		}
		else{
			parent = class_elem.parent.variableName_obj;
		
			str += GUIGenerator_Macros.text_function_UpdateValues_GetGameObject.
				Replace(GUIGenerator_Macros.replacement_variable,class_elem.variableName_obj).
				Replace(GUIGenerator_Macros.replacement_variableParent,parent).
				Replace(GUIGenerator_Macros.replacement_name,class_elem.nameOrig);
		}
		
		if(class_elem.type != GUIGenerator_Elem_Base.TYPE.OTHER){
			str += GUIGenerator_Macros.text_function_UpdateValues_GetComponent.
				Replace(GUIGenerator_Macros.replacement_variable,class_elem.variableName).
				Replace(GUIGenerator_Macros.replacement_variableParent,class_elem.variableName_obj).
				Replace(GUIGenerator_Macros.replacement_name,class_elem.nameOrig).
				Replace(GUIGenerator_Macros.replacement_type,class_elem.classType);
		}

		str += "\n";

		for(int i = 0 ; i < class_elem.children.Count ; ++i){
			CreateFunctionUpdateValues(class_elem.children[i], ref str);
		}
	}
	#endregion

	#region FILE_CONTROLLER
	void CreateControllerFile(List<GUIGenerator_Elem_Base> list){
		string className = GUIGenerator_Macros.text_classPrefix + GUIGenerator_Macros.file_controller;
		string classContent = "";

		classContent += GUIGenerator_Macros.text_includes;

		classContent += GUIGenerator_Macros.text_classEventButton;
		classContent += GUIGenerator_Macros.text_classEventToggle;

		classContent += GUIGenerator_Macros.text_classDeclaration.Replace(GUIGenerator_Macros.replacement_name, className);

		classContent += GUIGenerator_Macros.text_regionBegin.Replace(GUIGenerator_Macros.replacement_name, GUIGenerator_Macros.text_regionMacro_Variables);

		//VARIABLES
		classContent += GUIGenerator_Macros.text_variableInstance.Replace(GUIGenerator_Macros.replacement_type, className);

		for(int i = 0 ; i < list.Count; ++i){
			classContent += GUIGenerator_Macros.text_gameObjectVariable.Replace(GUIGenerator_Macros.replacement_name,list[i].classInstanceName);
		}

		classContent += "\n";

		for(int i = 0 ; i < list.Count; ++i){
			bool hasVariables = false;
			if(list[i].HasButtons()){
				classContent += GUIGenerator_Macros.text_buttonEventHandlerVariable.
					Replace(GUIGenerator_Macros.replacement_type,GUIGenerator_Macros.text_classEventButtonName).
					Replace(GUIGenerator_Macros.replacement_variable,list[i].className);

				hasVariables = true;
			}

			if(list[i].HasToggle()){
				classContent += GUIGenerator_Macros.text_toggleEventHandlerVariable.
					Replace(GUIGenerator_Macros.replacement_type,GUIGenerator_Macros.text_classEventToggleName).
					Replace(GUIGenerator_Macros.replacement_variable,list[i].className);

				hasVariables = true;
			}

			if(hasVariables)
				classContent += "\n";
		}

		classContent += GUIGenerator_Macros.text_regionEnd;
		classContent += "\n";


		classContent += GUIGenerator_Macros.text_regionBegin.Replace(GUIGenerator_Macros.replacement_name, GUIGenerator_Macros.text_regionMacro_Setup);
		//FIND GAMEOBJECTS
		CreateFunctionFindObjcts(list, ref classContent);

		//INITIALIZE CLASSES
		CreateFunctionInitializeValues(list, ref classContent);

		//INITIALIZE LISTENERS
		CreateFunctionInitializeListeners(list, ref classContent);

		//EVENT BUTTON FUNCTION
		classContent += GUIGenerator_Macros.text_function_OnButtonPressed;
		classContent += "\n";

		//EVENT TOGGLE FUNCTION
		classContent += GUIGenerator_Macros.text_function_OnTogglePressed;


		classContent += GUIGenerator_Macros.text_regionEnd;
		classContent += "\n";


		classContent += GUIGenerator_Macros.text_regionBegin.Replace(GUIGenerator_Macros.replacement_name, GUIGenerator_Macros.text_regionMacro_Callbacks);
		//LISTENERS CALLBACKS
		CreateFunctionCallbacks(list, ref classContent);

		classContent += GUIGenerator_Macros.text_regionEnd;
		classContent += "\n";


		//UNITYCALLBACKS
		classContent += GUIGenerator_Macros.text_regionBegin.Replace(GUIGenerator_Macros.replacement_name, GUIGenerator_Macros.text_regionMacro_UnityCallbacks);

		classContent += GUIGenerator_Macros.text_function_ControllerStart_Full;
		classContent += "\n";
		classContent += GUIGenerator_Macros.text_function_Update_Commented;
		classContent += "\n";
		classContent += GUIGenerator_Macros.text_function_OnGUI_Commented;

		classContent += GUIGenerator_Macros.text_regionEnd;


		classContent += GUIGenerator_Macros.text_classEnd;
		
		File.WriteAllText(directory_GUIScripts + className + GUIGenerator_Macros.file_end, classContent);
	}

	void CreateFunctionFindObjcts(List<GUIGenerator_Elem_Base> list, ref string classContent){
		classContent += GUIGenerator_Macros.text_function_FindObjects_Header;
		for(int i = 0 ; i < list.Count; ++i){
			classContent += GUIGenerator_Macros.text_function_FindObjects_Method_Find.
				Replace(GUIGenerator_Macros.replacement_variable,list[i].classInstanceName).
					Replace(GUIGenerator_Macros.replacement_name,list[i].nameOrig);
		}
		classContent += GUIGenerator_Macros.text_function_End;
		classContent += "\n";
	}

	void CreateFunctionInitializeValues(List<GUIGenerator_Elem_Base> list, ref string classContent){
		classContent += GUIGenerator_Macros.text_function_InitializeValues_Header;
		for(int i = 0 ; i < list.Count; ++i){
			classContent += GUIGenerator_Macros.text_function_InitializeValues_Method_ResetPosition.
				Replace(GUIGenerator_Macros.replacement_variable,list[i].classInstanceName);
		}
		classContent += "\n";
		
		for(int i = 0 ; i < list.Count; ++i){
			classContent += GUIGenerator_Macros.text_function_InitializeValues_Method_UpdateValues.
				Replace(GUIGenerator_Macros.replacement_name, list[i].className).
					Replace(GUIGenerator_Macros.replacement_variable, list[i].classInstanceName);
		}
		//classContent += "\n";
		
		for(int i = 0 ; i < list.Count; ++i){
			classContent += GUIGenerator_Macros.text_function_InitializeValues_Method_SetActive.
				Replace(GUIGenerator_Macros.replacement_name, list[i].classInstanceName).
				Replace(GUIGenerator_Macros.replacement_variable, i == 0 ? "true" : "false");
		}
		classContent += GUIGenerator_Macros.text_function_End;
		classContent += "\n";
	}

	void CreateFunctionInitializeListeners(List<GUIGenerator_Elem_Base> list, ref string classContent){
		classContent += GUIGenerator_Macros.text_function_InitializeListeners_Header;

		for(int i = 0 ; i < list.Count; ++i){
			List<GUIGenerator_Elem_Base> listButtons = new List<GUIGenerator_Elem_Base>();
		
			AggregateButtons(list[i].children, ref listButtons);

			if(listButtons.Count > 0){
				int ac = 0;
				for(int j = 0 ; j < listButtons.Count; ++j){
					classContent += GUIGenerator_Macros.text_function_InitializeListeners_AddClickListener.
							Replace(GUIGenerator_Macros.replacement_variable, list[i].className).
							Replace(GUIGenerator_Macros.replacement_variable2, listButtons[j].variableName).
							Replace(GUIGenerator_Macros.replacement_variable3, list[i].classButtonListenerFunction.Replace(GUIGenerator_Macros.replacement_variable,"" + ac++));
				}

				classContent += "\n";
			}

			List<GUIGenerator_Elem_Base> listToggles = new List<GUIGenerator_Elem_Base>();
			
			AggregateToggles(list[i].children, ref listToggles);
			
			if(listToggles.Count > 0){
				int ac = 0;
				for(int j = 0 ; j < listToggles.Count; ++j){
					classContent += GUIGenerator_Macros.text_function_InitializeListeners_AddToggleListener.
							Replace(GUIGenerator_Macros.replacement_variable, list[i].className).
							Replace(GUIGenerator_Macros.replacement_variable2, listToggles[j].variableName).
							Replace(GUIGenerator_Macros.replacement_variable3, list[i].classToggleListenerFunction.Replace(GUIGenerator_Macros.replacement_variable,"" + ac++));
				}
				
				classContent += "\n";
			}
		}

		classContent += GUIGenerator_Macros.text_function_End;
		classContent += "\n";
	}

	void CreateFunctionCallbacks(List<GUIGenerator_Elem_Base> list, ref string classContent){
		for(int i = 0 ; i < list.Count ; ++i){
			List<GUIGenerator_Elem_Base> listButtons = new List<GUIGenerator_Elem_Base>();

			AggregateButtons(list[i].children, ref listButtons);

			if(listButtons.Count > 0){
				classContent += GUIGenerator_Macros.text_function_OnButtonCallback_Header.
					Replace(GUIGenerator_Macros.replacement_name, list[i].name);
				
				classContent += GUIGenerator_Macros.text_function_OnButtonCallback_SwitchInit;

				int ac = 0;
				for(int j = 0 ; j < listButtons.Count; ++j){
					string eventVariable = GUIGenerator_Macros.text_buttonEventHandlerVariableName.
						Replace(GUIGenerator_Macros.replacement_variable,list[i].className);

					classContent += GUIGenerator_Macros.text_function_OnButtonCallback_Case.
							Replace(GUIGenerator_Macros.replacement_variable, "" + ac++).
							Replace(GUIGenerator_Macros.replacement_variableParent, list[i].name.ToUpper()).
							Replace(GUIGenerator_Macros.replacement_name, listButtons[j].name.ToUpper()).
							Replace(GUIGenerator_Macros.replacement_variable2, eventVariable);
				}

				classContent += GUIGenerator_Macros.text_function_OnButtonCallback_SwitchEnd;
				
				classContent += GUIGenerator_Macros.text_function_End;
				classContent += "\n";
			}

			List<GUIGenerator_Elem_Base> listToggles = new List<GUIGenerator_Elem_Base>();

			AggregateToggles(list[i].children, ref listToggles);
			
			if(listToggles.Count > 0){
				classContent += GUIGenerator_Macros.text_function_OnToggleCallback_Header.
					Replace(GUIGenerator_Macros.replacement_name, list[i].name);
				
				classContent += GUIGenerator_Macros.text_function_OnToggleCallback_SwitchInit;
				
				int ac = 0;
				for(int j = 0 ; j < listToggles.Count; ++j){
					string eventVariable = GUIGenerator_Macros.text_toggleEventHandlerVariableName.
						Replace(GUIGenerator_Macros.replacement_variable,list[i].className);

					classContent += GUIGenerator_Macros.text_function_OnToggleCallback_Case.
						Replace(GUIGenerator_Macros.replacement_variable, "" + ac++).
							Replace(GUIGenerator_Macros.replacement_variableParent, list[i].name.ToUpper()).
							Replace(GUIGenerator_Macros.replacement_name, listToggles[j].name.ToUpper()).
							Replace(GUIGenerator_Macros.replacement_variable2, eventVariable);
				}
				
				classContent += GUIGenerator_Macros.text_function_OnToggleCallback_SwitchEnd;
				
				classContent += GUIGenerator_Macros.text_function_End;
				classContent += "\n";
			}
		}
	}

	void AggregateButtons(List<GUIGenerator_Elem_Base> mainList, ref List<GUIGenerator_Elem_Base> listButtons){
		for(int i = 0 ; i < mainList.Count; ++i){
			if(mainList[i].type == GUIGenerator_Elem_Base.TYPE.BUTTON){
				listButtons.Add(mainList[i]);
			}

			if(mainList[i].children.Count > 0){
				AggregateButtons(mainList[i].children, ref listButtons);
			}
		}
	}

	void AggregateToggles(List<GUIGenerator_Elem_Base> mainList, ref List<GUIGenerator_Elem_Base> listToggles){
		for(int i = 0 ; i < mainList.Count; ++i){
			if(mainList[i].type == GUIGenerator_Elem_Base.TYPE.TOGGLE){
				listToggles.Add(mainList[i]);
			}
			
			if(mainList[i].children.Count > 0){
				AggregateToggles(mainList[i].children, ref listToggles);
			}
		}
	}

	#endregion

	#region FILE_ANIMATION
	void CreateAnimationFile(){
		string className = GUIGenerator_Macros.text_classPrefix + GUIGenerator_Macros.file_animation;
		string classContent = "";
		
		classContent += GUIGenerator_Macros.text_includes;
		classContent += GUIGenerator_Macros.text_classDeclaration.Replace(GUIGenerator_Macros.replacement_name, className);

		//VARIABLES
		classContent += GUIGenerator_Macros.text_regionBegin.Replace(GUIGenerator_Macros.replacement_name, GUIGenerator_Macros.text_regionMacro_Variables);
		classContent += GUIGenerator_Macros.text_variableAnimationInstance.Replace(GUIGenerator_Macros.replacement_variable, className);
		classContent += GUIGenerator_Macros.text_variableAnimationTime;
		classContent += GUIGenerator_Macros.text_regionEnd;
		classContent += "\n";

		//SHOW/HIDE
		classContent += GUIGenerator_Macros.text_regionBegin.Replace(GUIGenerator_Macros.replacement_name, GUIGenerator_Macros.text_regionMacro_AnimationShowHide);
		classContent += GUIGenerator_Macros.text_function_ShowMenu + "\n";
		classContent += GUIGenerator_Macros.text_function_HideMenu;
		classContent += GUIGenerator_Macros.text_regionEnd;
		classContent += "\n";

		//SLIDE
		classContent += GUIGenerator_Macros.text_regionBegin.Replace(GUIGenerator_Macros.replacement_name, GUIGenerator_Macros.text_regionMacro_AnimationSlide);
		classContent += GUIGenerator_Macros.text_functionSideBase 	+ "\n";
		classContent += GUIGenerator_Macros.text_functionSlidePanel + "\n";
		classContent += GUIGenerator_Macros.text_functionSideBringRight + "\n";
		classContent += GUIGenerator_Macros.text_functionSideBringLeft 	+ "\n";
		classContent += GUIGenerator_Macros.text_functionSideRemoveRight+ "\n";
		classContent += GUIGenerator_Macros.text_functionSideRemoveLeft + "\n";
		classContent += GUIGenerator_Macros.text_functionSideBringTop + "\n";
		classContent += GUIGenerator_Macros.text_functionSideBringBottom + "\n";
		classContent += GUIGenerator_Macros.text_functionSideRemoveTop + "\n";
		classContent += GUIGenerator_Macros.text_functionSideRemoveBottom + "\n";
		classContent += GUIGenerator_Macros.text_functionSideBaseInstance + "\n";
		classContent += GUIGenerator_Macros.text_regionEnd;
		classContent += "\n";

		//UNITYCALLBACKS
		classContent += GUIGenerator_Macros.text_regionBegin.Replace(GUIGenerator_Macros.replacement_name, GUIGenerator_Macros.text_regionMacro_UnityCallbacks);		
		classContent += GUIGenerator_Macros.text_functionAnimationAwake.Replace(GUIGenerator_Macros.replacement_variable, className);	
		classContent += GUIGenerator_Macros.text_regionEnd;

		classContent += GUIGenerator_Macros.text_classEnd;
		
		File.WriteAllText(directory_GUIScripts + className + GUIGenerator_Macros.file_end, classContent);
	}

	#endregion

	#endregion

	#region RUN
	public List<string> ExistingFiles(){
		CreatePaths();

		List<string> files = new List<string>();

		List<GUIGenerator_Elem_Base> list = new List<GUIGenerator_Elem_Base>();
		
		GetAllComponents(this.gameObject, ref list, null);
		
		//CHECK FOR REPEATED NAMES
		FindAndResolveRepeatedElements(list);

		//Create one file for each main panel
		for(int i = 0 ; i < list.Count ; ++i){
			list[i].className = GUIGenerator_Macros.text_classPrefix + list[i].name;

			string classFile = directory_GUIScripts + list[i].className + GUIGenerator_Macros.file_end;

			if(File.Exists(classFile)){
				files.Add(list[i].className + GUIGenerator_Macros.file_end);
			}
		}

		//ANIMATION FILE
		string animationFile = directory_GUIScripts + GUIGenerator_Macros.text_classPrefix + GUIGenerator_Macros.file_animation + GUIGenerator_Macros.file_end;
		if(File.Exists(animationFile)){
			files.Add(GUIGenerator_Macros.file_animation + GUIGenerator_Macros.file_end);
		}
		//CONTROLLER FILE
		string controllerFile = directory_GUIScripts + GUIGenerator_Macros.text_classPrefix + GUIGenerator_Macros.file_controller + GUIGenerator_Macros.file_end;
		if(File.Exists(controllerFile)){
			files.Add(GUIGenerator_Macros.file_controller + GUIGenerator_Macros.file_end);
		}

		return files;
	}

	public void GenerateFiles(){
		CreatePaths();
		CreateDirectories();
		
		CreateAll();

		GUIGenerator_Elem_Base.idAc = 0;
	}
	#endregion
}
