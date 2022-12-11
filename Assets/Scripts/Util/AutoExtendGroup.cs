using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Util
{
    /// <summary>
    /// Attached to Group object.
    /// Auto Expand Group layout in response to content changes.
    /// Manages the group / group parent / group holder.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(ContentSizeFitter))]
    public class AutoExtendGroup : UIBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private RectTransform _rectTransform;
        private RectTransform _parentRectTransform;
        private BoxCollider2D _parentCollider;
        private VerticalLayoutGroup _groupLayoutGroup;
        private RectTransform _groupHolderRectTransform;
        
        protected override void Start()
        {
            base.Start();
            
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rectTransform = GetComponent<RectTransform>();
            
            var groupParent = _rectTransform.parent;
            _parentRectTransform = groupParent.GetComponent<RectTransform>();
            _parentCollider = groupParent.GetComponent<BoxCollider2D>();
            
            var groupHolder = groupParent.parent;
            _groupLayoutGroup = groupHolder.GetComponent<VerticalLayoutGroup>();
            _groupHolderRectTransform = groupHolder.GetComponent<RectTransform>();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            
            // More node added : 
            // -> Increase Group Height (this script)
            // -> Increase Group Parent Height
            // -> Increase Group Holder Height (ContentSizeFitter)

            if (!_rectTransform) return;
            
            // Change Group height 
            var rect = _rectTransform.rect;
            var deltaHeight = rect.height - _spriteRenderer.size.y;
            _spriteRenderer.size = rect.size;

            // Change Group Parent height and box collider Height
            _parentRectTransform.sizeDelta = rect.size;
            _parentCollider.size = rect.size;
            
            // Change Group Holder height and position (ContentSizeFitter)
            var groupHolderRect = _groupHolderRectTransform.rect;
            _groupLayoutGroup.CalculateLayoutInputVertical();
            _groupLayoutGroup.SetLayoutVertical();
            _groupHolderRectTransform.sizeDelta = new Vector2(groupHolderRect.width, groupHolderRect.height + deltaHeight);
            _groupHolderRectTransform.anchoredPosition = new Vector2(_groupHolderRectTransform.anchoredPosition.x, (groupHolderRect.height + deltaHeight) * 0.5f);
        }
    }
}
