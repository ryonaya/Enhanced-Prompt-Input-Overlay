using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Proc
{
    /// <summary>
    /// Attached to Group object.
    /// Auto Expand Group layout in response to node adding/removing.
    /// Manages the group / group parent / group holder.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(ContentSizeFitter))]
    public class AutoExtendGroup : UIBehaviour
    {
        private RectTransform _rectTransform;
        private FlowLayoutGroup _layoutGroup;
        private ContentSizeFitter _fitter;
        
        private RectTransform _parentRectTransform;
        private HorizontalLayoutGroup _parentLayoutGroup;
        private ContentSizeFitter _parentFitter;
        
        private RectTransform _groupHolderRectTransform;
        private VerticalLayoutGroup _groupLayoutGroup;
        private ContentSizeFitter _groupFitter;
        
        protected override void Start()
        {
            base.Start();
            _rectTransform = GetComponent<RectTransform>();
            _layoutGroup = GetComponent<FlowLayoutGroup>();
            _fitter = GetComponent<ContentSizeFitter>();
            
            var groupParent = _rectTransform.parent;
            _parentRectTransform = groupParent.GetComponent<RectTransform>();
            // _parentLayoutGroup = groupParent.GetComponent<HorizontalLayoutGroup>();
            // _parentFitter = groupParent.GetComponent<ContentSizeFitter>();

            var groupHolder = groupParent.parent;
            if (!groupHolder) return;
            _groupHolderRectTransform = groupHolder.GetComponent<RectTransform>();
            _groupLayoutGroup = groupHolder.GetComponent<VerticalLayoutGroup>();
            _groupFitter = groupHolder.GetComponent<ContentSizeFitter>();     
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            
            if (!_rectTransform) return;
            // More node added : 
            // -> Increase Group Height (this script)
            // -> Increase Group Parent Height
            // -> Increase Group Holder Height (ContentSizeFitter)

            // Change Group height 
            _fitter.enabled = true;
            _fitter.SetLayoutHorizontal();
            _fitter.SetLayoutVertical();
            _layoutGroup.CalculateLayoutInputHorizontal();
            _layoutGroup.CalculateLayoutInputVertical();
            _layoutGroup.SetLayoutHorizontal();
            _layoutGroup.SetLayoutVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
            _fitter.enabled = false;

            // Change Group Parent height 
            _parentRectTransform.sizeDelta = _rectTransform.sizeDelta;

            // Change Group Holder height and position (ContentSizeFitter)
            _groupFitter.enabled = true;
            _groupFitter.SetLayoutHorizontal();
            _groupFitter.SetLayoutVertical();
            _groupLayoutGroup.CalculateLayoutInputHorizontal();
            _groupLayoutGroup.CalculateLayoutInputVertical();
            _groupLayoutGroup.SetLayoutHorizontal();
            _groupLayoutGroup.SetLayoutVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_groupHolderRectTransform);
            _groupFitter.enabled = false;
        }

        public void Proc()
        {
            OnRectTransformDimensionsChange(); 
            if (gameObject.activeInHierarchy)
                StartCoroutine(ProcSecond());
        }

        IEnumerator ProcSecond()
        {
            yield return new WaitForEndOfFrame();
            OnRectTransformDimensionsChange();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            StopAllCoroutines();
        }
    }
}
