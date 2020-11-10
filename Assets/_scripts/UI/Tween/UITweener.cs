using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace LivingTheDeal.UI
{

    public enum UIAnimationTypes
    {
        Move,
        Scale,
        ScaleX,
        ScaleY,
        Fade
    }

    public class UITweener : MonoBehaviour
    {
        public GameObject objectToAnimate;

        public UIAnimationTypes animationType;
        public LeanTweenType easeType;
        public float duration;
        public float delay;
        public bool hideDuringDelay;

        public bool loop;
        public bool pingpong;

        public bool startPositionOffset;
        public Vector3 from;
        public Vector3 to;
        
        private LTDescr _tweenObject;
        
        public bool showOnEnable;
        public bool workOnDisable;


        public void OnEnable()
        {
            if (showOnEnable)
            {
                Show();
            }
            
        }

        public void Show()
        {
            HandleTween();
        }

        public void HandleTween()
        {
            if (objectToAnimate == null)
            {
                objectToAnimate = gameObject;
            }

            switch (animationType)
            {
                case UIAnimationTypes.Fade:
                    Fade();
                    break;
                case UIAnimationTypes.Move:
                    MoveAbsolute();
                    break;
                case UIAnimationTypes.Scale:
                    Scale();
                    break;
                case UIAnimationTypes.ScaleX:
                    from = new Vector3(from.x, transform.localScale.y, transform.localScale.z);
                    to = new Vector3(to.x, transform.localScale.y, transform.localScale.z);
                    Scale();
                    break;
                case UIAnimationTypes.ScaleY:
                    from = new Vector3(transform.localScale.x, from.y, transform.localScale.z);
                    to = new Vector3(transform.localScale.x, to.y, transform.localScale.z);
                    Scale();
                    break;
            }

            _tweenObject.setDelay(delay);

            if((delay>=0f) && hideDuringDelay)
            {
                CanvasGroup group = objectToAnimate.GetComponent<CanvasGroup>();
                if (group == null)
                {
                    group = objectToAnimate.AddComponent<CanvasGroup>();
                }

                group.alpha = 0;
                //Show the object when it starts
                _tweenObject.setOnStart(() =>
                {
                    group.alpha = 1;
                });

            }

            _tweenObject.setEase(easeType);

            if (loop)
            {
                _tweenObject.loopCount = int.MaxValue;
            }
            if (pingpong)
            {
                _tweenObject.setLoopPingPong();
            }
        }

        public void Fade()
        {

            if(gameObject.GetComponent<CanvasGroup>() == null)
            {
                gameObject.AddComponent<CanvasGroup>();
            }

            if (startPositionOffset)
            {
                objectToAnimate.GetComponent<CanvasGroup>().alpha = from.x;
            }
                _tweenObject = LeanTween.alphaCanvas(objectToAnimate.GetComponent<CanvasGroup>(), to.x, duration);
        }

        public void MoveAbsolute()
        {
            objectToAnimate.GetComponent<RectTransform>().anchoredPosition = from;


            _tweenObject = LeanTween.move(objectToAnimate.GetComponent<RectTransform>(), to, duration);
        }

        public void Scale()
        {
            if (startPositionOffset)
            {
                objectToAnimate.GetComponent<RectTransform>().localScale = from;
            }
            _tweenObject = LeanTween.scale(objectToAnimate, to, duration);
        }

        void SwapDirection()
        {
            var temp = from;
            from = to;
            to = temp;
            
        }

        public void Disable()
        {

            SwapDirection();

            HandleTween();

            _tweenObject.setOnComplete(()=> {

                SwapDirection();

                gameObject.SetActive(false);
            });
        }
        public void Disable(Action onCompleteAction)
        {

            SwapDirection();

            HandleTween();

            _tweenObject.setOnComplete(()=> {

                SwapDirection();

                onCompleteAction();
            });
        }

        private void OnDisable()
        {

            //LeanTween.cancel(gameObject);
            
        }







    }
}
