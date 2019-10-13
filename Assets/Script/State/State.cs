using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public Controller player;
    public bool leftstep;
    public float timebetweenstep;

    public State(Controller player)
    {
        this.player = player;
        player.GetComponent<Animator>().SetBool("Write", false);
        player.cameradeath.SetFloat("compteur", -0.1f);
        player.GetComponent<Animator>().SetFloat("CompteurMort", -0.1f);
        player.vignette.SetFloat("compteur", -0.1f);
    }
    public virtual void Update()
    {
        if (player.machine.progress > 0) player.machine.progress -= Time.deltaTime;
        else player.machine.progress = 0;

        // Si le joueur va en arrière
        if (Input.GetAxis("Horizontal") < 0)
        {
            if (!player.GetComponent<Animator>().GetBool("Bloc"))
                player.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_NoBack/SFX_NoBack_" + Controller.GetRandom(4)));
            player.GetComponent<Animator>().SetBool("Bloc", true);

        }
        // Sinon
        else
        {
            Vector3 move = Vector3.zero;
            move.x = Input.GetAxis("Horizontal");
            player.GetComponent<Animator>().SetBool("Bloc", false);
            //move.y = Input.GetAxis("Vertical");
            move = move.normalized * player.speed;

            player.GetComponent<Animator>().SetFloat("Move", move.magnitude);

            // Si le joueur avance
            if (move.x > 0) { 
                // Gestion du son des pas
                timebetweenstep += Time.deltaTime * 2;
                if (timebetweenstep > 1)
                {
                    timebetweenstep = 0;
                    leftstep = !leftstep;
                    player.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sound/Footsteps/FTS_Full/FTS_" + ((leftstep) ? "Left_" : "Right_") + Controller.GetRandom(7)));
                }
                // Calcul des caractères du joueurs
                player.nbchar += player.charCurve.Evaluate((player.transform.position.x - player.originalpos) / 120) * Time.deltaTime;
            }
            // Déplacement
            player.GetComponent<Rigidbody2D>().velocity = move;
            ReplaceCam();
        }
    }

    public virtual void DieInput()
    {
        player.state = new Die(player);
    }
    public virtual void WriteInput()
    {
        player.state = new Write(player);
    }

    private void ReplaceCam()
    {
        Vector3 campos = Camera.main.transform.parent.position;
        campos.x = player.transform.position.x + 2.5f;
        campos.y = player.machine.walkheight;
        Camera.main.transform.parent.transform.position = campos;
    }
}
