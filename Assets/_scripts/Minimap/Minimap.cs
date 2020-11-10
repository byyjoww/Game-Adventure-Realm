using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Minimap
{
    public static class Minimap
    {
        public static event Action<MinimapWindow> OnWindowShow;
        public static event Action<MinimapWindow> OnWindowHide;
        public static event Action<MinimapZoom> OnZoomChanged;

        public static void Init()
        {
            MinimapZoom.OnZoomChanged += MinimapZoom_OnZoomChanged;
            MinimapWindow.OnWindowShow += MinimapWindow_OnWindowShow;
            MinimapWindow.OnWindowHide += MinimapWindow_OnWindowHide;
        }

        private static void MinimapZoom_OnZoomChanged(MinimapZoom obj)
        {
            OnZoomChanged?.Invoke(obj);
        }

        private static void MinimapWindow_OnWindowShow(MinimapWindow obj)
        {
            OnWindowShow?.Invoke(obj);
        }

        private static void MinimapWindow_OnWindowHide(MinimapWindow obj)
        {
            OnWindowHide?.Invoke(obj);
        }

        public static void ShowWindow()
        {
            MinimapWindow.Show();
        }

        public static void HideWindow()
        {
            MinimapWindow.Hide();
        }

        public static void ZoomIn()
        {
            MinimapZoom.ZoomIn();
        }

        public static void ZoomOut()
        {
            MinimapZoom.ZoomOut();
        }

        public static void SetZoom(float size)
        {
            MinimapZoom.SetZoom(size);
        }

        public static float GetZoom()
        {
            return MinimapZoom.GetZoom();
        }
    }
}