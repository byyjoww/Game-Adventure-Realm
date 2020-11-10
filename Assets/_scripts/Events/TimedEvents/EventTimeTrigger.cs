using System;
using System.Collections;
using UnityEngine;

public class EventTimeTrigger : MonoBehaviour
{

    [SerializeField, Tooltip("Call the event every <timeTrigger> seconds.")]
    double timeTrigger;

    [SerializeField] protected ScriptableEvent eventTarget;
    [SerializeField] bool startOnAwake = false;
    [SerializeField] bool dontDestroy = false;
    [SerializeField] DateTimeValue dateTimeInitializer;

    DateTime nextEvent;

    bool pauseEvent = false;
    bool initialized = false;

    float SecondsForNextEvent => Mathf.Max((float)(nextEvent - DateTime.UtcNow).TotalSeconds, 0);

    public bool IsRunning => currentRoutine != null;
    public bool IsPaused => pauseEvent;

    Coroutine currentRoutine;

    protected void Awake()
    {
        if (startOnAwake && eventTarget == null)
        {
            Destroy(this);
            Debug.LogError("trying to StartOnAwake an EventTimeTrigger without an Event.");
            return;
        }

        if (dontDestroy)
        {
            DontDestroyOnLoad(this);
        }

        if (startOnAwake)
        {
            StartTimer();
        }
    }

    void Init()
    {
        if(initialized) return;

        nextEvent = dateTimeInitializer != null
            ? dateTimeInitializer.Value
            : DateTime.UtcNow.AddSeconds(timeTrigger);
        initialized = true;
    }

    public void SetNextEvent(DateTime nexEventTime)
    {
        nextEvent = nexEventTime;
        initialized = true;
    }

    public void Pause()
    {
        pauseEvent = true;
    }

    public void Resume()
    {
        pauseEvent = false;
    }


    public void StartTimer(double timeToTrigger, ScriptableEvent targetEvent, DateTimeValue nextEventValue, bool keepAlive)
    {
        if (keepAlive)
        {
            DontDestroyOnLoad(this);
        }

        timeTrigger = timeToTrigger;
        dateTimeInitializer = nextEventValue;
        eventTarget = targetEvent;

        if (currentRoutine != null) return;

        Init();

        currentRoutine = StartCoroutine(RunTimerEvent());
    }

    public void StartTimer()
    {
        if (currentRoutine != null) return;

        Init();

        currentRoutine = StartCoroutine(RunTimerEvent());
    }

    public void StopTimer()
    {
        if (currentRoutine == null) return;

        StopCoroutine(currentRoutine);
        currentRoutine = null;
    }

    public void SetLastEvent(DateTime time)
    {
        nextEvent = time.AddSeconds(timeTrigger);
        initialized = true;
    }

    IEnumerator RunTimerEvent()
    {
        Init();

        var now = DateTime.UtcNow;
        while (nextEvent < now)
        {
            eventTarget.Raise();

            if (pauseEvent)
            {
                yield return new WaitUntil(() => !pauseEvent);
            }

            nextEvent = nextEvent.AddSeconds(timeTrigger);

            if (dateTimeInitializer != null)
            {
                dateTimeInitializer.Value = nextEvent;
            }
        }

        while (true)
        {
            yield return new WaitForSeconds(SecondsForNextEvent);

            while (pauseEvent)
            {
                yield return new WaitUntil(() => !pauseEvent);
                yield return new WaitForSeconds(SecondsForNextEvent);
            }

            eventTarget.Raise();

            nextEvent = nextEvent.AddSeconds(timeTrigger);

            if (dateTimeInitializer != null)
            {
                dateTimeInitializer.Value = nextEvent;
            }
        }
    }
}
