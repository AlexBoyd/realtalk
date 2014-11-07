using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fungus
{
    [CommandInfo("Dialog", 
             "SayVideo", 
             "PlayVideo")]
    public class SayVideo : Command
    {
        public MovieTexture movie;
        public new GUITexture guiTexture;
        public new AudioSource audio;
        private bool skipped = false;
        private bool playing = false;

        public override void OnEnter ()
        {
            StartCoroutine (PlayMovieCoroutine ());

        }

        private IEnumerator PlayMovieCoroutine ()
        {
            skipped = false;
            playing = true;
            guiTexture.color = new Color (1, 1, 1, 0);
            movie.Play ();
            guiTexture.texture = movie;
            
            audio.clip = ((MovieTexture)guiTexture.texture).audioClip;
            audio.Play ();
            audio.volume = 0;
            yield return new WaitForSeconds (0.3f);
            guiTexture.color = new Color (1, 1, 1, 1);
            audio.volume = 1;
            yield return new WaitForSeconds (movie.duration - 0.1f);
            if (!skipped)
            {
                playing = false;
                ((MovieTexture)guiTexture.texture).Stop ();
                Continue ();
            }
        }
        
        private void Update ()
        {
            if (Input.GetKeyUp (KeyCode.Space) && playing)
            {
                skipped = true;
                playing = false;
                ((MovieTexture)guiTexture.texture).Stop ();

                Continue ();
            }
        }
    }
}