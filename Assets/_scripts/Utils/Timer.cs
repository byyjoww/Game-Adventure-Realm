using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Timers
{
    //[CreateFromPrefab("Timer")]
    public class Timer : Singleton<Timer>
    {
        public static List<TimerInstance> activeTimers = new List<TimerInstance>();

        public static TimerInstance CreateTimer(float timeInSeconds, bool isDestroyed = true)
        {
            GameObject timerObj = new GameObject();
            timerObj.transform.parent = Timer.Instance.transform;
            timerObj.AddComponent(typeof(TimerInstance));
            var timer = timerObj.GetComponent<TimerInstance>();
            timer.isDestroyed = isDestroyed;
            timer.StartTimer(timeInSeconds);
            activeTimers.Add(timer);

            return timer;
        }

        public void Update()
        {
            for (int i = 0; i < activeTimers.Count; i++)
            {
                if (activeTimers[i] == null) { activeTimers.Remove(activeTimers[i]); i--; }
                else if (!activeTimers[i].isEnded) { activeTimers[i].Tick(); }
            }
        }

        public float RemainingCooldown(TimerInstance timer)
        {
            var t = activeTimers.SingleOrDefault(x => x == timer);
            if (t == null) { return 0; }
            else { return t.time; }
        }
    }
}

public class TimerInstance : MonoBehaviour
{
    public float time;
    public bool isEnded = false;
    public bool isDestroyed = true;
    public event Action OnTimerEnd;
    public event Action OnTick;

    public void StartTimer(float time)
    {
        this.time = time;
    }

    public void Tick()
    {
        if (isEnded) { return; }

        time -= Time.deltaTime;
        OnTick?.Invoke();

        if (time <= 0)
        {
            time = 0;
            isEnded = true;
            OnTimerEnd?.Invoke();
            if (isDestroyed) { Destroy(this.gameObject); }            
        }
    }

    public void AddTime(float time)
    {
        this.time += time;
        if (time > 0)
        {
            isEnded = false;
        }
    }

    public void SetTime(float time)
    {
        this.time = time;
        if (time > 0)
        {
            isEnded = false;
        }
    }
}