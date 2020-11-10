using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTween : MonoBehaviour
{
    [SerializeField] public SpriteRenderer SpriteRenderer;
    [SerializeField] Color target;
    Color start;

    [SerializeField] AnimationCurve tweenCurve;
    float time;
    [SerializeField] public float timeScale = 1f;
    [SerializeField] bool disableOnFinish;
    void OnEnable()
    {
        start = SpriteRenderer.color;
        time = 0;
    }
    
    private void Update()
    {
        SpriteRenderer.color = Color.Lerp(start, target, tweenCurve.Evaluate(timeScale * time));

        float maxTime = tweenCurve.keys[tweenCurve.keys.Length - 1].time;
        time += Time.deltaTime;

        if (disableOnFinish && timeScale * time >= maxTime)
        {
            SpriteRenderer.color = target;
            this.enabled = false;
        }
    }

    private void OnValidate()
    {
        SpriteRenderer = SpriteRenderer ?? GetComponent<SpriteRenderer>();
    }
}
