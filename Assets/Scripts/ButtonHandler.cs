using System;
using System.Collections;
using System.Collections.Generic;
using Proc;
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
    public BoxCollider2D addGroupCollider;

    public TMP_InputField importText;

    private Camera _main;
    private OnGroupParentRemoved _onGroupParentRemoved;

    private void Start()
    {
        _addNodeButtonLayout = addNodeButtonRect.GetComponent<HorizontalLayoutGroup>();
        _groupNameInput = addGroupInputs.GetComponentInChildren<TMP_InputField>();
        _colorPicker = addGroupInputs.GetComponentInChildren<ColorPicker>();
        addGroupInputs.SetActive(false);
        importText.gameObject.SetActive(false);
        
        _main = Camera.main;

        _onGroupParentRemoved = groupHolderRect.GetComponent<OnGroupParentRemoved>();
    }

    #region Add Node
    private GameObject AddNode(bool toProc = true)
    {
        // if no group, add group
        if (groupHolderRect.childCount == 0)
        {
            AddGroup("Default");
        }
        
        // Attach node to the last group
        Transform lastGroupHolder = groupHolderRect.GetChild(groupHolderRect.childCount - 1);
        Transform lastGroup = lastGroupHolder.GetChild(1);
        GameObject newNode = Instantiate(node, Vector3.zero, Quaternion.identity, lastGroup);

        // Proc resize event
        if (toProc)
        {
            lastGroup.GetComponent<AutoExtendGroup>().Proc();
            _onGroupParentRemoved.Proc();
        }
        
        return newNode;
    }
    
    public void AddNodeButton()
    {
        // For Button, since it can't pass parameter or return value
        AddNode();
    }
    #endregion

    #region Add Group
    public void ToggleAddGroupInputs()
    {
        if (!addGroupCollider || !addGroupCollider.OverlapPoint(_main.ScreenToWorldPoint(Input.mousePosition)))
            return;
        
        if (addGroupInputs.activeSelf)
        {
            addGroupInputs.SetActive(false);
        }
        else
        {
            addGroupInputs.SetActive(true);
            _groupNameInput.Select();
        }
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

    public void AddGroupButton()
    {
        // For Button, since it can't pass parameter or return value
        if (string.IsNullOrEmpty(_groupNameInput.text)) return;
        AddGroup(_groupNameInput.text);
    }
    
    private IEnumerator ProcNextFrame()
    {
        yield return new WaitForFixedUpdate();
        _onGroupParentRemoved.Proc();
    }
    #endregion
    
    #region Import text to Node
    public void OpenImportText()
    {
        importText.gameObject.SetActive(true);
        importText.Select();
    }
    
    public void ImportTextToNode()
    {
        if (string.IsNullOrEmpty(importText.text))
        {
            importText.gameObject.SetActive(false);
            return;
        }
        String[] words = importText.text.Split(',');
        List<String> targets = new List<string>();
        foreach (string word in words)
        {
            String trimmed = word.Trim();
            if (trimmed.Length > 0)
            {
                targets.Add(trimmed);
            }
        }

        for (int i=0; i<targets.Count-1; i++)
        {
            GameObject newNode = AddNode(false);
            newNode.GetComponentInChildren<TMP_InputField>().text = targets[i];
        }
        GameObject lastNewNode = AddNode(true);
        lastNewNode.GetComponentInChildren<TMP_InputField>().text = targets[targets.Count-1];
        
        importText.text = "";
        importText.gameObject.SetActive(false);
    }
    #endregion
}