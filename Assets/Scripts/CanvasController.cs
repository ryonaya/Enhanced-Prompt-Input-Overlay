using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Util;

public class CanvasController : MonoBehaviour
{
    
    public static CanvasController instance;
    public CanvasTMPInput canvasInputField;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
