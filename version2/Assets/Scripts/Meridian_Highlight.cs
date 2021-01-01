using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meridian_Highlight : MonoBehaviour
{
    string obj_name;
    public GameObject AcuPoint;
    public GameObject AcuPointObject;
    public GameObject AcuPointText;
    public static string MSG;
    //public GameObject cam1;

    void Start()
    {
        obj_name = this.gameObject.name;
        AcuPoint.GetComponent<Renderer>().enabled = false;
        AcuPointObject.GetComponent<Renderer>().enabled = false;
        AcuPointText.GetComponent<Renderer>().enabled = false;
        OnMessage("DU1,DU2,DU3,DU4,DU5,DU6,DU7,DU8,DU9,DU10,DU11,DU12,DU13,DU14,DU15,DU16,DU17,DU18,DU19,DU20,DU21,DU22,DU23,DU24,DU25,DU26,DU27");
        //Debug.Log(obj_name);
    }
    //void Update()
    //{
    //    OnMessage("LU1,LU2,LU3,LU4,LU5,LU6,LU7,LU8,LU9,LU10,LU11");
    //}

    public void GetTheMessage(string Message)
    {
        //Debug.Log("Message Received!!!! :" + Message);
        OnMessage(Message);
    }

    void OnMessage(string Message)
    {
        String[] separator = { "," };
        String[] strlist = Message.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strlist.Length; i++)
        {
            if (strlist[i].Equals(obj_name))
            {
                if (gameObject.name.Equals(obj_name))
                {
                    HighlightRed();
                }
            }
        }

    }

    void HighlightRed()
    {
        AcuPoint.GetComponent<Renderer>().enabled = true; // true to show
        AcuPointObject.GetComponent<Renderer>().enabled = true; // true to show
        AcuPointText.GetComponent<Renderer>().enabled = true; // true to show
    }
}
