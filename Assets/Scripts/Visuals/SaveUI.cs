using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Visuals
{
    public class SaveUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text lable;

        private int _index;
        private Action<string> _delegateOnClick;
        private string _label;
        
        public void Init(int index, string label, Action<string> delegateOnClick)
        {
            _label = label;
            _index = index;
            lable.text = label;
            _delegateOnClick = delegateOnClick;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _delegateOnClick?.Invoke(_label);
        }
    }
}