using System.Globalization;
using Channels.Concrete;
using Features.Game;
using Tiger.Events;
using Tiger.Events.Concrete;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class ScoreBoard : DataChannelResponder<DataChannel<int>, int>
{
    [SerializeField] private TextMeshProUGUI speedDisplay;
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    [SerializeField] private TextMeshProUGUI finalScoreDisplay;
    [SerializeField] private TextMeshProUGUI livesDisplay;

    [SerializeField] private Vector3Channel playerVelocity;
    [SerializeField] private Vector3Channel playerAcceleration;
    [SerializeField] private GameStateChannel gameState;
    
    [SerializeField] private float speedScore = 3300f;
    [SerializeField] private float speedMin = 20f;
    [SerializeField] private float speedMax = 120f;
    [SerializeField] private float speedExponent = 5;
    [SerializeField] private float accelScore = 1f;

    private NumberFormatInfo _nfi;
    private long _score = 0;
    private long _goal = 0;

    private long _speedBonus = 1;
    
    private float _speedBonusGoal = 0;
    private float _speedBonusCurrent = 0;
    public long lives { get; private set; } = 3;

    private void Awake()
    {
        _nfi = new NumberFormatInfo {NumberGroupSeparator = "'"};
        _score = 0;
        Update();
        
        gameState.Subscribe(OnGameState);
        playerAcceleration.Subscribe(OnAcceleration);
    }

    private void OnAcceleration(Vector3 value)
    {
        _goal += Mathf.CeilToInt(value.sqrMagnitude * accelScore * Time.deltaTime) * _speedBonus;
    }

    private void OnGameState(GameState state)
    {
        switch (state)
        {
            case GameState.TitleScreen:
                ResetScores(true);
                break;
            
            default:
                ResetScores(false);
                break;
            
            case GameState.Dying:
                lives--;
                ResetScores(false);
                break;
            
            case GameState.GameOver:
                finalScoreDisplay.text = _score.ToString("#,0", _nfi);
                break;
        }

    }

    private void ResetScores(bool full)
    {
        if (full)
        {
            lives = 3;
            _goal = 0;
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
        if (CheckGameOver()) return;

        //Speed bonus
        var speed = math.saturate(math.remap(speedMin, speedMax, 0, 1, playerVelocity.value.magnitude));
        _speedBonusGoal = math.pow(speed, speedExponent) * (speedScore);
        _speedBonusCurrent += (_speedBonusGoal - _speedBonusCurrent) / 10.0f * Time.deltaTime;

        if (gameState.value == GameState.Alive)
        {
            var bonus = Mathf.FloorToInt(_speedBonusCurrent) * (4 - lives);
            _speedBonus = bonus switch
            {
                >= 1000 => bonus / 1000 * 1000,
                >= 100 => bonus / 100 * 100,
                >= 10 => bonus / 10 * 10,
                _ => 1
            };
        }
        else _speedBonus = 1;
        
        //Smooth increment
        _goal = _goal / 50 * 50;
        var delta = (_goal - _score) * 60 * Time.deltaTime;
        _score += (long) delta;

        scoreDisplay.text = _score.ToString("#,0", _nfi);

        speedDisplay.text = $"{_speedBonus}x";
    }

    private bool CheckGameOver()
    {
        return (gameState.value == GameState.GameOver);
    }

    protected override void OnEvent(int data)
    {
        long score = data;
        _goal += score * _speedBonus;
    }
}
