using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rotorz.ReorderableList;

namespace Fungus
{
	
	[CustomEditor (typeof(SetRealTalkChooseDialog))]
	public class SetRealTalkDialogChooseEditor : CommandEditor 
	{
		protected SerializedProperty realTalkDialogProp;
		
		protected virtual void OnEnable()
		{
			realTalkDialogProp = serializedObject.FindProperty("realTalkChooseDialog");
		}
		
		public override void DrawCommandGUI() 
		{
			serializedObject.Update();
			
                        CommandEditor.ObjectField<RealTalkChooseDialog>(realTalkDialogProp,
			                                        new GUIContent("Real Talk Dialog", "Dialog to use when displaying options with the Choose command."), 
			                                        new GUIContent("<None>"),
                                                    RealTalkChooseDialog.activeDialogs);
			serializedObject.ApplyModifiedProperties();
		}
	}
}