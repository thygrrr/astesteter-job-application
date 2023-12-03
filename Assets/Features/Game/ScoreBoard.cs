using System.Globalization;
using Channels.Concrete;
using Tiger.Events;
using Tiger.Events.Concrete;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Game
{
    public class ScoreBoard : MonoBehaviour
    {
        [Space] [Header("Balance Parameters")]
        [SerializeField] private float speedScore = 3300f;
        [SerializeField] private float speedMin = 20f;
        [SerializeField] private float speedMax = 120f;
        [SerializeField] private float speedExponent = 5;
        [SerializeField] private float accelScore = 1f;

        [Space][Header("Displays")]
        [SerializeField] private TextMeshProUGUI speedDisplay;
        [SerializeField] private TextMeshProUGUI scoreDisplay;
        [SerializeField] private TextMeshProUGUI finalScoreDisplay;
        [SerializeField] private TextMeshProUGUI livesDisplay;

        [Space] [Header("Channels")]
        [SerializeField] private DataChannel<int> scoreChannel;
        [SerializeField] private Vector3Channel playerVelocity;
        [SerializeField] private Vector3Channel playerAcceleration;
        [SerializeField] private GameStateChannel gameState;

        private NumberFormatInfo _nfi;
        private long _score;
        private long _goal;

        private long _speedBonus = 1;
        private long _speedBonusSmooth = 1;

        private float _speedBonusGoal;
        private float _speedBonusCurrent;
        
        public long lives { get; private set; } = 3;

        private float _displayAlpha = 0.3f;

        private void Awake()
        {
            _nfi = new NumberFormatInfo {NumberGroupSeparator = "'"};
            _score = 0;

            gameState.Subscribe(OnGameState);
        }

        private void OnEnable()
        {
            //playerAcceleration.Subscribe(OnAcceleration);
            scoreChannel.Subscribe(OnScore);
        }

        private void OnDisable()
        {
            //playerAcceleration.Unsubscribe(OnAcceleration);
            scoreChannel.Unsubscribe(OnScore);
        }

        private void OnGameState(GameState state)
        {
            switch (state)
            {
                case GameState.TitleScreen:
                    SetUpScores(true);
                    break;

                case GameState.Spawning:
                    enabled = true;
                    SetUpScores();
                    break;

                case GameState.Dying:
                    lives--;
                    SetUpScores();
                    break;
            
                case GameState.GameOver:
                    enabled = false;
                    finalScoreDisplay.text = _score.ToString("#,0", _nfi);
                    break;
            }

        }

        private void SetUpScores(bool newGame = false)
        {
            if (newGame)
            {
                lives = 3;
                _goal = 0;
                Update();
            }
            
            _speedBonus = 0;
            _speedBonusGoal = 0;
            _speedBonusCurrent = 0;
            _score = _goal;

            var livesString = lives switch
            {
                >= 4 => "cheater",
                3 => "three",
                2 => "two",
                1 => "one",
                _ => "zero"
            };
            livesDisplay.text = livesString;
        }

        private void Update()
        {
            //Speed bonus
            var speed = math.saturate(math.remap(speedMin, speedMax, 0, 1, playerVelocity.value.magnitude));
            _speedBonusGoal = math.pow(speed, speedExponent) * speedScore;
            _speedBonusCurrent += (_speedBonusGoal - _speedBonusCurrent) / 10.0f * Time.deltaTime;

            if (gameState.value == GameState.Alive)
            {
                var bonus = Mathf.FloorToInt(_speedBonusCurrent) * (4 - lives);
                _speedBonus = bonus switch
                {
                    >= 500 => bonus / 500 * 500,
                    >= 100 => bonus / 100 * 100,
                    >= 10 => bonus / 10 * 10,
                    _ => 1
                };
            }
            else _speedBonus = 1;

            UpdateScoreDisplay();

            UpdateSpeedBonusDisplay();
        }

        private void UpdateScoreDisplay()
        {
            //Smooth increment, 20% per frame
            var delta = (_goal - _score) / 5;
            if (delta == 0) delta = (int) math.sign(_goal - _score);
            _score += delta;

            scoreDisplay.color = _score < _goal ? new Color(2, 2, 0, 1) : new Color(1, 1, 1, _displayAlpha);
            scoreDisplay.text = _score.ToString("#,0", _nfi);
        }

        private void UpdateSpeedBonusDisplay()
        {
            //Smooth increment, 10% per frame
            var delta = (_speedBonus - _speedBonusSmooth) / 10;
            if (delta == 0) delta = (int) math.sign(_speedBonus - _speedBonusSmooth);
            _speedBonusSmooth += delta;

            var color = (_speedBonus - _speedBonusSmooth) switch
            {
                > 0 => new Color(1, 1, 0, 1) * 2,
                < 0 => new Color(1, 0, 0, 1) * 2,
                _ => new Color(1, 1, 1, _displayAlpha)
            };
            speedDisplay.color = color;
            speedDisplay.text = _speedBonusSmooth <= 9000 ? $"{_speedBonusSmooth}x" : @"OVER 9000";
        }

        private void OnScore(int data)
        {
            long score = data;
            _goal += score * _speedBonus;
        }

        private void OnAcceleration(Vector3 value)
        {
            _goal += Mathf.CeilToInt(value.sqrMagnitude * accelScore * Time.deltaTime) * _speedBonus;
        }
    }
}
