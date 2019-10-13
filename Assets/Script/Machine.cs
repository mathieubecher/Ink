using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    public float speedProgress = 0.3f;
    public float progress = 0;

    private float machinheight = -5;
    private float originmachinheight;
    public float walkheight;
    private float writeheight = -4;
    // Start is called before the first frame update
    void Start()
    {
        originmachinheight = transform.position.y;
        walkheight = Camera.main.transform.parent.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camera = Camera.main.transform.position;
        camera.y = curve.Evaluate(progress * (1 / speedProgress)) * writeheight;

        Vector3 machinpos = transform.position;
        machinpos.y = curve.Evaluate(1 - progress * (1 / speedProgress)) * machinheight + originmachinheight;

        Camera.main.transform.position = camera;
        transform.position = machinpos;
    }
}
