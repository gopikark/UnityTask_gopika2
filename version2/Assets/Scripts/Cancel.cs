using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cancel : MonoBehaviour, IPointerClickHandler
{
    public bool ButtonName = false;
    public GameObject CancelButton;
    public static string Message;
    public Texture FocusImg;
    public string CancelScript;
    public static string Msg;
    public static string AcuPoints;
    //public GameObject cam2;


    public void ReceivedMessagefromSelectPrefab(string ReceivedMessage)
    {
        Msg = ReceivedMessage + " List";
        Debug.Log("message  "+ Msg);
    }

    public void ReceivedAcuPoint(string AcuPoint)
    {
        AcuPoints = AcuPoint;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (CancelButton.name == "Cancel")
        {
            ButtonName = true;
        }

        Selected(Msg, AcuPoints);
    }

    private void Selected(string ReceivedMessage, string AcuPoints)
    {
       // GameObject.Find("AcuPointImage").GetComponent<RawImage>().enabled = false;
        Debug.Log("Called: " + AcuPoints);
        Debug.Log("ReceivedMessage: " + ReceivedMessage);
        GameObject.Find(Msg).GetComponent<RawImage>().color = new Color(255, 255, 225);
      //  GameObject.Find("FocusImage").GetComponent<RawImage>().texture = FocusImg;
       // GameObject.Find("FocusText").GetComponent<Text>().text = "Press the AcuPoint to Focus on it.";
    //    GameObject.Find("FocusText").GetComponent<RectTransform>().sizeDelta = new Vector2(236.7f, 22.12f);
       // GameObject.Find("FocusText").GetComponent<RectTransform>().localPosition = new Vector3(6.349899f, 0, 0);
        (this.gameObject.GetComponent(CancelScript) as MonoBehaviour).enabled = false;
        //Camera.main.transform.localPosition = new Vector3(0.04967863f, 2.019993f, 10.3464f);
        //Camera.main.transform.localRotation = Quaternion.Euler(0f, 179.542f, 0f);
        GameObject.Find("MainCamera").SendMessage("CamMsg", "Cancel");
        GameObject.Find("MainCamera").SendMessage("msgController", "Cancel");
        String[] separator = { "," };
        List<string> strList = new List<string>(AcuPoints.Split(separator, StringSplitOptions.RemoveEmptyEntries));
        for (int i = 0; i < strList.Count; i++)
        {
          //  Debug.Log(strList[i]);
            GameObject.Find(strList[i] + "_Object").GetComponent<Renderer>().enabled = true;
            GameObject.Find(strList[i] + "_Text").GetComponent<Renderer>().enabled = true;
        }
    }
}
