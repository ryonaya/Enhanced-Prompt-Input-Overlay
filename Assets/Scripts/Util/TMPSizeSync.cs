using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class TMPSizeSync : UIBehaviour
{
    private RectTransform _parentRectTransform;
    private RectTransform _rectTransform;

    protected override void Start()
    {
        base.Start();
        _parentRectTransform = transform.parent.GetComponent<RectTransform>();
        _rectTransform = GetComponent<RectTransform>();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();

        _parentRectTransform.sizeDelta = _rectTransform.sizeDelta;
    }
}
