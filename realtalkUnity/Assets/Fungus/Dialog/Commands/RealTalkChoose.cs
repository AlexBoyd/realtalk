using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fungus
{
    [CommandInfo("Dialog", 
                 "Real Talk Choose", 
                 "Presents a list of options for the player to choose from, with an optional timeout. Add options using preceding AddOption commands.")]
    public class RealTalkChoose : Command
    {
        public class RealTalkOption
        {
            public string optionText;
            public Sequence targetSequence;
            public float valence;
        }

       
        static public List<RealTalkOption> options = new List<RealTalkOption> ();
        [TextArea(5,10)]
        public string
            chooseText;
        public Character character;
        public AudioClip voiceOverClip;
        public float timeoutDuration;
        protected bool showBasicGUI;
        public FungusScript fungus = null;

        public override void OnEnter ()
        {
            if (fungus == null)
            {
                fungus = GetFungusScript ();
            }
            RealTalkChooseDialog dialog = SetRealTalkChooseDialog.activeDialog;
            showBasicGUI = false;
            if (dialog == null)
            {
                // Try to get any SayDialog in the scene
                dialog = GameObject.FindObjectOfType<RealTalkChooseDialog> ();
                if (dialog == null)
                {
                    showBasicGUI = true;
                    return;
                }
            }

            if (options.Count == 0)
            {
                Continue ();
            }
            else
            {
                dialog.ShowDialog (true);
                dialog.SetCharacter (character);
                
                List<RealTalkChooseDialog.Option> dialogOptions = new List<RealTalkChooseDialog.Option> ();
                switch (dialog.RTMode)
                {
                case RealTalkChooseDialog.RealTalkMode.Control:
                    foreach (RealTalkOption option in options)
                    {
                        RealTalkChooseDialog.Option dialogOption = new RealTalkChooseDialog.Option ();
                        dialogOption.text = option.optionText;
                        dialogOption.valence = option.valence;
                        Sequence onSelectSequence = option.targetSequence;
                        
                        dialogOption.onSelect = delegate {
                            
                            dialog.ShowDialog (false);
                            
                            if (onSelectSequence == null)
                            {
                                Continue ();
                            }
                            else
                            {
                                ExecuteSequence (onSelectSequence);
                            }
                        };
                        
                        dialogOptions.Add (dialogOption);
                    }
                    
                    options.Clear ();
                    
                    if (voiceOverClip != null)
                    {
                        MusicController.GetInstance ().PlaySound (voiceOverClip, 1f);
                    }
                    
                    dialog.Choose (chooseText, dialogOptions, 0, delegate {
                        dialog.ShowDialog (false);
                        Continue ();
                    });
                    break;

                case RealTalkChooseDialog.RealTalkMode.Slider:
                    dialog.Choose (chooseText, dialogOptions, timeoutDuration, delegate {
                        dialog.ShowDialog (false);
                        float val = dialog.emotionalSlider.value;
                        options.Sort ((a, b) => Math.Abs (val - a.valence).CompareTo (Math.Abs (val - b.valence)));
                        ExecuteSequence (options.First ().targetSequence);
                        options.Clear();
                        dialog.ShowDialog (false);

                    });

                    break;

                case RealTalkChooseDialog.RealTalkMode.MoodMode:

                    dialog.Choose(chooseText, dialogOptions, timeoutDuration, delegate {
                        options.Sort ((a, b) => a.valence.CompareTo (b.valence));
                        if (dialog.em1.isOn)
                        {
                            ExecuteSequence (options [0].targetSequence);
                        }
                        if (dialog.em2.isOn)
                        {
                            ExecuteSequence (options [1].targetSequence);
                        }
                        if (dialog.em3.isOn)
                        {
                            ExecuteSequence (options [2].targetSequence);
                        }
                        options.Clear();
                        dialog.ShowDialog (false);


                    });


                    break;
                }
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
                foreach (RealTalkOption option in options)
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

            foreach (RealTalkOption option in options)
            {
                if (GUILayout.Button (option.optionText))
                {
                    options.Clear ();
                    showBasicGUI = false;
                    ExecuteSequence (option.targetSequence);
                }
            }

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