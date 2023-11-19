using System;
using UnityEngine;

namespace Tiger.Loggers
{
    [CreateAssetMenu(menuName = "Debug/Debug Settings", fileName = "Debug Settings", order = 0)]
    public class DebugSettings : ScriptableObject
    {
        public static Level LogLevel;
        
        public bool enabled; 
        public Level level;
        public Color color = Color.cyan;

        private string ApplyColor(string input)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{input}</color>";
        }

        public void Log(string message, UnityEngine.Object context = null)
        {
            if (!enabled || level > LogLevel) return;
            
            //TODO: color the message nicely (some format parsing perhaps).
            switch (level)
            {
                case Level.Info:
                    Debug.Log(ApplyColor(message), context);
                    break;
                case Level.Warn:
                    Debug.LogWarning(ApplyColor(message), context);
                    break;
                case Level.Error:
                    Debug.LogError(ApplyColor(message), context);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public enum Level
        {
            Info,
            Warn,
            Error
        }
    }
}
