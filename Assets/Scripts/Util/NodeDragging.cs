using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Util
{
    /// <summary>
    /// Handle single Node drag and drop
    /// attached to the node
    /// Just for trashcan-ing nodes
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(EventTrigger))]
    public class NodeDragging : MonoBehaviour
    {
        // public Material outlineMaterial;
        // private Material _defaultMaterial;
        
        private Camera _main;
        // private Transform _parent;
        private Transform _transform;
        private RaycastHit2D[] _result;
        // private AutoExtendGroup _groupScript;
        // private SpriteRenderer _spriteRenderer;
        // private HorizontalLayoutGroup _horizontalLayoutGroup;
        // private ContentSizeFitter _contentSizeFitter;
        // private RectTransform _rectTransform;
        
        private BoxCollider2D _collider;
        // private Vector2 _cursorOffset;
        private bool _validDrag;

        // private bool _isClone = false;
        // private GameObject _doppelganger;
        
        private void Start()
        {
            _main = Camera.main;
            _transform = transform;
            // _spriteRenderer = GetComponent<SpriteRenderer>();
            // _defaultMaterial = _spriteRenderer.material;
            _result = new RaycastHit2D[10];
            _collider = GetComponent<BoxCollider2D>();
            // _horizontalLayoutGroup = GetComponentInParent<HorizontalLayoutGroup>();
            // _contentSizeFitter = GetComponentInParent<ContentSizeFitter>();
            // _rectTransform = GetComponent<RectTransform>();
            
            // _contentSizeFitter.enabled = true;
            // _contentSizeFitter.SetLayoutHorizontal();
            // _contentSizeFitter.SetLayoutVertical();
            // _horizontalLayoutGroup.CalculateLayoutInputHorizontal();
            // _horizontalLayoutGroup.SetLayoutHorizontal();
            // LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
            // _contentSizeFitter.enabled = false;

            // _parent = _transform.parent;
            // if (_parent)
            //     _groupScript = _parent.GetComponent<AutoExtendGroup>();
            //
            // if (_isClone)
            //     _spriteRenderer.material = outlineMaterial;
        }

        public void OnBeginDrag()
        {
            // if (_isClone) return;
            
            if (_collider && _collider.OverlapPoint(_main.ScreenToWorldPoint(Input.mousePosition)))
                _validDrag = true;
            else return;
            
            // _parent = _transform.parent;
            // _groupScript = _parent.GetComponent<AutoExtendGroup>();
            // _cursorOffset = _collider.offset;

            // Create a copy of this object, and move its collider right under the cursor
            // _doppelganger = Instantiate(
            //     gameObject, _transform.position,
            //     _transform.rotation, null
            // );
            // var doppelgangerScript = _doppelganger.GetComponent<NodeDragging>();
            // doppelgangerScript._isClone = true;

            // Dim the original
            // _spriteRenderer.color *= 0.5f;
        }

        public void OnDrag()
        {
            // if (_isClone || !_validDrag) return;
            // if (!_validDrag) return;

            // Vector3 mousePos = _main.ScreenToWorldPoint(Input.mousePosition - new Vector3(0.5f, 0.5f, 0));
            // mousePos -= (Vector3)_cursorOffset;
            // _doppelganger.transform.position = new Vector3(mousePos.x, mousePos.y, _transform.position.z);
        }

        public void OnEndDrag()
        {
            // if (_isClone || !_validDrag) return;
            if (!_validDrag) return;
            
            // Destroy(_doppelganger);
            // _spriteRenderer.color *= 2.0f;
            
            // raycast from mouse position to find which Group to attach onto
            // if didn't land on any Group, then go back to original position
            // _transform.SetParent(_parent);

            Physics2D.RaycastNonAlloc(
                _main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero,
                _result
            );
            foreach (var hit in _result)
            {
                if (!hit.collider) continue;

                // if (hit.collider.CompareTag("Drag Target"))
                // {
                //     _transform.SetParent(hit.transform);
                //     
                //     // Proc both last parent and current parent
                //     if (_groupScript)   // Could be null if it is added by a button
                //         _groupScript.Proc();
                //     _parent = _transform.parent;
                //     _groupScript = _parent.GetComponent<AutoExtendGroup>();
                //     _groupScript.Proc();
                // }
                if (hit.collider.CompareTag("Trash Can"))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
