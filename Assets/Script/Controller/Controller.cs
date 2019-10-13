using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public State state;

    // Vitesse de déplacement
    [SerializeField] public float speed = 1.5f;

    // Machine à écrire
    [SerializeField] public Machine machine;
    public InputField field;
    public string text;

    // Courbe de progression des caractères
    public NbChar nbChar;
    public SpriteMask mask;
    public SpriteRenderer ombre;
    
    public Animator vignette;


    // Start is called before the first frame update
    void Start()
    {
        state = new State(this);
            
    }
    public static string GetRandom(int maxvalue)
    {
        string number = "";
        int rand = (int)Mathf.Floor((Random.value) * maxvalue + 1);
        if (rand < 10) number += "0";
        return number + rand;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.M)) state.DieInput();
        if (Input.GetKeyDown(KeyCode.Tab)) state.WriteInput();

        state.Update();
        UpdateOmbre();
    }

    public void UpdateOmbre()
    {
        ombre.color = new Color(1, 1, 1, ((Mathf.Floor(nbChar.number) - text.Length) / 250.0f));
        mask.sprite = GetComponent<SpriteRenderer>().sprite;
    }   
}
