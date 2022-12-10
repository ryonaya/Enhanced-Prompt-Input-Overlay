using System;
using UnityEngine;

namespace Util
{
    public class MaintainPosition : MonoBehaviour
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
            if (collapsed)
            {
                transform.position += new Vector3(panel.bounds.size.x, 0, 0);
            }
            else
            {
                transform.position -= new Vector3(panel.bounds.size.x, 0, 0);
            }
            collapsed = !collapsed;
        }
    }
}
