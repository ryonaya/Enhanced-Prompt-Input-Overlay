using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public RectTransform groupHolderRect;
    public GameObject addGroupInputs;
    private TMP_InputField _groupNameInput;
    private ColorPicker _colorPicker;
    public GameObject group;
    
    private OnGroupParentRemoved _onGroupParentRemoved;
    
    private void Start()
    {
        _addNodeButtonLayout = addNodeButtonRect.GetComponent<HorizontalLayoutGroup>();
        _groupNameInput = addGroupInputs.GetComponentInChildren<TMP_InputField>();
        _colorPicker = addGroupInputs.GetComponentInChildren<ColorPicker>();
        _groupNameInput.onEndEdit.AddListener(AddGroup);
        addGroupInputs.SetActive(false);
        
        _onGroupParentRemoved = groupHolderRect.GetComponent<OnGroupParentRemoved>();
    }

    public void AddNode()
    {
        // Don't add node if there is already one
        // if (addNodeButtonRect.childCount > 1) return;
        
        // Attach node to the last group
        Transform lastGroupHolder = groupHolderRect.GetChild(groupHolderRect.childCount - 1);
        Transform lastGroup = lastGroupHolder.GetChild(1);
        GameObject newNode = Instantiate(node, Vector3.zero, Quaternion.identity, lastGroup);
        newNode.transform.SetSiblingIndex(lastGroup.GetSiblingIndex());

        // Re-calculates layout group
        StartCoroutine(AddNodeCoroutine(newNode));
        
        // Proc resize event
        lastGroup.GetComponent<AutoExtendGroup>().Proc();
        _onGroupParentRemoved.Proc();
    }
    
    private IEnumerator AddNodeCoroutine(GameObject newNode)
    {
        yield return new WaitForEndOfFrame();
        
        // Trigger edit mode
        // newNode.GetComponentInChildren<TMPSizeSync>().OnSelect();
        
        // Re-calculates layout group to put new node next to the add node button
        _addNodeButtonLayout.CalculateLayoutInputHorizontal();
        _addNodeButtonLayout.SetLayoutHorizontal();
    }
    
    public void OpenAddGroupInputs()
    {
        addGroupInputs.SetActive(true);
        _groupNameInput.Select();
    }

    // OnEndEdit
    private void AddGroup(string groupName)
    {
        if (string.IsNullOrEmpty(groupName)) return;
        
        GameObject newGroup = Instantiate(group, Vector3.zero, Quaternion.identity, groupHolderRect);
        newGroup.transform.SetSiblingIndex(groupHolderRect.childCount);
        newGroup.transform.GetChild(1).GetComponent<Image>().color = _colorPicker.color;
        newGroup.GetComponentInChildren<TMP_Text>().text = groupName;
        
        _groupNameInput.text = "";
        addGroupInputs.SetActive(false);

        StartCoroutine(ProcNextFrame());
    }
    
    private IEnumerator ProcNextFrame()
    {
        yield return new WaitForFixedUpdate();
        _onGroupParentRemoved.Proc();
    }
}
