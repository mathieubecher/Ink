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
    // Start is called before the first frame update
    void Start()
    {
        originposx = dead.transform.position.x - Camera.main.transform.position.x;
        text.text = dead.text.Replace('#','\n');
        transform.position = new Vector3(0, -20, 0);
        transform.rotation = Quaternion.Euler(0,0,Random.value * 10 -5);
        Vector2 size = new Vector2(text.GetComponent<TextMeshPro>().GetComponent<Renderer>().bounds.size.x, text.GetComponent<TextMeshPro>().GetComponent<Renderer>().bounds.size.y);
        beginheight = text.GetTextInfo(text.text).textComponent.GetPreferredValues().y;
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (!dead.active) Destroy(this.gameObject);

        //float axes = Mathf.Lerp(dead.transform.position.x,Camera.main.transform.position.x, 0.2f);
        float axes = Camera.main.transform.position.x;
        Vector3 y = new Vector3(0,transform.position.y,0);
        if (spawnProgress < 1f && dead.active)
        {
            spawnProgress += Time.deltaTime;
            y = (new Vector3(0, curve.Evaluate(spawnProgress) * 6 - 15 + beginheight, 0));
        }
        else if (!dead.active && spawnProgress > 0)
        {
            text.sortingOrder = -2;
            page.sortingOrder = -3;
        
            spawnProgress -= Time.deltaTime;
            page.color = new Color(1, 1, 1, curve.Evaluate((spawnProgress - 0.2f)*5/4));
            y = (new Vector3(0, curve.Evaluate(spawnProgress) * 6 - 15 + beginheight, 0));
        }
        else if (!dead.active) Destroy(this.gameObject);

        transform.position = new Vector3(axes, 0, 0) + y ;
       
    }
}
