using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Timer : NetworkBehaviour
{
    [SerializeField] Text text;

    private NetworkVariable<bool> _started = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> _ended = new NetworkVariable<bool>();
    int speed;

    float time;

    private NetworkVariable<int> currentAllSeconds = new NetworkVariable<int>();

    private int AllSeconds => currentAllSeconds.Value;




    private int? destination = null;
    private bool isFrozen;

    public bool Started => _started.Value;
    public bool Ended => _ended.Value;

    private event Action OnFinished;

    private void Update()
    {
        if (!Started)
            return;


        TimeSpan timeSpan = new TimeSpan(0, 0, AllSeconds);

        if (timeSpan.Hours != 0)
            text.text = $"{timeSpan.Hours} : {timeSpan.Minutes} : {timeSpan.Seconds}";
        else
            text.text = $"{timeSpan.Minutes} : {timeSpan.Seconds}";

        if (!IsServer) // only server Will Edit Time
            return;

        if (isFrozen || Ended)
            return;

            time += Time.deltaTime * speed;
            int seconds = Mathf.CeilToInt(time);
            if (currentAllSeconds.Value != seconds)
                currentAllSeconds.Value = seconds;



        if (destination.HasValue && seconds == destination.Value)
        {
            _ended.Value = true;
            StopTimer();
        }
    }

    public enum TimeType { Seconds = 1, Minutes = 60, Hours = 3600};

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startFrom">minutes</param>
    /// <param name="type">seconds, mminutes or hours</param>
    public void Server_StartUpTimer(byte startFrom = 0, TimeType type = TimeType.Minutes, Action _OnFinishCallback = null)
    {
        time = startFrom * ((int)type);
        _started.Value = true;
        speed = 1;
        destination = null;

        OnFinished += _OnFinishCallback;
    }

    /// <summary>
    /// A countdown from <paramref name="startFrom"/> to 0
    /// </summary>
    /// <param name="startFrom">minutes</param>
    /// <param name="type">seconds, mminutes or hours</param>
    public void Server_StartDownTimer(Duration duration, Action _OnFinishCallback = null)
    {
        time = duration.TotalSecons;
        _started.Value = true;
        speed = -1;
        destination = 0;

        OnFinished += _OnFinishCallback;
    }

    private void StopTimer()
    {
        OnFinished?.Invoke();
        OnFinished = null;
    }

    public void Server_Freeze()
    {
        isFrozen = true;
    }
}
