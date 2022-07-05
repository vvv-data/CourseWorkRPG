using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBeetweenAttacks = 1f; // промежуток между атаками
        [SerializeField] float weaponDamage = 5f; // урон врагу от ударов

        Health target;  //цель - враг координаты его, но класс скрипта Health
        float timeSinceLastAttack = Mathf.Infinity; // сколько прошло с последней атаки, чтобы опсчитать когда делать следующую через timeBeetweenAttacks

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            
            if (target == null) return; // если нету цели врага то все что дальше не выполняем, таргет присваивается функцией Attack которая вызывается в скрипте PlayerController
            if (target.IsDead()) return; // если target.IsDead() тру то враг уже не живой, эта IsDead() находится в скрипте Health на противнике
            if (!GetIsInRange())  //если не подошол близко к врагу то выпоняем
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f); // 1f - множитель максимальной скорости тоесть на максималной
            }
            else
            {
                GetComponent<Mover>().Cancel(); // выполняется когда подошли к врагу
                AttackBehavior(); // включаем анимацию атаки
            }
        }

        private void AttackBehavior() // включаем анимацию атаки
        {
            transform.LookAt(target.transform); // доворачиваем персонажа перед атакой прямо на противника, чтобы не бил боком как то
            if(timeSinceLastAttack > timeBeetweenAttacks)
            {
                // тут также будет также срабатывать тригер удара Hit() по врагу тк событие ее вызова  прописано в самом анимационном клипе
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event функция Hit() выполняется при анимационном событии удара, она прописана в самом анимационном клипе

        void Hit()
        {
            if (target == null) return; // если цель нулевая ничего не делаем
            target.TakeDamage(weaponDamage); // вызываем функцию TakeDamage(weaponDamage) из скрита Health, которая считает урон для врага от нанесенных ударов
        }
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange; // isInRange будет тру ессли верно выражение, тоесть подошли близко к врагу
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false; // если объекта противника нет то фальше
            Health   targetToTest = combatTarget.GetComponent<Health>(); // переменная с компонентами скрипта Health
            return targetToTest != null && !targetToTest.IsDead();  // вернет труе если targetToTest не ноль и объект живой, иначе вернет фальше
        }
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this); //переключает от простой хотьбы к атаке и останавливает перед врагом с помощью функции Cancel
            target = combatTarget.GetComponent<Health>(); // присвоили переменной компонент  скрипта Health он находится на противнике
        }
        public void Cancel()
        {
            StopAttack(); 
            target = null;
            GetComponent<Mover>().Cancel(); // выполняется остановка движения, например когда подошли к врагу
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");  // подали команду в аниматор остановить анимацию аттаки, например если повернули персонажа в другую сторону
        }

    }
}
