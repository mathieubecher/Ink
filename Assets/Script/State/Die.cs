using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Die : State
{
    public float count;
    private float MAXDEADCOUNT = 5;
    private bool dead = false;
    private bool cancel = false;

    public Die(Controller player) : base(player)
    {
        count = MAXDEADCOUNT;
        player.GetComponent<Animator>().SetBool("Write", false);
        player.GetComponent<Animator>().SetFloat("Move", 0);
        player.GetComponent<Animator>().SetBool("Bloc", false);
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_Die/SFX_Die"));
        player.cameradeath.enabled = true;  
    }

    public override void Update()
    {
        if (Input.GetKey(KeyCode.M) && !cancel) count -= Time.deltaTime;
        else if (!Input.GetKey(KeyCode.M) && !cancel)
        {
            cancel = true;
            player.GetComponent<AudioSource>().Stop();
            player.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_Die/SFX_DieBack"));
        }
        if (cancel)
        {
            count += MAXDEADCOUNT;
            if (player.cameradeath.GetCurrentAnimatorStateInfo(0).IsName("Camera"))
            {
                player.cameradeath.enabled = false;
                player.state = new State(player);
            }
        }

        player.cameradeath.SetFloat("compteur", MAXDEADCOUNT - count);
        player.vignette.SetFloat("compteur", MAXDEADCOUNT - count);
        player.GetComponent<Animator>().SetFloat("CompteurMort", MAXDEADCOUNT - count);
        
        if (count < 0 && !dead) player.StartCoroutine(Exit());
    }

    public IEnumerator Exit()
    {
        // Valide la mort. Evite d'appeler plusieur fois la fonction
        dead = true;

        // Si le joueur a écrit quelque chose
        if (player.text != "")
        {
            // Préparation de la requete
            string textParse = player.text.Replace('"', ' ');
            string time = TwoChar(System.DateTime.Now.Day) + "/" + TwoChar(System.DateTime.Now.Month) + "/" + System.DateTime.Now.Year + " " + TwoChar(System.DateTime.Now.Hour) + ":" + TwoChar(System.DateTime.Now.Minute) + ":" + TwoChar(System.DateTime.Now.Second);

            WWWForm form = new WWWForm();
            form.AddField("position", "" + player.transform.position.x);
            form.AddField("text", textParse);
            form.AddField("time", time);
            UnityWebRequest www = UnityWebRequest.Post("http://portfoliobecher.com/Ink/SetDead.php", form);

            // Envoie de la requête
            yield return www.SendWebRequest();
            while (!www.isDone)
            {

            }

        }
        // Fin de la partie
        player.state = new End(player);

    }

    public override void DieInput()
    {

    }
    public override void WriteInput()
    {

    }


    public string TwoChar(int value)
    {
        string tochar = "" + value;
        if (value < 10) tochar = "0" + tochar;
        return tochar;
    }
}
