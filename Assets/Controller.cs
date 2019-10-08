using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    private Vector3 move;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    public void Move()
    {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");
        Vector3 campos = Camera.main.transform.position;
        campos.x = transform.position.x;
        Camera.main.transform.position = campos;
        GetComponent<Rigidbody2D>().velocity = move.normalized * speed;
    }
}
