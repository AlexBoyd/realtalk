using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fungus
{
    [CommandInfo("Dialog", 
                 "RealTalk", 
                 "Presents RealTalk. Add options using preceding AddRealTalkOption commands.")]

    public class RealTalk : Command
    {
        public class Option
        {
            public string optionTitle;
            public Sequence targetSequence;
            public float valence;
        }

        static public List<Option> realTalkOptionsList = new List<Option> ();
        [TextArea(5,10)]
        public string
            chooseText;
        public Character character;
        public AudioClip voiceOverClip;
        public MovieTexture movieClip;
        public float timeoutDuration;
        protected bool showBasicGUI;
        
        public override void OnEnter ()
        {
            RealTalkDialog dialog = SetRealTalkDialog.activeDialog;
            showBasicGUI = false;
            if (dialog == null)
            {
                // Try to get any SayDialog in the scene
                dialog = GameObject.FindObjectOfType<RealTalkDialog> ();

                if (dialog == null)
                {
                    showBasicGUI = true;
                    return;
                }
            }

            if (realTalkOptionsList.Count == 0)
            {
                Continue ();
            } else
            {
                dialog.ShowDialog (true);
                dialog.SetCharacter (character);
                List<RealTalkDialog.Option> dialogOptions = new List<RealTalkDialog.Option> ();
                foreach (Option option in realTalkOptionsList)
                {
                    RealTalkDialog.Option dialogOption = new RealTalkDialog.Option ();
                    dialogOption.text = option.optionTitle;
                    dialogOption.valence = option.valence;
                    Sequence onSelectSequence = option.targetSequence;
                    
                    dialogOption.onSelect = delegate {
                        
                        dialog.ShowDialog (false);
                        
                        if (onSelectSequence == null)
                        {
                            Continue ();
                        } else
                        {
                            ExecuteSequence (onSelectSequence);
                        }
                    };
                    
                    dialogOptions.Add (dialogOption);
                }

                if (voiceOverClip != null)
                {
                    MusicController.GetInstance ().PlaySound (voiceOverClip, 1f);
                }

                if (movieClip != null)
                {
                    // Do something with movie texture
                }
                
                dialog.RealTalk (chooseText, dialogOptions, timeoutDuration, delegate() {
                    dialog.ShowDialog (false);

                    float executableOption;
                    foreach (Option option in realTalkOptionsList)
                    {
                        executableOption = Math.Abs (option.valence - Input.GetAxis ("Horizontal"));
//                            realTalkOptionsList.Clear ();

                        // Logic for getting valence property closest to slider value
                        showBasicGUI = false;
                        ExecuteSequence (option.targetSequence);
                    }

                    realTalkOptionsList.Clear ();

                    Continue ();
                });
            }
        }
        
        public override string GetSummary ()
        {
            return "\"" + chooseText + "\"";
        }
        
        public override void GetConnectedSequences (ref List<Sequence> connectedSequences)
        {
            // Show connected sequences from preceding AddOption commands
            if (IsExecuting ())
            {
                foreach (Option option in realTalkOptionsList)
                {
                    if (option.targetSequence != null)
                    {
                        connectedSequences.Add (option.targetSequence);
                    }
                }
            }
        }
        
        protected virtual void OnGUI ()
        {
            if (!showBasicGUI)
            {
                return;
            }
            
            // Draw a basic GUI to use when no uGUI dialog has been set
            // Does not support drawing character images
            
            GUILayout.BeginHorizontal (GUILayout.Width (Screen.width));
            GUILayout.FlexibleSpace ();
            
            GUILayout.BeginVertical (GUILayout.Height (Screen.height));
            GUILayout.FlexibleSpace ();
            
            GUILayout.BeginVertical (new GUIStyle (GUI.skin.box));
            
            if (character != null)
            {
                GUILayout.Label (character.nameText);
                GUILayout.Space (10);
            }
            
            GUILayout.Label (chooseText);

//            float executableOption;
//          foreach (Option option in realTalkOptionsList)
//          {
//                executableOption = Math.Abs(option.valence - RealTalkDialog.valenceSlider.value);
//              if (executableOption < 1.01)
//              {
//                  realTalkOptionsList.Clear();
//                  showBasicGUI = false;
//                  ExecuteSequence(option.targetSequence);
//              }
//          }
            
            GUILayout.EndVertical ();
            
            GUILayout.FlexibleSpace ();
            GUILayout.EndVertical ();
            
            GUILayout.FlexibleSpace ();
            GUILayout.EndHorizontal ();
        }
        
        public override Color GetButtonColor ()
        {
            return new Color32 (184, 210, 235, 255);
        }
    
    }
}