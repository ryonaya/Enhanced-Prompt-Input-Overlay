using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Proc
{
    /// <summary>
    /// Attached to the TMP Input Field object.
    /// Used to sync the size of its parent (node).
    /// </summary>
    public class TMPInputSizeSync : UIBehaviour
    {

        private RectTransform _nodeRectTransform;
        private RectTransform _rectTransform;
        private RectTransform _textAreaRectTransform;
        private ContentSizeFitter _nodeFitter;
        private ContentSizeFitter _fitter;
        private ContentSizeFitter _textAreaFitter;
        private HorizontalLayoutGroup _nodeLayout;
        private HorizontalLayoutGroup _layoutGroup;
        private HorizontalLayoutGroup _textAreaLayoutGroup;

        private TMP_InputField _inputField;
        private TMP_Text _text;
        private Transform _caret;
        private bool _caretDragging = false;
        
        protected override void Start()
        {
            base.Start();
            
            var node = transform.parent;
            _nodeRectTransform = node.GetComponent<RectTransform>();
            _nodeFitter = node.GetComponent<ContentSizeFitter>();
            _nodeLayout = node.GetComponent<HorizontalLayoutGroup>();
            
            _rectTransform = GetComponent<RectTransform>();
            _fitter = GetComponent<ContentSizeFitter>();
            _layoutGroup = GetComponent<HorizontalLayoutGroup>();

            var textArea = transform.GetChild(0);
            _textAreaRectTransform = textArea.GetComponent<RectTransform>();
            _textAreaFitter = textArea.GetComponent<ContentSizeFitter>();
            _textAreaLayoutGroup = textArea.GetComponent<HorizontalLayoutGroup>();
            
            // chile of textarea : caret, placeholder, text
            _inputField = GetComponent<TMP_InputField>();
            _text = textArea.Find("Text").GetComponent<TMP_Text>();
            _caret = textArea.Find("Caret");
        }

        // protected override void OnRectTransformDimensionsChange()
        public void MainProc()
        {
            // base.OnRectTransformDimensionsChange();

            if (!_nodeFitter || !_nodeRectTransform)
                return;

            // _textAreaFitter.enabled = true;
            // _textAreaFitter.SetLayoutHorizontal();
            // _textAreaFitter.SetLayoutVertical();
            // No layout group in parent of a text object, Unity didn't support it
            // _textAreaLayoutGroup.CalculateLayoutInputHorizontal();
            // _textAreaLayoutGroup.CalculateLayoutInputVertical();
            // _textAreaLayoutGroup.SetLayoutHorizontal();
            // _textAreaLayoutGroup.SetLayoutVertical();
            // LayoutRebuilder.ForceRebuildLayoutImmediate(_textAreaRectTransform);
            // _textAreaFitter.enabled = false;
            
            _fitter.enabled = true;
            _fitter.SetLayoutHorizontal();
            _fitter.SetLayoutVertical();
            // _layoutGroup.CalculateLayoutInputHorizontal();
            // _layoutGroup.CalculateLayoutInputVertical();
            // _layoutGroup.SetLayoutHorizontal();
            // _layoutGroup.SetLayoutVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
            _fitter.enabled = false;
            
            _nodeFitter.enabled = true;
            _nodeFitter.SetLayoutHorizontal();
            _nodeFitter.SetLayoutVertical();
            _nodeLayout.CalculateLayoutInputHorizontal();
            _nodeLayout.CalculateLayoutInputVertical();
            _nodeLayout.SetLayoutHorizontal();
            _nodeLayout.SetLayoutVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_nodeRectTransform);
            _nodeFitter.enabled = false;

            Transform node = transform.parent;
            Transform group = node.parent;
            group.GetComponent<AutoExtendGroup>().Proc();
        }

        public void Proc()
        {
            MainProc();
            if (gameObject.activeInHierarchy)
                StartCoroutine(ProcAgain());
        }

        IEnumerator ProcAgain()
        {
            yield return null;
            MainProc();
        }
        
        public void ResetTextPosition()
        {
            StartCoroutine(ResetLater());
        }

        IEnumerator ResetLater()
        {
            yield return null;
            _text.transform.localPosition = Vector3.zero;
            _caret.transform.localPosition = Vector3.zero;
        }

        public void ResetTextPositionNow()
        {
            _text.transform.localPosition = Vector3.zero;
            _caret.transform.localPosition = Vector3.zero;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            StopAllCoroutines();
        }

        public void OnPointerDown()
        {
            _caretDragging = true;
        }
        
        public void OnPointerUp()
        {
            _caretDragging = false;
        }
        
        private void Update()
        {
            // The worst semi-solution to the problem of caret position
            // it will still blinking when dragging
            if (!_caretDragging) return;
            _text.transform.localPosition = Vector3.zero;
            _caret.transform.localPosition = Vector3.zero;
        }
    }
}