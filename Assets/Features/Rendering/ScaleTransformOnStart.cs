using Tweens;
using UnityEngine;

namespace Features.Rendering
{
    public class ScaleTransformOnStart : MonoBehaviour
    {
        public float delay = 0.75f;
        public float duration = 1f;
        
        private void Start()
        {
            transform.localScale = Vector3.zero;
            var tween = new LocalScaleTween()
            {
                delay = delay,
                duration = duration,
                easeType = EaseType.BackOut,
                from = Vector3.zero,
                to = Vector3.one,
            };
            
            gameObject.AddTween(tween);
        }
    }
}
