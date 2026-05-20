using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PCLoveGame.UI
{
    public sealed class DraggablePuzzlePieceView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Action<DraggablePuzzlePieceView> _onPlaced;
        private Canvas _canvas;
        private RectTransform _targetRect;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private Vector2 _initialAnchoredPosition;
        private bool _isPlaced;
        private float _snapDistance;

        public string PieceId { get; private set; }

        public void Initialize(string pieceId, string label, Canvas canvas, RectTransform targetRect, float snapDistance, Action<DraggablePuzzlePieceView> onPlaced)
        {
            PieceId = pieceId;
            _canvas = canvas;
            _targetRect = targetRect;
            _snapDistance = snapDistance;
            _onPlaced = onPlaced;
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            _initialAnchoredPosition = _rectTransform.anchoredPosition;

            Text text = GetComponentInChildren<Text>();

            if (text != null)
            {
                text.text = label;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_isPlaced)
            {
                return;
            }

            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isPlaced || _canvas == null)
            {
                return;
            }

            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isPlaced || _targetRect == null)
            {
                return;
            }

            _canvasGroup.blocksRaycasts = true;

            float distance = Vector2.Distance(_rectTransform.anchoredPosition, _targetRect.anchoredPosition);

            if (distance <= _snapDistance)
            {
                _rectTransform.anchoredPosition = _targetRect.anchoredPosition;
                _isPlaced = true;
                _canvasGroup.blocksRaycasts = false;
                _onPlaced?.Invoke(this);
                return;
            }

            _rectTransform.anchoredPosition = _initialAnchoredPosition;
        }

        public void SetInitialPosition(Vector2 anchoredPosition)
        {
            _initialAnchoredPosition = anchoredPosition;
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.anchoredPosition = anchoredPosition;
        }
    }
}
