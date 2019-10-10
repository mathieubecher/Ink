using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DeadPaper : MonoBehaviour
{
    public AnimationCurve curve;
    public SpriteRenderer page;
    public Dead dead;
    private float beginheight = 0;
    [SerializeField] private TextMeshPro text;
    private float spawnProgress = 0;
    private float originposx;
    private bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        dead.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_TakePaper/SFX_TakePaper_" + Controller.GetRandom(10)));
        originposx = dead.transform.position.x - Camera.main.transform.position.x;
        text.text = dead.text.Replace('#','\n');
        transform.position = new Vector3(0, -20, 0);
        transform.rotation = Quaternion.Euler(0,0,Random.value * 10 -5);
        beginheight = (text.GetTextInfo(text.text).textComponent.GetPreferredValues().y < 5)? text.GetTextInfo(text.text).textComponent.GetPreferredValues().y : 5 ;
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (!dead.active) Destroy(this.gameObject);

        //float axes = Mathf.Lerp(dead.transform.position.x,Camera.main.transform.position.x, 0.2f);


        float axes = Camera.main.transform.position.x;

        

        Vector3 y = new Vector3(0,transform.position.y,0);
        if (spawnProgress < 1f && dead.active && active)
        {
            spawnProgress += Time.deltaTime;
            y = (new Vector3(0, curve.Evaluate(spawnProgress) * 6 - 15 + beginheight, 0));
        }
        else if ((!active || !dead.active) && spawnProgress > 0)
        {
            if(active)dead.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_PutPaperBack/SFX_PutPaperBack_" + Controller.GetRandom(4)));
            active = false;
            text.sortingOrder = -3;
            page.sortingOrder = -4;
        
            spawnProgress -= Time.deltaTime;
            page.color = new Color(1, 1, 1, curve.Evaluate((spawnProgress - 0.2f)*5/4));
            y = (new Vector3(0, curve.Evaluate(spawnProgress) * 6 - 15 + beginheight, 0));
        }
        else if (!active || !dead.active) Destroy(this.gameObject);

        transform.position = new Vector3(axes, 0, 0) + y ;

        /*

        float axesy = Input.mousePosition.y;
        float size = Screen.height;
        float textProgress = 1- axesy / size;
        if (textProgress >= 1) textProgress = 1;
        Debug.Log(textProgress);
        int nbreturn = dead.text.Split('\n').Length;
        string newtext = "";
        int i = nbreturn-1;
        while(i>=0 && i > textProgress * nbreturn)
        {
            newtext = newtext + dead.text.Split('\n')[i];
            --i;
        }
        text.text = newtext;
        */
    }
}
