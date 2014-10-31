using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rotorz.ReorderableList;

namespace Fungus
{

	[CustomEditor (typeof(AddRealTalkOption))]
	public class AddRealTalkOptionEditor : CommandEditor 
	{
		protected SerializedProperty realTalkOptionTitleProp;
		protected SerializedProperty realTalkOptionValenceProp;
		protected SerializedProperty hideOnSelectedProp;
		protected SerializedProperty targetSequenceProp;
		
		protected virtual void OnEnable()
		{
			realTalkOptionTitleProp = serializedObject.FindProperty("optionTitle");
			realTalkOptionValenceProp = serializedObject.FindProperty ("valence");
			hideOnSelectedProp = serializedObject.FindProperty("hideOnSelected");
			targetSequenceProp = serializedObject.FindProperty("targetSequence");
		}
		
		public override void DrawCommandGUI() 
		{
			serializedObject.Update();
			
			AddRealTalkOption t = target as AddRealTalkOption;
			
			EditorGUILayout.PropertyField(realTalkOptionTitleProp, new GUIContent("Option Title", "Title of the option."));
			EditorGUILayout.PropertyField(realTalkOptionValenceProp, new GUIContent("Valence", "Valence property of the option."));

			
			SequenceEditor.SequenceField(targetSequenceProp,
			                             new GUIContent("Target Sequence", "Sequence to execute when this option is selected by the player."),
			                             new GUIContent("<Continue>"),
			                             t.GetFungusScript());
			
			EditorGUILayout.PropertyField(hideOnSelectedProp, new GUIContent("Hide On Selected", "Hide this option forever once the player has selected it."));
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}