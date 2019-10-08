using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WritePaper : MonoBehaviour
{
    public Controller player;
    private float beginheight = 0;
    [SerializeField] private TextMeshPro text;
    private float spawnProgress = 0;
    float originpos;
    // Start is called before the first frame update
    void Start()
    {
        originpos = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = player.text;
        float height = text.GetTextInfo(text.text).textComponent.GetPreferredValues().y;
        transform.localPosition = new Vector3(transform.localPosition.x, originpos + height,0);
    }
}
