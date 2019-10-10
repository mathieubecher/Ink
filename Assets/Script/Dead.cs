using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour
{
    private static Dead currentdead;
    private Collider2D cursor;
    public float deadTime;
    public string text;
    public bool active = false;
    [SerializeField] private List<Sprite> listdeadSprite;
    // Start is called before the first frame update
    void Start()
    {
        if (deadTime > 60 * 24) Destroy(this.gameObject);
        
        int id = listdeadSprite.Count - 1;
        
        if ((int)Mathf.Floor(listdeadSprite.Count * deadTime / (60 * 12)) < listdeadSprite.Count)
            id = (int)Mathf.Floor(listdeadSprite.Count * deadTime / (60 * 12));
        if (id < 0) id = 0;
        GetComponent<SpriteRenderer>().sprite = listdeadSprite[id];
    }

    // Update is called once per frame
    void Update()
    {
        deadTime += Time.deltaTime/(float)(60);
        if(cursor != null)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
            if (cursor.Distance(GetComponent<Collider2D>()).distance > 0)
            {
                
                cursor = null;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (!active)
                {
                    GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_TakePaper/SFX_TakePaper_" + Controller.GetRandom(10)));
                    Debug.Log("Sound/SFX/SFX_TakePaper/SFX_TakePaper_" + Controller.GetRandom(10));
                    currentdead = this;
                    GameObject g = Instantiate((GameObject)Resources.Load("Paper"),Vector3.zero,Quaternion.identity);
                    (g.GetComponent(typeof(DeadPaper)) as DeadPaper).dead = this;
                    active = true;
                }       
                
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1,1,1, 1);
        }
        if (active)
        {
            if (currentdead != this)
            {
                Debug.Log("plus active");
                
                active = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cursor == null)
        {
            cursor = collision;
        }
            
    }
}
