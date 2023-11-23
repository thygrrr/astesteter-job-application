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
        [Header("Effects")] [SerializeField] private float cannonShake = 0.5f;
        [SerializeField] private ParticleSystem[] cannonFX;

        [SerializeField] [Header("Channels")] private IntChannel ammoCountChannel;

        [Header("Guns, Guns, Guns")] [SerializeField]
        private Rigidbody bulletPrefab;

        [SerializeField] private float muzzleVelocity = 100;
        [SerializeField] private int clipSize = 4; // Our 1979 ancestor had 4 shots
        [SerializeField] private float reloadTime = 1;
        [SerializeField] private float cycleTime = 0.05f;
        
        private int _bullets;
        private float _reloadTimer;
        private float _cycleTimer;

        private WorldBounds _world;
        
        #region Unity Events

        private void Awake()
        {
            Log.TagColor = Color.red;

            _world = GetComponentInParent<WorldBounds>();
            if (!_world) Log.Error("No world bounds found in parent!", this);
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

        private void OnValidate()
        {
            if (gameObject.IsAsset()) return;
            if (!bulletPrefab) Log.Error("No bullet prefab assigned!", this);
        }

        #endregion

        #region Gun Logic

        private bool canShoot => _cycleTimer <= 0 && _bullets > 0;

        public void OnFire(InputAction.CallbackContext context)
        {
            if (canShoot && context.action.IsPressed()) Shoot();
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

        private void AddRecoilEffects()
        {
            var fx = cannonFX.Shift();
            fx.gameObject.SetActive(true);
            fx.Play();
            ScreenShake.Add(transform.position, cannonShake, 0);

            transform.gameObject.AddTween(_recoil);
        }

        private static void OnRecoilUpdate(TweenInstance<Transform, float> instance, float value)
        {
            var t = instance.target.transform.parent;
            t.localPosition = -t.forward * value;
        }

        private readonly FloatTween _recoil = new()
        {
            from = 0.5f,
            to = 0,
            easeType = EaseType.SineInOut,
            duration = 0.8f,
            //gun rocks the entire ship ;) I know this is a feature envy smell but it's a fun effect.
            //TODO: theoretically conflicts with other tweens, for example the SpawnProcedure.
            //(but these are exclusive to each other thanks to the game state, so no real life impact atm.)
            //also, SpawnProcedure scales, and OnRecoilUpdate translates.
            onUpdate = OnRecoilUpdate,
        };
        
        #endregion
    }
}