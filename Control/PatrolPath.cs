using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.3f; // радиус шариков обозначающих охраняемую территорию
        private void OnDrawGizmos()  //рисуем шарики
        {
            for (int i = 0; i < transform.childCount; i++)  // transform.childCount количество вложенных точек
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWayPoint(i), waypointGizmoRadius); // центр точки transform.GetChild(i).position, радиус transform.GetChild(i).position
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount) return 0; // если последняя точка то следующая будет первой с индоксом 0
            return i + 1; //следующая точка индекс + 1
        }

        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
