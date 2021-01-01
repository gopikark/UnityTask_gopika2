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

public class Detect : MonoBehaviour, IPointerClickHandler
{
    public Material SphereMaterial;
    // Start is called before the first frame update
    void Start()
    {
       // SphereMaterial = Resources.Load<Material>("Target1");
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        // Get the current material applied on the GameObject
        Material oldMaterial = meshRenderer.material;
        Debug.Log("Applied Material: " + oldMaterial.name);
        // Set the new material on the GameObject
        // meshRenderer.material = SphereMaterial;
        UpdateMaterial();
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        UpdateMaterial();
    }

    // Update is called once per frame
    void UpdateMaterial()
    {
        // SphereMaterial = Resources.Load<Material>("Target1");
        System.Random random = new System.Random();
        int num = random.Next(0, 4);
        if (num >= 2)
        {
            Debug.Log("value " + num);
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = SphereMaterial;

        }
        else
        {
            Debug.Log("value " + num);
        }
    }
}
