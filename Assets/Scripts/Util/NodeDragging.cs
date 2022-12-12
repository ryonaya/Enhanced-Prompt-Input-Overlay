using UnityEngine;
using UnityEngine.EventSystems;

namespace Util
{
    /// <summary>
    /// Handle single Node drag and drop
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(EventTrigger))]
    public class NodeDragging : MonoBehaviour
    {
        private Camera _main;
        private Transform _parent;
        private Transform _transform;
        private RaycastHit2D[] _result;
        private AutoExtendGroup _groupScript;
        private bool _proc = false;
    
        private void Start()
        {
            _main = Camera.main;
            _transform = transform;
            _parent = _transform.parent;
            _groupScript = _parent.GetComponent<AutoExtendGroup>();
            _result = new RaycastHit2D[5];
        }
    
        public void OnBeginDrag()
        {
            _proc = true;
            _parent = _transform.parent;
            _transform.SetParent(null);
        }
    
        public void OnDrag()
        {
            Vector3 mousePos = _main.ScreenToWorldPoint(Input.mousePosition - new Vector3(0.5f, 0.5f, 0));
            _transform.position = new Vector3(mousePos.x, mousePos.y, _transform.position.z);
            
            // Proc rect change
            if (_groupScript && _proc)
            {
                _groupScript.Proc();
                _proc = false;
            }
        }

        public void OnEndDrag()
        {
            _proc = false;
        
            // raycast from mouse position to find which Group to attach onto
            // if didn't land on any Group, then go back to original position
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
                    _transform.SetParent(hit.transform);
                    _parent = _transform.parent;
                    _groupScript = _parent.GetComponent<AutoExtendGroup>();
                    _groupScript.Proc();
                }
                else if (hit.collider.CompareTag("Trash Can"))
                {
                    Destroy(gameObject);                
                }
            }
        }
    }
}
