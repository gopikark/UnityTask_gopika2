using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


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
// Creating the class

public class SelectPrefab : MonoBehaviour, IPointerClickHandler
{

    
    public Material SphereMaterial2;

    public Material SphereMaterial;
    // public and local variables and Gameobjects
    private string url = "https://cbotdrg92b.execute-api.eu-central-1.amazonaws.com/Digital_library_API/digital-library?x-api-key=KaFn9xEXuw8T2fOzkUa7j8dkrgMlNmQt80epCtSp";
    string jsonAngle;
    public GameObject ThisAcuPointPrefab;
    string AcuPointPreFabName;
    public static string Message;
    public Texture CancelImg;
    public string Cancel;
    // public string Lung_Highlight;
    // While Starting, Check's the name of the GameObject
    GameObject g ;
    GameObject objByName;
    void Start()
    {
        //AcuPointPreFabName = this.gameObject.name;
      //  (GameObject.Find("FocusButton").GetComponent(Cancel) as MonoBehaviour).enabled = false;
        // GameObject.Find("AcuPointImage").GetComponent<RawImage>().enabled = false;
        // g = GameObject.Find("AcuPointImagee");
        // g.gameObject.SetActive(false);
       objByName = FindInActiveObjectByName("AcuPointImagee");
        g = FindInActiveObjectByName("closeimg");
    }


GameObject FindInActiveObjectByName(string name)
{
    Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
    for (int i = 0; i < objs.Length; i++)
    {
        if (objs[i].hideFlags == HideFlags.None)
        {
            if (objs[i].name == name)
            {
                return objs[i].gameObject;
            }
        }
    }
    return null;
}
// Receives the Message from Other class

public void MessagefromCreatePrefab(string ReceivedMessage)
    {
        Message = ReceivedMessage;
        //Debug.Log(Message);
    }

    // OnPointerClick work's like a button for Panel component

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        objByName.gameObject.SetActive(true);
        g.gameObject.SetActive(true);

        var thisname = name;
       // Debug.Log(thisname);
        thisname = thisname.Split(' ')[0];
        Debug.Log("point name "+thisname);
        Selected(thisname, Message);

        GameObject.Find("obname").GetComponentInChildren<Text>().text = thisname;
        Data myAngle = new Data() { Search = "Acupuncture", Specific_part = new string[] { thisname }, Merge = "None" };
        jsonAngle = JsonUtility.ToJson(myAngle);
        StartCoroutine(PostRequest());
        UpdateMaterial();

    }

    void UpdateMaterial()
    {
         var thisname=GameObject.Find("obname").GetComponentInChildren<Text>().text;
        // SphereMaterial = Resources.Load<Material>("Target1");
        System.Random random = new System.Random();
        int num = random.Next(0, 4);
        if (num >= 2)
        {
            Debug.Log("value " + num);
            MeshRenderer meshRenderer = GameObject.Find(thisname+"_Object").GetComponent<MeshRenderer>();
            Debug.Log(meshRenderer.material);
            meshRenderer.material = SphereMaterial;
            GameObject.Find("DetectImg").SetActive(true);
        //    GameObject.Find("detecttext").GetComponentInChildren<Text>().text = "DETECTED";
        //    GameObject.Find("detecttext").GetComponent<Text>().color = new Color32(42, 255, 120, 255);
            GameObject.Find("DetectImg").GetComponent<RawImage>().texture = Resources.Load("Images/Detected") as Texture2D;

        }
        else
        {
            new Color32(248, 88, 88, 255);
            MeshRenderer meshRenderer = GameObject.Find(thisname + "_Object").GetComponent<MeshRenderer>();
            Debug.Log(meshRenderer.material);
            meshRenderer.material = SphereMaterial2;
            Debug.Log("value " + num);
            GameObject.Find("DetectImg").SetActive(true);
          //  GameObject.Find("detecttext").GetComponentInChildren<Text>().text = "NOT DETECTED";
           // GameObject.Find("detecttext").GetComponent<Text>().color = new Color32(248, 88, 88, 255);
            GameObject.Find("DetectImg").GetComponent<RawImage>().texture = Resources.Load("Images/NotDetected") as Texture2D;
        }
    }
    public IEnumerator PostRequest()
    {
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
            string b = request1.downloadHandler.text;
          //  Debug.Log(b);
            var words = b.Split(':');
            //  Debug.Log(words[8]);
            var w = words[8].Split(',');
           // Debug.Log(w[0]);
            w[0] = w[0].Replace("\"", string.Empty);
            GameObject.Find("Angle").GetComponent<Text>().text = w[0];
            //  Debug.Log(words[11]);
            var ww = words[11].Split(',');
          //  Debug.Log(ww[0]);
            ww[0] = ww[0].Replace("\"", string.Empty);
            GameObject.Find("Depth").GetComponent<Text>().text = ww[0];
            var www = words[5].Split('"');
          //  Debug.Log(www[1]);
            GameObject.Find("LocDesciption").GetComponent<Text>().text = www[1];
            ///// words[1] = words[1].Replace("\"", string.Empty);
            // words[1] = words[1].Replace(" ", string.Empty);
            // GameObject.Find("sample").GetComponentInChildren<Text>().text = words[1];

        }
    }

    // Accessing the received message and shared with gameobject to Highlight

    void Selected(string thisname, string AcuPoints)
    {

        String[] separator = { "," };
        List<string> strList = new List<string>(AcuPoints.Split(separator, StringSplitOptions.RemoveEmptyEntries));
        for (int i = 0; i < strList.Count; i++)
        {
            if (strList[i].Equals(thisname))
            {
                if (gameObject.name.Equals(thisname + " List"))
                {
                    HighlightRed(thisname,AcuPoints);
                }
            }
        }
    } 

    // Highlight the gameobject

    void HighlightRed(string thisname,string AcuPoints)
    {
        
        ThisAcuPointPrefab.GetComponent<RawImage>().color = new Color32(166, 255, 159, 255);
        if (GameObject.FindWithTag("AcuPoints"))
        {
            //var ReceivedMessage = ":LooKAtTheObject";
            //GameObject.Find(thisname).SendMessage("SendMsgToAcuPoint", ReceivedMessage);
            Camera.main.SendMessage("msgController", thisname + "Focus");
            Camera.main.SendMessage("CamMsg", "LooKAtTheObject");
        }
        GameObject.Find(thisname + "_Object").GetComponent<Renderer>().enabled = true;
        GameObject.Find(thisname+ "_Text").GetComponent<Renderer>().enabled = true;
        Focus(thisname, AcuPoints);
        String[] separator = { "," };
        List<string> strListB = new List<string>(AcuPoints.Split(separator, StringSplitOptions.RemoveEmptyEntries));
        strListB.Remove(thisname);
        for(int i = 0; i < strListB.Count; i++)
        {
            GameObject.Find(strListB[i] + " List").GetComponent<RawImage>().color = new Color(255, 255, 225);
           // Debug.Log(strListB[i]);
              GameObject.Find(strListB[i] + "_Object").GetComponent<Renderer>().enabled = false;
            GameObject.Find(strListB[i] + "_Text").GetComponent<Renderer>().enabled = false;
         //    GameObject.Find(strListB[i] + "_ObjectLhs").GetComponent<Renderer>().enabled = false;
         //   GameObject.Find(strListB[i] + "_ObjectRhs").GetComponent<Renderer>().enabled = false;
         //     GameObject.Find(strListB[i] + "_TextLhs").GetComponent<Renderer>().enabled = false;
            // GameObject.Find(strListB[i] + "_TextRhs").GetComponent<Renderer>().enabled = false;
        }
    }

    void Focus(string thisname, string AcuPoints)
    {
      //  GameObject.Find("FocusButton").GetComponentInChildren<RawImage>().texture = CancelImg;
        GameObject.Find("AcuPointImage").GetComponent<RawImage>().enabled = true;
        GameObject.Find("AcuPointImage").GetComponent<RawImage>().texture = Resources.Load("Images/"+ thisname) as Texture2D;
     //   GameObject.Find("FocusButton").GetComponentInChildren<Text>().text = "Cancel the Focus";
      //  GameObject.Find("FocusText").GetComponent<RectTransform>().sizeDelta = new Vector2(131.5f, 22.12f);
     //  GameObject.Find("FocusText").GetComponent<RectTransform>().localPosition = new Vector3(-45.91f, -1.525879e-05f,0);
        (GameObject.Find("closeimg").GetComponent(Cancel) as MonoBehaviour).enabled = true;
        GameObject.Find("closeimg").SendMessage("ReceivedMessagefromSelectPrefab", thisname);
        GameObject.Find("closeimg").SendMessage("ReceivedAcuPoint", AcuPoints);
    }
 }
