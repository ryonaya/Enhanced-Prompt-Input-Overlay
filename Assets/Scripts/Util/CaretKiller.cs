using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaretKiller : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        DestroyImmediate( GameObject.Find("Caret"));
    }
}
