using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : MonoBehaviour
{
    private Collider2D cursor;
    [SerializeField] private DeadPaper paper;
    private void Update()
    {
        if (cursor != null)
        {
            if (cursor.Distance(GetComponent<Collider2D>()).distance > 0)
            {
                cursor = null;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                paper.dead.active = false;
            }
        }
        if (!paper.dead.active) GetComponent<SpriteRenderer>().sortingOrder = -2;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cursor == null)
        {
            cursor = collision;
        }
    }
}
