using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Util
{
    [RequireComponent(typeof(TMP_InputField))]
    public class TMPInputHandler : MonoBehaviour
    {
        private TMP_InputField _inputField;
        private TMP_Text _childText;
        private ContentSizeFitter _parentFitter;
        private HorizontalLayoutGroup _parentLayoutGroup;
        private RectTransform _parentRectTransform;
        private RectTransform _rectTransform;
        private float _originalWidth;
        
        private void Start()
        {
            _inputField = GetComponent<TMP_InputField>();
            _childText = GetComponentInChildren<TMP_Text>();
            _parentFitter = GetComponentInParent<ContentSizeFitter>();
            _parentLayoutGroup = GetComponentInParent<HorizontalLayoutGroup>();
            _parentRectTransform = GetComponentInParent<RectTransform>();
            _rectTransform = GetComponent<RectTransform>();
            _originalWidth = _rectTransform.sizeDelta.x;
        }

        public void OnValueChanged()
        {
            var delta = _rectTransform.sizeDelta;
            var deltaWidth = delta.x - _originalWidth;
            _originalWidth = delta.x;

            _childText.text = _inputField.text;
            
            _parentFitter.enabled = true;
            _parentFitter.SetLayoutHorizontal();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_parentRectTransform);
            _parentFitter.enabled = false;
            // _parentLayoutGroup.CalculateLayoutInputHorizontal();
            // _parentLayoutGroup.SetLayoutHorizontal();
            // var sizeDelta = _parentRectTransform.sizeDelta;
            // sizeDelta = new Vector2(sizeDelta.x + deltaWidth, sizeDelta.y);
            // _parentRectTransform.sizeDelta = sizeDelta;
            
        }
        
        public void OnEndEdit()
        {
            
        }
        
        public void OnSelect()
        {
            // Debug.Log("OnSelect");
        }
    }
}
