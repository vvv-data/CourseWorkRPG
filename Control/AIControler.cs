using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIControler : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f; // расстояние на котором начинается преследование
        [SerializeField] float suspicionTime = 3f; // продолжительность подозрения(сколько стоит после преследования, потом поворачивается и идет на пост охранять)
        [SerializeField] PatrolPath patrolPath; // скопировали туда родителя, это объект родитель префаб с сточками дочерними охранной територии(сделали не через гейм обжект тк это префаб)
        [SerializeField] float waypointTolerance = 1f; // дистанция при которой подошел если охранник то срабатывает движение к следующей охранной точке
        [SerializeField] float waypointDwellTime = 3f; // сколько постоит охранник в охранной точке при патрулировании
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f; // это множитель скорости от максимальной 0,2 это 20%, чтобы задавать разную скорость но в пределах максимальной, которая устнавивается в скрипте мувер

        Fighter fighter; // класс скрипта Fighter
        Health health; // класс скрипта Health
        Mover mover; // класс скрипта Mover
        GameObject player;
        Vector3 guardPosition;  // точка которую нужно охранять, возвращаться в нее
        float timeSinceLastSawPlayer = Mathf.Infinity; // время с момента последней встречи игрока, чтобы сделать подозрение
        float timeSinceArrivedAtWaypoint = Mathf.Infinity; // время с момента прибытия в охранную точку, чтобы считать сколько он в ней постоит
        int currentWaypointIndex = 0; // текущий индекс охранной точки

        private void Start()
        {
            fighter = GetComponent<Fighter>(); // присвоили компоненты скрипта файтера
            health = GetComponent<Health>(); // присвоили компоненты скрипта здоровья
            player = GameObject.FindWithTag("Player"); //получаем гаме обжект по тегу Player
            mover = GetComponent<Mover>(); // присвоили компоненты скрита Mover
            guardPosition = transform.position; // начальная точка где стоял охранник, которую нужно охранять, возвращаться в нее
        }

        private void Update()
        {
            if (health.IsDead()) return; // если не живой то никаких атак

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player)) // проверяем дистанцию от врага до плеера не меньше ли она дистанции преследовани и жив ли атакуемый
            {               
                AttackBehaviour(); // вызываем функцию атаки из скрипта файтер
                //print(gameObject.name + "chase"); // gameObject.name - это имя текущего объекта
            }
            else if (timeSinceLastSawPlayer < suspicionTime) // стоим если текущее время стояния меньше заданного времени подозрения
            {
                // suspicion state  стоим на месте подозреваем
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour(); // если преследование окончено, то охранник возвращается и патрулирует територию
            }
            NewMethod();
        }

        private void NewMethod()
        {
            timeSinceLastSawPlayer += Time.deltaTime; // прибавляем время чтобы посчитать сколько подозревает
            timeSinceArrivedAtWaypoint += Time.deltaTime;  // прибавляем время чтобы посчитать сколько постоял в охранной точке
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition; // по умолчанию начальная точка
            if(patrolPath != null)
            {
                if (AtWaypoint()) // если охранник подошел к точке близко на расстояние waypointTolerance то идем к следующей точке
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if(timeSinceArrivedAtWaypoint > waypointDwellTime) // еси охранник постоял в охранной точке достаточно то...
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction); // если преследование окончено, то охранник возвращается в начальную точку и начинает патрулировать по охранным точкам
            }
            //mover.StartMoveAction(nextPosition); // если преследование окончено, то охранник возвращается в начальную точку и начинает патрулировать по охранным точкам
        }
        private bool AtWaypoint()  // расчитываем дистанцию до охранной точки если она меньше толерантной то ТРУЕ занчит можно идти к следующей точке
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
           return distanceToWaypoint < waypointTolerance;
        }
        private void CycleWaypoint() // вызываем метод из скрипта PatrolPath и вычисляем индекс следующей точки
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWayPoint(currentWaypointIndex); // вызываем метод из скрипта PatrolPath и вычисляем координаты текущей точки
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction(); // останавливаем текущую акцию стоим подозреваем
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0; // обнуляем время после атаки чтобы посчитать подозрение
            fighter.Attack(player); // вызываем функцию атаки из скрипта файтер
        }

        private bool InAttackRangeOfPlayer() // проверяем близость расстояния для аттаки
        {
            // стравниваем дистанцию между плеером и текущим объектом тк скрит установлен на противнике, труе если близко
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position); 
            return distanceToPlayer < chaseDistance;
        }
        // Called by Unity
        private void OnDrawGizmosSelected()  // рисует элементы если объект выделен, хотим выделить дистанцию преследования
        {
            // это встроенная функция рисует сферу из голубых линий с центром в позиции текущего объекта и радиус  chaseDistance дистанцию преследования
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
      
    }
}
