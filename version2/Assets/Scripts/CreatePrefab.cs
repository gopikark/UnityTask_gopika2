using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;
//using Newtonsoft.Json;

using System;
//using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;



[Serializable]
public class Data
{


    public string Search;


    public string[] Specific_part;


    public string Merge;
}


public class CreatePrefab : MonoBehaviour
{
    private string url = "https://cbotdrg92b.execute-api.eu-central-1.amazonaws.com/Digital_library_API/digital-library?x-api-key=KaFn9xEXuw8T2fOzkUa7j8dkrgMlNmQt80epCtSp";
    string jsonData, jsonAngle, liv = "LIV10";
    public GameObject prefab;
    public GameObject other;
    GameObject baseObject;
    // Start is called before the first frame update
    void Start()
    {
        liv = GameObject.Find("obname").GetComponentInChildren<Text>().text;

        init();
        //  OnMessage("LU1,LU2,LU3,LU4,LU5,LU6");
    }

    void init()
    {


        Data myAngle = new Data() { Search = "Acupuncture", Specific_part = new string[] { liv }, Merge = "None" };

        Data mydata = new Data() { Search = "All", Specific_part = new string[] { "Acupoints" }, Merge = "None" };
        // Debug.Log(mydata);
        jsonData = JsonUtility.ToJson(mydata);
        jsonAngle = JsonUtility.ToJson(myAngle);


        //  Debug.Log(jsonData);
        // Debug.Log(jsonAngle);
        StartCoroutine(PostRequest());
    }


    public IEnumerator PostRequest()
    {




        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("x-api-key", "KaFn9xEXuw8T2fOzkUa7j8dkrgMlNmQt80epCtSp");
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        if (request.isNetworkError)
        {
            //  Debug.Log("Something went wrong, and returned error: " + request.error);
            // requestErrorOccurred = true;
        }
        else
        {
            //  Debug.Log("Response: " + request.downloadHandler.text);
            string a = request.downloadHandler.text;
            //  Debug.Log(a);
            var words = a.Split(new char[] { '[', ']' });
            // Debug.Log(words[1]);
            words[1] = words[1].Replace("\"", string.Empty);
            words[1] = words[1].Replace(" ", string.Empty);
            //OnMessage(words[1]);
            OnMessage("LU1,LU2");

        }



        var request1 = new UnityWebRequest(url, "POST");
        byte[] bodyRaw1 = new System.Text.UTF8Encoding().GetBytes(jsonAngle);
        request1.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw1);
        request1.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request1.SetRequestHeader("x-api-key", "KaFn9xEXuw8T2fOzkUa7j8dkrgMlNmQt80epCtSp");
        request1.SetRequestHeader("Content-Type", "application/json");

        yield return request1.SendWebRequest();
        //Debug.Log(request1);
        if (request1.isNetworkError)
        {
            Debug.Log("Something went wrong, and returned error: " + request1.error);
            // requestErrorOccurred = true;
        }
        else
        {
          //  Debug.Log("Response: " + request1.downloadHandler.text);
           // string b = request1.downloadHandler.text;
          //  Debug.Log(b);
          //  var words = b.Split(':');
           // Debug.Log(words[8]);
          //  var w = words[8].Split(',');
           // Debug.Log(w[0]);
            ///// words[1] = words[1].Replace("\"", string.Empty);
            // words[1] = words[1].Replace(" ", string.Empty);
            // GameObject.Find("sample").GetComponentInChildren<Text>().text = words[1];

        }
    }
    void OnMessage(string Message)
    {
        String[] separator = { "," };
        IList<string> strList = new List<string>(Message.Split(separator, StringSplitOptions.RemoveEmptyEntries));
        int j = -400;
        for (int i = 0; i < strList.Count; i++)
        {

            GameObject Point = Instantiate(prefab, new Vector3(j, 0, 0), transform.rotation) as GameObject;
            Point.transform.SetParent(other.transform, false);
            Point.name = strList[i] + " List";
            Point.GetComponentInChildren<Text>().text = strList[i];
            //  Debug.Log(strList[i]);
            j += 180 + 20;

            if (GameObject.FindWithTag("AcuPointPrefab"))
            {
                GameObject.Find(strList[i] + " List").SendMessage("MessagefromCreatePrefab", Message);
                //   Debug.Log("Msg Sent to :" + strList[i] + " List");
                //  Debug.Log("The message is "+Message);
            }
        }
        strList.Clear();

        // Update is called once per frame
        void Update()
        {
            liv = GameObject.Find("obname").GetComponentInChildren<Text>().text;
            Debug.Log("Stering " + liv);
            Data myAngle = new Data() { Search = "Acupuncture", Specific_part = new string[] { liv }, Merge = "None" };
            jsonAngle = JsonUtility.ToJson(myAngle);
            StartCoroutine(PostRequest());

        }
    }

}

