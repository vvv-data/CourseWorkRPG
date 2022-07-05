using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable // подключили интерфейс ISaveable(система сохранения папка Saving)
    {
       [SerializeField] float healthPoints = 100f; // это количество здоровья, сколько нужно ударов для врага

        bool isDead = false; // состояние противника живой

        public bool IsDead() // публичная функция по которой можно всегда узнать жив ли соперник, вызвав ее даже из другого скрипта
        {
            return isDead; 
        }

      public void TakeDamage(float damage) //уменьшаем здоровье врага при получении им удара
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if(healthPoints == 0)
            {
                Die(); //включаем тригер анимации падения противника

            }
            //print(healthPoints);

        }

        private void Die()
        {
            if(isDead) return; // если уже мертв не выполняем все что дальше
            isDead = true;
            GetComponent<Animator>().SetTrigger("die"); //включаем тригер анимации падения противника
            GetComponent<ActionScheduler>().CancelCurrentAction(); // прекращаем текущую акцию, чтобы он не таскался за объектом и тд
        }

        public object CaptureState()  // захватывает статистику, это CaptureState из интерфейса скрипта ISaveable (система сохранения папка Saving)
        {
            return healthPoints; // healthPoints остаток здоровья
        }

        public void RestoreState(object state)  // востанавливает статистику здоровья из сохраненки (система сохранения папка Save)
        {
            healthPoints = (float)state;
            
            if (healthPoints == 0)
            {
                Die(); //включаем типа не живой

            }
        }
    }
}
