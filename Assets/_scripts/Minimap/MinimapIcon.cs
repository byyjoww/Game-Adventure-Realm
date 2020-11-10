using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Minimap
{
    public class MinimapIcon : MonoBehaviour
    {
        private Vector3 baseScale;

        public void Start()
        {
            baseScale = transform.localScale;
            Minimap.OnZoomChanged += Minimap_OnZoomChanged;
        }

        private void Minimap_OnZoomChanged(MinimapZoom obj)
        {
            transform.localScale = baseScale * Minimap.GetZoom() / 180f;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}