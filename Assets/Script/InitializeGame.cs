using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        Destroy(this.gameObject);
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
            System.DateTime deaddate = System.DateTime.ParseExact(deadinfo.dead, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
           

            (deadObject.GetComponent(typeof(Dead)) as Dead).deadTime = (float)(System.DateTime.Now - deaddate).TotalMinutes;
        }
        

    }
}
