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
    private float timebetweenstep=0;
    private bool leftstep = false;
    

    public string text;
    private string lastText = "";
    public bool write = false;
    public float nbchar = 3;
    public int nbcharrestant;
    public TextMeshProUGUI textchar;
    public float originalpos;
    public SpriteRenderer perso;
    public SpriteMask mask;
    public SpriteRenderer ombre;
    private float walkheight;
    private float writeheight = -4;
    public float deadCount;
    private float MAXDEADCOUNT = 5;
    private float deadAnim = 1;
    private bool finishrequest = false;
    private bool dead = false;

    public Animator cameradeath;
    public Animator vignette;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        deadCount = MAXDEADCOUNT;
        walkheight = Camera.main.transform.parent.transform.position.y;
        originalpos = transform.position.x;
        originmachinheight = machin.transform.position.y;
        audio = GetComponent<AudioSource>();
        
    }
    public static string GetRandom(int maxvalue)
    {
        string number = "";
        int rand = (int)Mathf.Floor((Random.value) * maxvalue + 1);
        if (rand < 10) number += "0";
        return number + rand;
    }
    // Update is called once per frame
    void Update()
    {
        Move();
        nbcharrestant = Mathf.RoundToInt(Mathf.Floor(nbchar) - text.Length);
        textchar.SetText(nbcharrestant.ToString());
        ombre.color = new Color(1, 1, 1, (nbcharrestant / 250.0f));
        mask.sprite = perso.sprite;
    }
    

    public void Move()
    {
        if (!finishrequest)
        {
            if (!write)
            {
                field.DeactivateInputField();
                
                    if (Input.GetKey(KeyCode.M))
                    {
                        GetComponent<Animator>().SetBool("Write", false);
                        GetComponent<Animator>().SetFloat("Move", 0);
                        GetComponent<Animator>().SetBool("Bloc", false);
                        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        if (deadCount == MAXDEADCOUNT) audio.PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_Die/SFX_Die"));
                        deadCount -= Time.deltaTime;
                        cameradeath.SetFloat("compteur", MAXDEADCOUNT - deadCount);
                        cameradeath.enabled = true;
                        GetComponent<Animator>().SetFloat("CompteurMort", MAXDEADCOUNT - deadCount);
                        vignette.SetFloat("compteur", MAXDEADCOUNT - deadCount);
                        if (deadCount < 0 && !dead) StartCoroutine(Exit());
                    }
                    else
                    {
                        if (progress > 0) progress -= Time.deltaTime;
                        //field.ActivateInputField();
                        move.x = Input.GetAxis("Horizontal");
                        if (move.x < 0)
                        {
                            if (!GetComponent<Animator>().GetBool("Bloc"))
                                audio.PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_NoBack/SFX_NoBack_" + GetRandom(4)));
                            GetComponent<Animator>().SetBool("Bloc", true);

                        }
                        else
                        {
                            //move.y = Input.GetAxis("Vertical");
                            move = move.normalized * speed;
                            GetComponent<Animator>().SetBool("Write", false);
                            GetComponent<Animator>().SetFloat("Move", move.magnitude);
                            GetComponent<Animator>().SetBool("Bloc", false);

                            if (move.x > 0)
                                timebetweenstep += Time.deltaTime * 2;
                            if (timebetweenstep > 1)
                            {
                                timebetweenstep = 0;
                                leftstep = !leftstep;

                                audio.PlayOneShot((AudioClip)Resources.Load("Sound/Footsteps/FTS_Full/FTS_" + ((leftstep) ? "Left_" : "Right_") + GetRandom(7)));
                            }


                            if (move.x > 0)
                            {

                                nbchar += charCurve.Evaluate((transform.position.x - originalpos) / 120) * Time.deltaTime;
                            }

                            Vector3 campos = Camera.main.transform.parent.position;
                            campos.x = transform.position.x + 2.5f;
                            campos.y = walkheight;
                            Camera.main.transform.parent.transform.position = campos;
                            GetComponent<Rigidbody2D>().velocity = move;

                            if (Input.GetKeyDown(KeyCode.Tab) && cameradeath.GetCurrentAnimatorStateInfo(0).IsName("Camera"))
                            {
                                
                                audio.PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_TakeTypewriter/SFX_TakeTypewriter_" + Controller.GetRandom(4)));
                                write = true;
                                cameradeath.enabled = false;

                            }
                            if (deadCount < MAXDEADCOUNT) audio.Stop();
                            deadCount = MAXDEADCOUNT;
                            cameradeath.SetFloat("compteur", -0.1f);
                            GetComponent<Animator>().SetFloat("CompteurMort", -0.1f);
                            vignette.SetFloat("compteur", -0.1f);
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
                    audio.PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_AwayTypewriter/SFX_AwayTypewriter_" + Controller.GetRandom(4)));
                    lastText = text;
                    field.text = "";
                    write = false;

                }
                else
                {
                    string oldtext = text;
                    text = lastText + field.text;
                    if (oldtext.Length < text.Length)
                    {
                        if(text.Length <= nbchar)
                        audio.PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_Typewiter/SFX_Key/SFX_Typewriter_Key_" + Controller.GetRandom(13)));
                    }
                }
                if (text.Length > nbchar)
                {
                    audio.PlayOneShot((AudioClip)Resources.Load("Sound/SFX/SFX_Typewiter/SFX_Spool/SFX_Typewriter_Spool_" + Controller.GetRandom(4)));
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
            deadAnim -= Time.deltaTime;
            if(deadAnim < 0 && finishrequest)
                SceneManager.LoadScene(0);
        }
        


    }

    public IEnumerator Exit()
    {
        Debug.Log("Begin Dead");
        dead = true;
        if (text != "")
        {

            string textParse = text.Replace('"', ' ');
            string time = TwoChar(System.DateTime.Now.Day) + "/" + TwoChar(System.DateTime.Now.Month) + "/" + System.DateTime.Now.Year + " " + TwoChar(System.DateTime.Now.Hour) + ":" + TwoChar(System.DateTime.Now.Minute) + ":" + TwoChar(System.DateTime.Now.Second);

            WWWForm form = new WWWForm();
            form.AddField("position", "" + transform.position.x);
            form.AddField("text", textParse);
            form.AddField("time", time);

            Debug.Log("Begin Save");
            Debug.Log(textParse);
            UnityWebRequest www = UnityWebRequest.Post("http://portfoliobecher.com/Ink/SetDead.php", form);

            yield return www.SendWebRequest();
            while (!www.isDone)
            {
                Debug.Log("j'attend");
            }
            Debug.Log(www.downloadHandler.text);
            Debug.Log("Save OK");

        }
        deadAnim = 2;

        finishrequest = true;
        GetComponent<Animator>().SetBool("Dead", true);
    }

    public string TwoChar(int value)
    {
        string tochar = ""+ value;
        if (value < 10) tochar = "0" + tochar;
        return tochar;
    }

}
