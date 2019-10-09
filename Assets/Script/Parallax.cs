using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float length, startpos;
    public GameObject cam;
    public float parallaxEffect;
    public Parallax part2;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.gameObject;
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos = part2.startpos + part2.length;
        else if (temp < startpos - length) startpos = part2.startpos - length;
    }
}
