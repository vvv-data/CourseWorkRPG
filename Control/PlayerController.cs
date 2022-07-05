using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement; // подключили чтобы сработала GetComponent<Mover>().MoveTo(hit.point); 
using System;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health; // класс скрипта здоровье
        private void Start()
        {
            health = GetComponent<Health>(); // присвоили компоненты скрипта здоровье
        }
        private void Update()
        {
            if (health.IsDead()) return; // если не живой то ничего не делаем
            if(InteractWithCombat()) return; // если передвижение к врагу труе то возвращаем исполнение этой функции
            if(InteractWithMovement()) return; // если правда то передвижение по назначению
        }

        private bool InteractWithCombat()
        {
          RaycastHit[] hits =  Physics.RaycastAll(GetMouseRay()); // получаем массив объектов на которые кликнули если они стоят друг за другом
            foreach (RaycastHit hit in hits) // перебираем элементы объектов
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>(); // получаем элемены врага, они будут если объект варг и будут 0 если объект не враг
                if (target == null) continue; // если объект не является врагом то target == null начинаем цыкл следующего элемента

                // target.gameObject это уже  гейм обжект таргета

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue; // если объект не является врагом, или он не живой то начинаем цыкл следующего элемента, а этот пропускаем тк он не враг 
                if (Input.GetMouseButtonDown(0)) // если момент щелчка мыши
                {
                    GetComponent<Fighter>().Attack(target.gameObject); // вызываем функцию Attack() из скрипта Fighter со значениями точки клика target                   
                }
                return true; // возвращаем труе если нашли врага
            }
            return false; 

        }

        private bool InteractWithMovement()
        {

            
            RaycastHit hit; // новая переменная нового направления
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit); // GetMouseRay() Возвращает луч, идущий от камеры через точку экрана в функцию Physics.Raycast(GetMouseRay(), out hit) вводим новое направлени GetMouseRay() из камеры получаем новое значение для переменной hit (hit.point можно поней узнать координаты новой точки в которую кликнули)

            if (hasHit) // если функция сработала то hit.point будут координаты точки в которую кликнули
            {
                if (Input.GetMouseButton(0))  // если нажали левую кнопку мыши
                {
                    // перемещение к объекту таргет при условии что установили NavMeshAgent
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f); // hit.point это точка куда кликнули, вызываем функцию перемещения не к врагу, 1f это множитель максим скорости тоесть на максимальной
                }
                return true; // если точка найдена то возвращаем правду
            }
            return false; 


        }

        private static Ray GetMouseRay() // Возвращает луч, идущий от камеры через точку экрана.
        {
         
            return Camera.main.ScreenPointToRay(Input.mousePosition);
            
        }
    }
}
