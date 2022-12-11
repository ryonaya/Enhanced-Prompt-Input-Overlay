using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Util
{
    /// <summary>
    /// Attached to Canvas Input Field.
    /// Used when Node Text is clicked,
    /// to allow user to edit the text,
    /// and finally send the text to the Node.
    /// </summary>
    public class CanvasTMPInput : MonoBehaviour
    {   
        private TMP_InputField _inputField;
        private RectTransform _rectTransform;
        private TMP_Text _childText;
        private RectTransform _childRectTransform;
        private ContentSizeFitter _childFitter;
        private float _originalWidth;
    
        private void Start()
        {
            _inputField = GetComponent<TMP_InputField>();
            _rectTransform = GetComponent<RectTransform>();
            _originalWidth = _rectTransform.sizeDelta.x;
        }
    
        public void OnValueChanged()
        {
            var delta = _rectTransform.sizeDelta;
            var deltaWidth = delta.x - _originalWidth;
            _originalWidth = delta.x;

            // PlaceHolder for no text
            if (_inputField.text.Length == 0)
            {
                _inputField.text = "■■■";
            }
            else if (_inputField.text.Length >= 4 && _inputField.text.Contains("■"))
            {
                // Remove all place holder
                for (int i=1; i<=3; i++)
                    _inputField.text = _inputField.text.Replace("■", "");
            }

            // Replace " " with "_" for content size fitter to work properly.
            _inputField.text = _inputField.text.Replace(" ", "_");
        
            // Pass the string to the child (TMP_text).
            _childText.text = _inputField.text;
            
            // Re-calculate the ContentSizeFitter of child.
            // for it to proc it's rect size change.
            _childFitter.enabled = true;
            _childFitter.SetLayoutHorizontal();
            _childFitter.SetLayoutVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_childText.GetComponent<RectTransform>());
            _childFitter.enabled = false;
        
            // Update dynamically.
            _rectTransform.position = _childRectTransform.position;
            _rectTransform.sizeDelta = _childRectTransform.sizeDelta;
        }

        public void OnEndEdit()
        {
            _rectTransform.position = new Vector3(-10000, -10000, 0);
            _inputField.DeactivateInputField();
        }
    
        public void Init(TMP_Text childText, RectTransform rectTransform, ContentSizeFitter childFitter)
        {
            _childText = childText;
            _childFitter = childFitter;
            _childRectTransform = rectTransform;
            _rectTransform.position = rectTransform.position;
            _rectTransform.sizeDelta = rectTransform.sizeDelta;
            _inputField.text = _childText.text;
            _inputField.Select();
            _inputField.ActivateInputField();        
        }
    }
}