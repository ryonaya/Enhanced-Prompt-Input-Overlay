using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

/// <summary>
/// Attached to Button Holder
/// Handle button events and its logic
/// </summary>
public class ButtonHandler : MonoBehaviour
{
    
    public RectTransform addNodeButtonRect;
    private HorizontalLayoutGroup _addNodeButtonLayout;
    public GameObject node;

    private void Start()
    {
        _addNodeButtonLayout = addNodeButtonRect.GetComponent<HorizontalLayoutGroup>();
    }

    public void AddNode()
    {
        // Don't add node if there is already one
        if (addNodeButtonRect.childCount > 1) return;
        
        GameObject newNode = Instantiate(node, Vector3.zero, Quaternion.identity, addNodeButtonRect);
        newNode.transform.SetSiblingIndex(addNodeButtonRect.GetSiblingIndex());

        // Re-calculates layout group
        StartCoroutine(AddNodeCoroutine(newNode));
    }
    
    private IEnumerator AddNodeCoroutine(GameObject newNode)
    {
        yield return null;
        
        // Trigger edit mode
        newNode.GetComponentInChildren<TMPSizeSync>().OnSelect();
        
        // Re-calculates layout group to put new node next to the add node button
        _addNodeButtonLayout.CalculateLayoutInputHorizontal();
        _addNodeButtonLayout.SetLayoutHorizontal();
    }
}
