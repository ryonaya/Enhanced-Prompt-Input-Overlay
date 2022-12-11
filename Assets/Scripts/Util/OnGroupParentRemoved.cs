using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Util
{
    /// <summary>
    /// Attached to Group Holder.
    /// When Group Parent is removed or added,
    /// Group holder will automatically grow/reduce in size.
    /// Thus we need to modify the size/position of its parent (panel).
    /// </summary>
    public class OnGroupParentRemoved : UIBehaviour
    {
        private RectTransform _rectTransform;
        private float _preHeight;
        
        protected override void Start()
        {
            base.Start();
            _rectTransform = GetComponent<RectTransform>();
            _preHeight = _rectTransform.rect.height;
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            var rect = _rectTransform.rect;
            if (_preHeight == 0)    // Init
            {
                _preHeight = rect.height;
                return;
            }
            var deltaHeight = rect.height - _preHeight;
            _preHeight = rect.height;
            if (deltaHeight == 0)   // No change
            {
                return;
            }

            // Move this position
            _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, rect.height * 0.5f);
            
            // Modify panel's size
            var panel = transform.parent;
            var panelSpriteRenderer = panel.GetComponent<SpriteRenderer>();
            var panelRectTransform = panel.GetComponent<RectTransform>();
            var size = panelSpriteRenderer.size;
            size = new Vector2(size.x, size.y + deltaHeight);
            panelSpriteRenderer.size = size;
            panelRectTransform.sizeDelta = size;
            panelRectTransform.localPosition = new Vector2(panelRectTransform.localPosition.x, -size.y * 0.5f);
        }
    }
}
