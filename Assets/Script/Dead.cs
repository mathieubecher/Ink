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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deadTime += Time.deltaTime/(float)(60);
        if(cursor != null)
        {
            if (cursor.Distance(GetComponent<Collider2D>()).distance > 0)
            {
                
                cursor = null;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (!active)
                {
                    currentdead = this;
                    GameObject g = Instantiate((GameObject)Resources.Load("Paper"),Vector3.zero,Quaternion.identity);
                    (g.GetComponent(typeof(DeadPaper)) as DeadPaper).dead = this;
                    active = true;
                }
                
            }
        }
        if (active)
        {
            if (currentdead != this) active = false;
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
