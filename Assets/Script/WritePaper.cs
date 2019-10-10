using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WritePaper : MonoBehaviour
{
    public Controller player;
    private float beginheight = 0;
    [SerializeField] private TextMeshPro text;
    private float spawnProgress = 0;
    float originpos;
    [SerializeField] private int nbline = 10;
    private float MAXHEIGHT = 10;
    // Start is called before the first frame update
    void Start()
    {
        originpos = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        
        string[] split = player.text.Split('\n');
        string splittext ="";
        for(int i = split.Length-1; i>=0 ;--i)
        {
            if (i >= split.Length - nbline) splittext = split[i] + '\n' + splittext;
        }
        
        */
        text.text = player.text;
        float lastheight = text.GetTextInfo(text.text).textComponent.GetPreferredValues().y;
        float height = text.GetTextInfo(text.text).textComponent.GetPreferredValues().y;
        if (height > lastheight) player.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_Typewiter/SFX_Bell/SFX_Typewriter_Bell_" + Controller.GetRandom(4)));

        while (height > MAXHEIGHT)
        {
            text.text = text.text.Substring(1);
            height = text.GetTextInfo(text.text).textComponent.GetPreferredValues().y;
        }
        transform.localPosition = new Vector3(transform.localPosition.x, originpos + height,0);


    }   
}
