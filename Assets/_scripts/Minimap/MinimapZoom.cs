using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Minimap
{
    [RequireComponent(typeof(Camera))]
    public class MinimapZoom : MonoBehaviour
    {
        public static event Action<MinimapZoom> OnZoomChanged;

        [SerializeField] private Camera minimapCamera;        

        [Header("Zoom Details")]
        [SerializeField] private const float ZOOM_INCREMENT = 10;
        [SerializeField] private const float STARTING_ZOOM = 100;
        [SerializeField] private const float MIN_ZOOM = 0;
        [SerializeField] private const float MAX_ZOOM = 200;

        public static MinimapZoom Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            if (minimapCamera == null) minimapCamera = GetComponent<Camera>();
            minimapCamera.orthographicSize = STARTING_ZOOM;
        }

        public static void SetZoom(float orthographicSize)
        {
            Instance.minimapCamera.orthographicSize = orthographicSize;

            if (Instance.minimapCamera.orthographicSize < MIN_ZOOM) { Instance.minimapCamera.orthographicSize = MIN_ZOOM; }
            if (Instance.minimapCamera.orthographicSize > MAX_ZOOM) { Instance.minimapCamera.orthographicSize = MAX_ZOOM; }

            OnZoomChanged?.Invoke(Instance);
        }

        public static float GetZoom()
        {
            return Instance.minimapCamera.orthographicSize;
        }

        public static void ZoomIn()
        {
            SetZoom(Instance.minimapCamera.orthographicSize - ZOOM_INCREMENT);            
        }

        public static void ZoomOut()
        {
            SetZoom(Instance.minimapCamera.orthographicSize + ZOOM_INCREMENT);            
        }

        private void OnValidate()
        {
            if (minimapCamera == null) minimapCamera = GetComponent<Camera>();
        }
    }
}