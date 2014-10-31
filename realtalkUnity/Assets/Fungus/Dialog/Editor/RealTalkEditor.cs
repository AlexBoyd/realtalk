﻿using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rotorz.ReorderableList;

namespace Fungus
{
	
	[CustomEditor (typeof(RealTalk))]

	public class RealTalkEditor : CommandEditor 
	{
		static public bool showTagHelp;
		
		protected SerializedProperty realTalkTextProp;
		protected SerializedProperty characterProp;
		protected SerializedProperty voiceOverClipProp;
		protected SerializedProperty movieClipProp;

		protected SerializedProperty timeoutDurationProp;
		
		protected virtual void OnEnable()
		{
			realTalkTextProp = serializedObject.FindProperty("chooseText");
			characterProp = serializedObject.FindProperty("character");
			voiceOverClipProp = serializedObject.FindProperty("voiceOverClip");
			movieClipProp = serializedObject.FindProperty("movieClip");
			timeoutDurationProp = serializedObject.FindProperty("timeoutDuration");
		}
		
		public override void DrawCommandGUI() 
		{
			serializedObject.Update();
			
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button(new GUIContent("Tag Help", "Show help info for tags"), new GUIStyle(EditorStyles.miniButton)))
			{
				showTagHelp = !showTagHelp;
			}
			EditorGUILayout.EndHorizontal();
			
			if (showTagHelp)
			{
				SayEditor.DrawTagHelpLabel();
			}
			
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.PropertyField(realTalkTextProp);
			
			RealTalk t = target as RealTalk;
			
			if (t.character != null &&
			    t.character.profileSprite != null &&
			    t.character.profileSprite.texture != null)
			{
				Texture2D characterTexture = t.character.profileSprite.texture;
				
				float aspect = (float)characterTexture.width / (float)characterTexture.height;
				Rect previewRect = GUILayoutUtility.GetAspectRect(aspect, GUILayout.Width(50), GUILayout.ExpandWidth(false));
				CharacterEditor characterEditor = Editor.CreateEditor(t.character) as CharacterEditor;
				characterEditor.DrawPreview(previewRect, characterTexture);
				DestroyImmediate(characterEditor);
			}
			
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Separator();
			
			CommandEditor.ObjectField<Character>(characterProp,
			                                     new GUIContent("Character", "Character to display in dialog"), 
			                                     new GUIContent("<None>"),
			                                     Character.activeCharacters);
			
			EditorGUILayout.PropertyField(voiceOverClipProp, new GUIContent("Voice Over Clip", "Voice over audio to play when the Real Talk text is displayed"));

			EditorGUILayout.PropertyField(movieClipProp, new GUIContent("Movie Clip", "Movie to play when the Real Talk text is displayed"));

			EditorGUILayout.PropertyField(timeoutDurationProp, new GUIContent("Timeout Duration", "Time limit for player to make a choice. Set to 0 for no limit."));
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}