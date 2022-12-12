using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        
        private RectTransform _panelRectTransform;
        private HorizontalLayoutGroup _panelLayoutGroup;
        private ContentSizeFitter _panelFitter;
        
        private RectTransform _rootRectTransform;
        private HorizontalLayoutGroup _rootLayoutGroup;
        private ContentSizeFitter _rootFitter;
        
        
        protected override void Start()
        {
            base.Start();
            _rectTransform = GetComponent<RectTransform>();
            _preHeight = _rectTransform.rect.height;
            
            var panel = transform.parent;
            _panelRectTransform = panel.GetComponent<RectTransform>();
            _panelLayoutGroup = _panelRectTransform.GetComponent<HorizontalLayoutGroup>();
            _panelFitter = _panelRectTransform.GetComponent<ContentSizeFitter>();

            var root = panel.parent;
            _rootRectTransform = root.GetComponent<RectTransform>();
            _rootLayoutGroup = root.GetComponent<HorizontalLayoutGroup>();
            _rootFitter = root.GetComponent<ContentSizeFitter>();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            if (!_rectTransform) return;

            // Modify panel
            _panelFitter.enabled = true;
            _panelFitter.SetLayoutHorizontal();
            _panelFitter.SetLayoutVertical();
            _panelLayoutGroup.CalculateLayoutInputHorizontal();
            _panelLayoutGroup.SetLayoutHorizontal();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_panelRectTransform);
            _panelFitter.enabled = false;
            
            // Modify Root
            _rootFitter.enabled = true;
            _rootFitter.SetLayoutHorizontal();
            _rootFitter.SetLayoutVertical();
            _rootLayoutGroup.CalculateLayoutInputHorizontal();
            _rootLayoutGroup.SetLayoutHorizontal();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_panelRectTransform);
            _rootFitter.enabled = false;
        }

        // Force update 
        public void Proc()
        {
            OnRectTransformDimensionsChange();
        }
    }
}
