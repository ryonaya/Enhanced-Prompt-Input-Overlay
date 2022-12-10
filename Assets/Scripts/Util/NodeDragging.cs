using UnityEngine;
using UnityEngine.EventSystems;

namespace Util
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(EventTrigger))]
    public class NodeDragging : MonoBehaviour
    {
        private Camera _main;
        private Transform _parent;
        private Transform _transform;
        private RaycastHit2D[] _result;
        // private bool _isDragging = false;
    
        private void Start()
        {
            _main = Camera.main;
            _transform = transform;
            _parent = _transform.parent;
            _result = new RaycastHit2D[5];
        }
    
        public void OnBeginDrag()
        {
            // _isDragging = true;
            _parent = _transform.parent;
            _transform.SetParent(null);
        }
    
        public void OnDrag()
        {
            Vector3 mousePos = _main.ScreenToWorldPoint(Input.mousePosition - new Vector3(0.5f, 0.5f, 0));
            _transform.position = new Vector3(mousePos.x, mousePos.y, _transform.position.z);
        }

    
        public void OnEndDrag()
        {
            // _isDragging = false;
        
            // raycast from mouse position to find which Group Parent
            // to attach onto (if not, then go back to original position)
            _transform.SetParent(_parent);
        
            Physics2D.RaycastNonAlloc(
                _main.ScreenToWorldPoint(Input.mousePosition), 
                Vector2.zero,
                _result
            );
            foreach (var hit in _result)
            {
                if (!hit.collider) continue;
            
                if (hit.collider.CompareTag("Drag Target"))
                {
                    // child 0 is Group label, child 1 is the Group
                    _transform.SetParent(hit.transform.GetChild(1));
                    _parent = _transform.parent;
                }
                else if (hit.collider.CompareTag("Trash Can"))
                {
                    Destroy(gameObject);                
                }
            
            }
        }
    }
}
