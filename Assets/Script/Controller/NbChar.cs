using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NbChar : MonoBehaviour
{
    public Controller player;
    [SerializeField] public AnimationCurve charCurve;
    public float number = 3;
    private TextMeshProUGUI textchar;
    public float originalpos;

    // Start is called before the first frame update
    void Start()
    {
        textchar = GetComponent<TextMeshProUGUI>();
        originalpos = player.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        int nbcharrestant = Mathf.RoundToInt(Mathf.Floor(number) - player.text.Length);
        textchar.SetText(nbcharrestant.ToString());
    }
}
