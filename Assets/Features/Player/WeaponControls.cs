using Feature.Ui;
using Features.Space;
using Tiger.Events.Concrete;
using Tiger.ScreenShake;
using Tiger.Util;
using Tweens;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Features.Player
{
    using Log = Loggers.Create<WeaponControls>;

    public class WeaponControls : MonoBehaviour, GameInputActions.IWeaponActions
    {
        [Header("Effects")]        [SerializeField] private float cannonShake = 0.5f;
        [SerializeField] private ParticleSystem[] cannonFX;

        [SerializeField] [Header("Channels")]
        private IntChannel ammoCountChannel;

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

        private Vector3Tween _recoil;


        private bool canShoot => _cycleTimer <= 0 && _bullets > 0;

        private void Awake()
        {
            _world = GetComponentInParent<WorldBounds>() ?? FindAnyObjectByType<WorldBounds>();
            
        }

        private void OnEnable()
        {
            _bullets = 0;
            _reloadTimer = reloadTime; 
        }

        private void Update()
        {
            //Simple autoloader / bullet cycling.
            _cycleTimer -= Time.deltaTime;
            _reloadTimer -= Time.deltaTime;
            if (_reloadTimer <= 0 && _bullets < clipSize)
            {
                _bullets = clipSize;
                ammoCountChannel.Emit(_bullets);
            }
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
            ammoCountChannel.Emit(_bullets);
            _cycleTimer = cycleTime;
            _reloadTimer = reloadTime;

            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation, _world.transform);
            bullet.velocity = transform.forward * muzzleVelocity;
            
            AddRecoilEffects();
        }

        void AddRecoilEffects()
        {
            var fx = cannonFX.Shift();
            fx.gameObject.SetActive(true);
            fx.Play();
            ScreenShake.Add(transform.position, cannonShake, 0);

            _recoil = new Vector3Tween
            {
                from = -transform.forward * 0.5f,
                to = Vector3.zero,
                easeType = EaseType.SineInOut,
                duration = 0.8f,
                //gun rocks the entire ship ;) I know this is a feature envy smell but it's a fun effect.
                onUpdate = (_, value) => transform.parent.position = value,
            };
            transform.parent.gameObject.AddTween(_recoil);
        }
        private void OnValidate()
        {
            if (gameObject.IsAsset()) return;
            if (!bulletPrefab) Log.Error("No bullet prefab assigned!", this);
        }
    }
}
