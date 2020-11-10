using Elysium.AI.Navmesh;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Elysium.AI.Senses
{
    public class MonsterDetection : UnitDetection
    {
        protected override bool CheckTargetRemovalConditions(List<Transform> list, int index)
        {
            if (list[index - 1].gameObject == this.gameObject) { return true; }

            var v = list[index - 1].GetComponentInChildren<IDamageable>();
            if (v == null) { return true; }

            var unit = list[index - 1].gameObject.GetComponent<NPCInputController>();
            if (unit == null || unit.currentState == "dead") { return true; }

            return false;
        }

        protected override bool CheckThreatRemovalConditions(List<Transform> list, int index)
        {
            if (list[index - 1].gameObject == this.gameObject) { return true; }

            var v = list[index - 1].GetComponentInChildren<IDamageDealer>();
            if (v == null) { return true; }

            var unit = list[index - 1].gameObject.GetComponent<NPCInputController>();
            if (unit == null || unit.currentState == "dead") { return true; }

            return false;
        }
    }
}