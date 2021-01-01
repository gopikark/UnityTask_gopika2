using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class toggleee : MonoBehaviour
{
    public GameObject img1;
    public GameObject img2;

    void Update()
    {
      //  changeimg();
    }
    public void changeimg()

    {

        if (img1.activeInHierarchy == true)
        {
            Debug.Log("clicked1");
            img1.SetActive(false);
            img2.SetActive(true);


            //popup
        }
        else
        {
            img1.SetActive(true);
            img2.SetActive(false);
        }
    }
   
}
