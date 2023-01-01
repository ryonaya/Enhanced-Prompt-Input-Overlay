using System;
using TMPro;
using UnityEngine;

namespace Util
{
    /// <summary>
    /// 
    /// </summary>
    public class RootController : MonoBehaviour
    {
        public SpriteRenderer panel;
        public float leftBorder;
        public float upBorder;
    
        public bool collapsed;

        private void Start()
        {
            // move root to top-right corner of the screen
            var rootTransform = transform;
            rootTransform.position = new Vector3(-(Screen.width * 0.5f)+leftBorder, (Screen.height * 0.5f)-upBorder, 0);
        }

        public void ToggleCollapse()
        {
            if (collapsed)  // out
            {
                transform.position += new Vector3(panel.bounds.size.x, 0, 0);
            }
            else            // in and copy text
            {
                transform.position -= new Vector3(panel.bounds.size.x, 0, 0);
                GUIUtility.systemCopyBuffer = ExportText();
            }
            collapsed = !collapsed;
        }

        public String ExportText()
        {
            String text = "";

            TMP_InputField[] inputFields = GetComponentsInChildren<TMP_InputField>();
            for (int i=0; i<inputFields.Length; i++)
            {
                if (inputFields[i].text == "Enter text...") continue;
                text += inputFields[i].text + ", ";
            }

            return text;
        }
    }
}
