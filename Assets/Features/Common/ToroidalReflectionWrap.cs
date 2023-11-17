using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.UIElements;

namespace Features.Common
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ToroidalReflectionWrap : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        private GameObject _reflection;

        [SerializeField]
        private Bounds _worldBounds;
        private Bounds bounds => _meshRenderer.bounds;
        
        private Vector3 _lastPosition;
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
           
            _reflection = new GameObject("reflection", typeof(MeshRenderer), typeof(MeshFilter), typeof(RenderBounds));
            _reflection.SetActive(false);
            _reflection.transform.localScale = transform.localScale;
            _reflection.GetComponent<MeshFilter>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
            _reflection.GetComponent<MeshRenderer>().sharedMaterial = _meshRenderer.sharedMaterial;
        }

        private void LateUpdate()
        {
            transform.position += new Vector3(2.5f, 0, -1f) * Time.deltaTime * 3;

            //transform.position = Wrap(transform.position);
            
            // Nothing to do if original is fully inside the bounds.
            var fullyInside = _worldBounds.Contains(bounds.min) && _worldBounds.Contains(bounds.max);
            if (fullyInside)
            {
                _reflection.SetActive(false);
                return;
            }


            //If we have one reflection, we keep it till it's fully inside the first time.
            if (_reflection.activeSelf)
            {
                //Move reflection with the original object
                var delta = transform.position - _lastPosition;
                _reflection.transform.Translate(delta);
                _reflection.transform.rotation = transform.rotation;
                _lastPosition = transform.position;
                
                //Check if we can swap the position of the reflection and the original object if the original is further from the bounds.
                //We can't check for partial outside-ness because there are corner cases where both objects have their centers outside. 
                var moreOutside = _worldBounds.SqrDistance(transform.position) > _worldBounds.SqrDistance(_reflection.transform.position);
                if (moreOutside)
                {
                    (_reflection.transform.position, transform.position) = (transform.position, _reflection.transform.position);
                }
            } 
            else
            {
                SpawnReflection();
            }
        }

        private void SpawnReflection()
        {
            //Initialize position for delta motion
            _lastPosition = transform.position;

            //Place at reflection point
            _reflection.transform.position = Reflect(_lastPosition);
            _reflection.SetActive(true);
        }

        private float3 Wrap(float3 position)
        {
            float3 extents = _worldBounds.extents + bounds.extents;
            position = Reflect(position);

            //_reflection.transform.position = transform.position;

            return position;
        }

        private float3 Reflect(float3 position)
        {
            //Reflection needs to spawn on the opposite side of the bounds, long before Wrap() takes place
            float3 wrapSize = _worldBounds.size;
            return position - math.sign(position) * wrapSize * math.select(1.0f, 0.5f, math.cmax(math.abs(position)) > math.abs(position));
        }

        private void OnDestroy()
        {
            Destroy(_reflection);
        }
    }
}
