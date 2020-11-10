using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Minimap
{
    public class MinimapWindow : MonoBehaviour
    {
        public static event Action<MinimapWindow> OnWindowShow;
        public static event Action<MinimapWindow> OnWindowHide;

        public static MinimapWindow Instance;

        private void Awake()
        {
            Instance = this;
        }

        public static void Show()
        {
            Instance.gameObject.SetActive(true);
            OnWindowShow?.Invoke(Instance);
        }

        public static void Hide()
        {
            Instance.gameObject.SetActive(false);
            OnWindowHide?.Invoke(Instance);
        }

        #region BUTTON_FUNCTIONS

        public void ShowWindow()
        {
            Minimap.ShowWindow();
        }

        public void HideWindow()
        {
            Minimap.HideWindow();
        }

        public void ZoomIn()
        {
            Minimap.ZoomIn();
        }

        public void ZoomOut()
        {
            Minimap.ZoomOut();
        }

        #endregion
    }
}