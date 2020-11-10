using Elysium.AI.Navmesh;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Elysium.AI.Senses
{
    public class NPCDetection : UnitDetection
    {
        protected override bool CheckTargetRemovalConditions(List<Transform> list, int index)
        {
            if (list[index - 1].gameObject == this.gameObject) { return true; }

            var v = list[index - 1].GetComponentInChildren<SmartNPC>();
            if (v == null) { return true; }

            return false;
        }

        protected override bool CheckThreatRemovalConditions(List<Transform> list, int index)
        {
            return false;
        }
    }
}