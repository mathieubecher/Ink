﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class DeadInfo
{
    public string dead;
    public float position;
    public string text;
}
[System.Serializable]
public class ListaDead
{
    public List<DeadInfo> Deads;
}

public class InitializeGame : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject dead;
    public float miny =-1.9f;
    public float maxy =-5f;
    void Start()
    {
        StartCoroutine(Register());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Register()
    {
        
        UnityWebRequest www = UnityWebRequest.Get("http://portfoliobecher.com/Ink/GetDead.php");
        yield return www.SendWebRequest();
        string command = www.downloadHandler.text;

        ListaDead json = JsonUtility.FromJson<ListaDead>(command); 

        foreach(DeadInfo deadinfo in json.Deads)
        {
            GameObject deadObject = Instantiate(dead,new Vector3(deadinfo.position, (Random.value * (maxy -miny)) + miny, 0), Quaternion.identity);
            (deadObject.GetComponent(typeof(Dead)) as Dead).text = deadinfo.text;
            Debug.Log(deadinfo.dead );
            System.DateTime deaddate = System.DateTime.ParseExact(deadinfo.dead, "yyyy-dd-MM hh:mm:ss",null);

            //(deadObject.GetComponent(typeof(Dead)) as Dead).deadTime = (System.DateTime.Now - deaddate).Minutes;
            //Debug.Log(System.DateTime.Now);
            //Debug.Log(deadinfo.dead + " " + deaddate);
            Debug.Log((deadObject.GetComponent(typeof(Dead)) as Dead).deadTime);
        }
        

    }
}
