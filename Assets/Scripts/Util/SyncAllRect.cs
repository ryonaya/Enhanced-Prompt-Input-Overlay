using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Util
{
    /// <summary>
    /// Attached to any objects with RectTransform.
    /// Sync the size of SpriteRenderer/BoxCollider2D to the size of the RectTransform.
    /// Since they aren't affected by ContentSizeFitter
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class SyncAllRect : UIBehaviour
    {
        [Header("Move node drag box to the left")]
        public bool isSpecialCollider;
        public float leftPaddingOfNode = 50;

        private RectTransform _rectTransform;
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCollider2D;
        
        // Start is called before the first frame update
        protected override void Start()
        {   
            _rectTransform = GetComponent<RectTransform>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            if (!_rectTransform) return;

            var sizeDelta = _rectTransform.sizeDelta;
            if (_spriteRenderer)
                _spriteRenderer.size = sizeDelta;
            if (_boxCollider2D)
            {
                if (!isSpecialCollider)
                    _boxCollider2D.size = sizeDelta;
                else
                {
                    _boxCollider2D.size = new Vector2(leftPaddingOfNode, sizeDelta.y);
                    _boxCollider2D.offset = new Vector2(-(sizeDelta.x - leftPaddingOfNode) * 0.5f, 0);
                }
            }
        }
    }
}
