using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : State
{
    private float count = 2;
    public End(Controller player): base(player)
    {
        player.cameradeath.SetFloat("compteur", 0);
        player.vignette.SetFloat("compteur", 0);
        player.GetComponent<Animator>().SetFloat("CompteurMort", 0);
    }

    public override void Update()
    {
        count -= Time.deltaTime;
        if(count <= 0) SceneManager.LoadScene(0);
    }

    public override void DieInput()
    {

    }
    public override void WriteInput()
    {

    }
}
