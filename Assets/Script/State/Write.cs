using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Write : State
{
    private string text;
    public Write(Controller player) : base(player)
    {
        player.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_TakeTypewriter/SFX_TakeTypewriter_" + Controller.GetRandom(4)));
        player.GetComponent<Animator>().SetBool("Write", true);
        player.field.ActivateInputField();
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
    public override void Update()
    {
        if (player.machine.progress < player.machine.speedProgress) player.machine.progress += Time.deltaTime;
        else player.machine.progress = player.machine.speedProgress;

        string oldtext = player.text;
        // Mise à jour du texte
        player.text = player.lastText + player.field.text;
        if (oldtext.Length < player.text.Length && player.text.Length <= player.nbchar) { 
                player.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_Typewiter/SFX_Key/SFX_Typewriter_Key_" + Controller.GetRandom(13)));
        }
        else if (player.text.Length > player.nbchar)
        {
            player.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_Typewiter/SFX_Spool/SFX_Typewriter_Spool_" + Controller.GetRandom(4)));
            player.text = player.text.Substring(0, (int)Mathf.Floor(player.nbchar));
            player.field.text = player.field.text.Substring(0, player.text.Length - player.lastText.Length);
        }
    }


    public override void DieInput()
    {

    }
    public override void WriteInput()
    {
        player.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_AwayTypewriter/SFX_AwayTypewriter_" + Controller.GetRandom(4)));
        player.lastText = player.text;
        player.field.text = "";
        player.field.DeactivateInputField();
        player.state = new State(player);
    }
}
