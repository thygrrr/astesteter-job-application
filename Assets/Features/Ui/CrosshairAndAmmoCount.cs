using Tiger.Events;
using Tiger.Events.Concrete;
using Tweens;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Features.Ui
{
    [RequireComponent(typeof(RectTransform))]
    public class CrosshairAndAmmoCount : DataChannelResponder<IntChannel, int>
    {
        private RectTransform _transform;
        [SerializeField] private Image[] cartridges;

        private ColorTween _onTween;
        private ColorTween _offTween;
        
        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            foreach (var cartridge in cartridges) cartridge.gameObject.SetActive(false);

            _onTween = new ColorTween
            {
                from = Color.black,
                to = Color.white,
                duration = 0.1f,
                loops = 4,
                onAdd = tween => tween.target.gameObject.SetActive(true),
                //for brevity's sake: if this ever becomes an issue, we can make a separate tween for each cartridge
                onUpdate = (tween, value) => tween.target.GetComponent<Image>().color = value,
            };

            _offTween = new ColorTween
            {
                from = Color.red, 
                to = Color.black,
                duration = 0.3f,
                easeType = EaseType.BackIn,
                onAdd = tween => tween.target.gameObject.SetActive(true),
                //for brevity's sake: if this ever becomes an issue, we can make a separate tween for each cartridge
                onUpdate = (tween, value) => tween.target.GetComponent<Image>().color = value,
                onEnd = tween => tween.target.gameObject.SetActive(false),
            };
        }

        private void Update()
        {
            //Mouse capture
            var mouseHeld = Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed;
            Cursor.lockState = mouseHeld ? CursorLockMode.Confined : CursorLockMode.None;
        }

        protected override void OnEvent(int data)
        {
            for (var i = 0; i < cartridges.Length; i++)
            {
                var on = i < data;
                if (cartridges[i].gameObject.activeSelf == on) continue;
                cartridges[i].gameObject.CancelTweens();
                cartridges[i].gameObject.AddTween(on ? _onTween : _offTween);
            }
        }

        private void LateUpdate()
        {
            //CAVEAT: This only works if the parent UI element is anchored TOP-RIGHT (can be zero size)
            //Imagine that: The U in UGUI stands for "upside-down".
            _transform.anchoredPosition = Mouse.current.position.ReadValue();
        }
    }
}
