using Elysium.AI.Navmesh;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Elysium.AI.Senses
{
    public class UnitDetection : MonoBehaviour
    {
        [SerializeField] private float viewSphereRadius = 20f;
        public float ViewSphereRadius => viewSphereRadius;
        [SerializeField] private float viewAngle = 60f;
        public float ViewAngle => viewAngle;

        public bool ThreatDetected { get; set; }
        public event Action OnThreatDetected;
        public event Action OnThreatListUpdated;

        public bool TargetDetected { get; set; }
        public event Action OnTargetDetected;
        public event Action OnTargetListUpdated;

        [HideInInspector] public List<Transform> Targets = new List<Transform>();
        [HideInInspector] public List<Transform> Threats = new List<Transform>();
        [HideInInspector] public List<Transform> AllTargets = new List<Transform>();

        public System.Action<List<Transform>> OnTargetsUpdated;

        private void Start()
        {
            DetectUnit();
        }

        private void Update()
        {
            DetectUnit();
        }

        private void DetectUnit()
        {
            AllTargets = AINavigation.LocateTargetBlind(transform, viewSphereRadius, viewAngle);

            FilterTargets(AllTargets);
            FilterThreats(AllTargets);

            OnTargetsUpdated?.Invoke(AllTargets);
        }

        private void FilterTargets(List<Transform> targets)
        {
            if (targets == null) { return; }

            var t = new List<Transform>(targets);

            for (int i = t.Count; i > 0; i--)
            {
                if (CheckTargetRemovalConditions(t, i))
                {
                    t.RemoveAt(i - 1);
                }
            }

            if (t.Count < 1)
            {
                TargetDetected = false;
                Targets.Clear();
                OnTargetListUpdated?.Invoke();
                return;
            }

            TargetDetected = true;            
            Targets = t;
            OnTargetDetected?.Invoke();
            OnTargetListUpdated?.Invoke();
        }

        private void FilterThreats(List<Transform> threats)
        {
            if (threats == null) { return; }

            var t = new List<Transform>(threats);

            for (int i = t.Count; i > 0; i--)
            {
                if (CheckThreatRemovalConditions(t, i))
                {
                    t.RemoveAt(i - 1);
                }
            }

            if (t.Count < 1)
            {
                ThreatDetected = false;
                Threats.Clear();
                OnThreatListUpdated?.Invoke();
                return;
            }

            ThreatDetected = true;            
            Threats = t;
            OnThreatDetected?.Invoke();
            OnThreatListUpdated?.Invoke();
        }

        protected virtual bool CheckTargetRemovalConditions(List<Transform> list, int index)
        {
            if (list[index - 1].gameObject == this.gameObject) { return true; }

            //var v = list[index - 1].GetComponent<>();
            //if (v == null) { return true; }

            return false;
        }

        protected virtual bool CheckThreatRemovalConditions(List<Transform> list, int index)
        {
            if (list[index - 1].gameObject == this.gameObject) { return true; }            

            //var v = list[index - 1].GetComponent<>();
            //if (v == null) { return true; }

            return false;
        }

        private void OnDrawGizmos()
        {
            // Draw wire sphere indicating the detection radius
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, viewSphereRadius);

            // Draw rays indicating the field of view limits
            Gizmos.color = Color.black;

            Vector3 minFieldOfView = Quaternion.AngleAxis(-viewAngle, Vector3.up) * transform.forward * viewSphereRadius;
            Gizmos.DrawRay(transform.position, minFieldOfView);

            Vector3 maxFieldOfView = Quaternion.AngleAxis(viewAngle, Vector3.up) * transform.forward * viewSphereRadius;
            Gizmos.DrawRay(transform.position, maxFieldOfView);
        }
    }
}