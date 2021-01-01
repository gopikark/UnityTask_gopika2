using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class close : MonoBehaviour
{
    public GameObject g;
    public GameObject c;
    // Start is called before the first frame update
   public void closepanel()
    {
        g.gameObject.SetActive(false);
        c.gameObject.SetActive(false);
    }
}
