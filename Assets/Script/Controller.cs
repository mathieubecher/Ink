using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Controller : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private AnimationCurve curve;

    private float speedProgress = 0.3f;
    private float progress = 0;
    public InputField field;
    private Vector3 move;
    public string text;
    public bool write = false;
    private float walkheight;
    private float writeheight = -5;
    private string lastText = "";
    // Start is called before the first frame update
    void Start()
    {
        walkheight = Camera.main.transform.parent.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    public void Move()
    {
        if (!write) {
            field.DeactivateInputField();
            if (progress > 0) progress -= Time.deltaTime;
            //field.ActivateInputField();
            move.x = Input.GetAxis("Horizontal");
            move.y = Input.GetAxis("Vertical");
            move = move.normalized* speed;
            GetComponent<Animator>().SetBool("Write", false);
            GetComponent<Animator>().SetFloat("Move", move.magnitude);
            Vector3 campos = Camera.main.transform.parent.position;
            campos.x = transform.position.x + 2.5f;
            campos.y = walkheight;
            Camera.main.transform.parent.transform.position = campos;
            GetComponent<Rigidbody2D>().velocity = move;
            if (Input.GetKeyDown(KeyCode.Space)) write = true;
        }
        else
        {
            GetComponent<Animator>().SetBool("Write", true);
            if (progress < speedProgress) progress += Time.deltaTime;
            field.ActivateInputField();

            GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                lastText = text;
                write = false;
            }
            else
            {
                text = lastText + field.text;
            }
        }

        Vector3 camera = Camera.main.transform.position;
        camera.y = curve.Evaluate(progress*(1/speedProgress)) * writeheight;
        Camera.main.transform.position = camera;
    }
}
