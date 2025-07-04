using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Controls
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public event Action ON_FIRST_TOUCH;

        private const float _maxRadius = 60f;

        [SerializeField] private GameObject _background, _handle;

        [HideInInspector] public bool IsTouched;
        [HideInInspector] public float Horizontal, Vertical;

        private Vector2 _touchStart;
        private Vector2 _input;
        private Camera _uiCamera;

        private void Awake()
        {
            SpritesVisibility(false);
            _uiCamera = Camera.main; // Mude se usar Canvas "Screen Space - Camera"
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ON_FIRST_TOUCH?.Invoke();

            // Aparece joystick onde clicou/tocou
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _background.transform.parent as RectTransform, eventData.position, _uiCamera, out var pos);
            _background.transform.localPosition = pos;
            _handle.transform.localPosition = pos;
            SpritesVisibility(true);

            IsTouched = true;
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _background.transform.parent as RectTransform, eventData.position, _uiCamera, out var pos);

            var direction = pos - (Vector2)_background.transform.localPosition;
            _input = Vector2.ClampMagnitude(direction, _maxRadius);
            _handle.transform.localPosition = _background.transform.localPosition + (Vector3)_input;

            Horizontal = _input.x / _maxRadius;
            Vertical = _input.y / _maxRadius;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsTouched = false;
            Horizontal = 0f;
            Vertical = 0f;
            SpritesVisibility(false);
            _handle.transform.localPosition = _background.transform.localPosition;
        }

        private void SpritesVisibility(bool value)
        {
            _background.SetActive(value);
            _handle.SetActive(value);
        }

        public Vector2 Direction => new Vector2(Horizontal, Vertical);
    }
}
