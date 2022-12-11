using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Util
{
    /// <summary>
    /// Attached to the TMP text object.
    /// Used to sync the size of its parent (node).
    /// Also trigger canvas input field to properly edit text.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(TMP_Text))]
    public class TMPSizeSync : UIBehaviour
    {
        private TMP_Text _text;
        private RectTransform _rectTransform;
        private RectTransform _nodeRectTransform;
        private ContentSizeFitter _fitter;
        private ContentSizeFitter _nodeFitter;
        private HorizontalLayoutGroup _nodeLayout;

        protected override void Start()
        {
            base.Start();
            
            _text = GetComponent<TMP_Text>();
            
            var node = transform.parent;
            _rectTransform = GetComponent<RectTransform>();
            _nodeRectTransform = node.GetComponent<RectTransform>();
            _fitter = GetComponent<ContentSizeFitter>();
            _nodeFitter = node.GetComponent<ContentSizeFitter>();
            _nodeLayout = node.GetComponent<HorizontalLayoutGroup>();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            if (!_nodeFitter || !_nodeRectTransform)
               return;

            _nodeFitter.enabled = true;
            _nodeFitter.SetLayoutHorizontal();
            _nodeFitter.SetLayoutVertical();
            _nodeLayout.CalculateLayoutInputHorizontal();
            _nodeLayout.CalculateLayoutInputVertical();
            _nodeLayout.SetLayoutHorizontal();
            _nodeLayout.SetLayoutVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_nodeRectTransform);
            _nodeFitter.enabled = false;
        }
        
        public void OnSelect()
        {
            CanvasController.instance.canvasInputField.Init(_text, _rectTransform, _fitter);
        }
    }
}