using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
public class Controller : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private AnimationCurve charCurve;

    private float speedProgress = 0.3f;
    private float progress = 0;
    public InputField field;
    private Vector3 move;
    public string text;
    public bool write = false;
    private float walkheight;
    private float writeheight = -5;
    private string lastText = "";
    public float nbchar = 3;
    public TextMeshProUGUI textchar;
    public float originalpos;
    // Start is called before the first frame update
    void Start()
    {
        walkheight = Camera.main.transform.parent.transform.position.y;
        originalpos = transform.position.x;
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        textchar.SetText((Mathf.Floor(nbchar) - text.Length).ToString());
    }
    public void Move()
    {
        if (!write) {
            field.DeactivateInputField();
            if (progress > 0) progress -= Time.deltaTime;
            //field.ActivateInputField();
            move.x = Input.GetAxis("Horizontal");
            //move.y = Input.GetAxis("Vertical");
            move = move.normalized* speed;
            GetComponent<Animator>().SetBool("Write", false);
            GetComponent<Animator>().SetFloat("Move", move.magnitude);

            if(move.x > 0) nbchar += charCurve.Evaluate((transform.position.x - originalpos)/120) * Time.deltaTime;

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
            if (text.Length > nbchar)
            {
                text = text.Substring(0, (int)Mathf.Floor(nbchar));
                field.text = field.text.Substring(0, text.Length - lastText.Length);
            }
        }

        Vector3 camera = Camera.main.transform.position;
        camera.y = curve.Evaluate(progress*(1/speedProgress)) * writeheight;
        Camera.main.transform.position = camera;


    }



}
