using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private AnimationCurve charCurve;
    [SerializeField] private GameObject machin;

    private float speedProgress = 0.3f;
    private float progress = 0;
    private float machinheight = -5;
    private float originmachinheight;
    public InputField field;
    private Vector3 move;
    public string text;
    public bool write = false;
    private float walkheight;
    private float writeheight = -4;
    private string lastText = "";
    public float nbchar = 3;
    public int nbcharrestant;
    public TextMeshProUGUI textchar;
    public float originalpos;
    public SpriteRenderer ombre;
    private float deadCount;
    private float MAXDEADCOUNT = 5;


    public float deadprogress;
    public bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        deadCount = MAXDEADCOUNT;
        walkheight = Camera.main.transform.parent.transform.position.y;
        originalpos = transform.position.x;
        originmachinheight = machin.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        nbcharrestant = Mathf.RoundToInt(Mathf.Floor(nbchar) - text.Length);
        textchar.SetText(nbcharrestant.ToString());
    }
    public void Move()
    {
        if (!dead)
        {
            if (!write)
            {
                field.DeactivateInputField();
                if (progress > 0) progress -= Time.deltaTime;
                //field.ActivateInputField();
                move.x = Input.GetAxis("Horizontal");
                if (move.x < 0)
                {

                    GetComponent<Animator>().SetBool("Bloc", true);
                }
                else
                {
                    //move.y = Input.GetAxis("Vertical");
                    move = move.normalized * speed;
                    GetComponent<Animator>().SetBool("Write", false);
                    GetComponent<Animator>().SetFloat("Move", move.magnitude);
                    GetComponent<Animator>().SetBool("Bloc", false);

                    if (move.x > 0) nbchar += charCurve.Evaluate((transform.position.x - originalpos) / 120) * Time.deltaTime;

                    Vector3 campos = Camera.main.transform.parent.position;
                    campos.x = transform.position.x + 2.5f;
                    campos.y = walkheight;
                    Camera.main.transform.parent.transform.position = campos;
                    GetComponent<Rigidbody2D>().velocity = move;

                    if (Input.GetKeyDown(KeyCode.Tab)) write = true;

                    if (Input.GetKey(KeyCode.M))
                    {
                        deadCount -= Time.deltaTime;
                        if(deadCount < 0 && !dead) StartCoroutine(Exit());
                    }
                    else
                    {
                        deadCount = MAXDEADCOUNT;
                    }
                }
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
                    field.text = "";
                    write = false;
                }
                else if (Input.GetKeyDown(KeyCode.Tab))
                {
                    lastText = text;
                    field.text = "";
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
            camera.y = curve.Evaluate(progress * (1 / speedProgress)) * writeheight;

            Vector3 machinpos = machin.transform.position;
            machinpos.y = curve.Evaluate(1 - progress * (1 / speedProgress)) * machinheight + originmachinheight;

            Camera.main.transform.position = camera;
            machin.transform.position = machinpos;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            deadprogress -= Time.deltaTime;
            if(deadprogress < 0)
                SceneManager.LoadScene(0);
        }
        


    }
    
    public IEnumerator Exit()
    {
        dead = true;
        if (text != "") { 
            string textParse = text.Replace('"',' ');
            string time = TwoChar(System.DateTime.Now.Day) + "/" + TwoChar(System.DateTime.Now.Month) + "/" + System.DateTime.Now.Year + " " + TwoChar(System.DateTime.Now.Hour)+ ":" + TwoChar(System.DateTime.Now.Minute) + ":" + TwoChar(System.DateTime.Now.Second);
            UnityWebRequest www = UnityWebRequest.Get("http://portfoliobecher.com/Ink/SetDead.php?time=" + time + "&position=" + transform.position.x + "&text=" + textParse);
            yield return www.SendWebRequest();
        }
        
        deadprogress = 2;
        GetComponent<Animator>().SetBool("Dead", true);
    }
    
    public string TwoChar(int value)
    {
        string tochar = ""+ value;
        if (value < 10) tochar = "0" + tochar;
        return tochar;
    }

}
