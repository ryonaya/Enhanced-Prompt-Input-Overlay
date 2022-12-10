using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Util
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(ContentSizeFitter))]
    public class AutoExtendGroup : UIBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private RectTransform _rectTransform;

        protected override void Start()
        {
            base.Start();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rectTransform = GetComponent<RectTransform>();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            // Hierarchy:
            // Add More Node
            // -> Increase Group Height (this script)
            // -> Increase Group Parent Height
            // -> Increase Group Holder Height (ContentSizeFitter)
            // -> Increase Panel Height 
            
            // Change Group height 
            var rect = _rectTransform.rect;
            var deltaHeight = rect.height - _spriteRenderer.size.y;
            _spriteRenderer.size = rect.size;

            // Change Group Parent height and box collider Height
            var groupParent = _rectTransform.parent;
            var parentRectTransform = groupParent.GetComponent<RectTransform>();
            var parentCollider = groupParent.GetComponent<BoxCollider2D>();
            parentRectTransform.sizeDelta = rect.size;
            parentCollider.size = rect.size;
            
            // Change Group Holder height and position (ContentSizeFitter)
            var groupHolder = groupParent.parent;
            var groupLayoutGroup = groupHolder.GetComponent<VerticalLayoutGroup>();
            var groupHolderRectTransform = groupHolder.GetComponent<RectTransform>();
            var groupHolderRect = groupHolderRectTransform.rect;
            groupLayoutGroup.CalculateLayoutInputVertical();
            groupLayoutGroup.SetLayoutVertical();
            groupHolderRectTransform.sizeDelta = new Vector2(groupHolderRect.width, groupHolderRect.height + deltaHeight);
            groupHolderRectTransform.anchoredPosition = new Vector2(groupHolderRectTransform.anchoredPosition.x, (groupHolderRect.height + deltaHeight) * 0.5f);

            // Change Panel rect/sprite height and position 
            // This is handled by the OnGroupParentRemoved script
        }
    }
}
