using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Elysium.Attributes;

namespace Michsky.UI.ModernUIPack
{
    public class ProgressBar : MonoBehaviour
    {
        // Content
        [SerializeField, RequireInterface(typeof(IFillable))] private MonoBehaviour fillable;
        [SerializeField] private IntValue currentFill; 
        [SerializeField] private IntValue maxFill;
        [Range(0, 100)] public float currentPercent;
        [Range(0, 100)] public int speed;

        public float CurrentPercent
        {
            get
            {
                if(isManual && isScriptable && currentFill != null && maxFill != null)
                {
                    currentPercent = (float)currentFill.Value / (float)maxFill.Value * 100;
                }
                else if (isManual && !isScriptable && fillable != null)
                {                    
                    currentPercent = Fillable.CurrentFill / Fillable.MaxFill * 100;
                }

                return currentPercent;
            }
        }

        public IFillable Fillable => fillable as IFillable;

        // Resources
        public Image loadingBar;
        public TextMeshProUGUI textPercent;

        // Settings
        public bool isManual = true;
        public bool isScriptable;
        public bool restart;
        public bool invert;

        void OnEnable()
        {
            if (isManual && isScriptable)
            {
                currentFill.OnValueChanged += UpdateUI;
                UpdateUI();
            }
            else if (isManual && !isScriptable)
            {
                Fillable.OnFillValueChanged += UpdateUI;
                UpdateUI();
            }
        }

        private void OnDisable()
        {
            if (isManual && isScriptable)
            {
                currentFill.OnValueChanged -= UpdateUI;
            }
            else if (isManual && !isScriptable)
            {
                Fillable.OnFillValueChanged -= UpdateUI;
            }
        }

        void Update()
        {
            if (!isManual)
            {
                if (currentPercent <= 100 && invert == false)
                {
                    currentPercent += speed * Time.deltaTime;
                }
                else if (currentPercent >= 0 && invert == true)
                {
                    currentPercent -= speed * Time.deltaTime;
                }
                if (currentPercent >= 100 && speed != 0 && restart == true && invert == false)
                {
                    currentPercent = 0;
                }
                else if (currentPercent == 0 && speed != 0 && restart == true && invert == true)
                {
                    currentPercent = 100;
                }

                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            var percent = CurrentPercent;
            loadingBar.fillAmount = percent / 100;
            textPercent.text = ((int)percent).ToString("F0") + "%";
        }
    }
}