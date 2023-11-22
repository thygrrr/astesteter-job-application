using Feature.Ui;
using Features.Space;
using Tiger.ScreenShake;
using Tiger.Util;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Features.Player
{
    using Log = Loggers.Create<WeaponControls>;

    public class WeaponControls : MonoBehaviour, GameInputActions.IWeaponActions
    {
        [Header("Effects")]
        [SerializeField] private float cannonShake = 0.5f;
        [SerializeField] private ParticleSystem[] cannonFX;


        [Header("Guns, Guns, Guns")] 
        [SerializeField] private Rigidbody bulletPrefab;
        [SerializeField] private float muzzleVelocity = 100;
        [SerializeField] private int clipSize = 4; // Our 1979 ancestor had 4 shots
        [SerializeField] private float reloadTime = 1;
        [SerializeField] private float cycleTime = 0.05f;

    
        private int _bullets;
        private float _reloadTimer;
        private float _cycleTimer;
        
        private WorldBounds _world;

        private bool canShoot => _cycleTimer <= 0 && _bullets > 0;

        private void Awake()
        {
            _world = GetComponentInParent<WorldBounds>() ?? FindAnyObjectByType<WorldBounds>();
        }

        private void Update()
        {
            _cycleTimer -= Time.deltaTime;

            //Autoloader
            _reloadTimer -= Time.deltaTime;
            if (_reloadTimer <= 0) _bullets = clipSize;
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (canShoot && context.action.IsPressed())
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            _bullets--;
            _cycleTimer = cycleTime;
            _reloadTimer = reloadTime;

            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation, _world.transform);
            bullet.velocity = transform.forward * muzzleVelocity;

            var fx = cannonFX.Shift();
            fx.gameObject.SetActive(true);
            fx.Play();
            ScreenShake.Add(transform.position, cannonShake, 0);
        }

        private void OnValidate()
        {
            if (gameObject.IsAsset()) return;
            if (!bulletPrefab) Log.Error("No bullet prefab assigned!", this);
        }
    }
}
