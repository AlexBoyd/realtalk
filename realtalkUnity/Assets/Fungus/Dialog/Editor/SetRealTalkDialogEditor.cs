using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rotorz.ReorderableList;

namespace Fungus
{
	
	[CustomEditor (typeof(SetRealTalkDialog))]
	public class SetRealTalkDialogEditor : CommandEditor 
	{
		protected SerializedProperty realTalkDialogProp;
		
		protected virtual void OnEnable()
		{
			realTalkDialogProp = serializedObject.FindProperty("realTalkDialog");
		}
		
		public override void DrawCommandGUI() 
		{
			serializedObject.Update();
			
			CommandEditor.ObjectField<RealTalkDialog>(realTalkDialogProp,
			                                        new GUIContent("Real Talk Dialog", "Dialog to use when displaying options with the Choose command."), 
			                                        new GUIContent("<None>"),
			                                        RealTalkDialog.activeDialogs);
			serializedObject.ApplyModifiedProperties();
		}
	}
}