using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;  // библиотека для дестинешена
using RPG.Core;
using RPG.Saving;


namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable    // подключили интерфейсы IAction, ISaveable(система сохранения папка Saving), значит надо использовать их функции
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f; // максимальная скорость
        NavMeshAgent navMeshAgent;
        Health health; // класс скрипта Health

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>(); // присвоили компонент
            health = GetComponent<Health>(); // присвоили компоненты скрипта здоровья
        }

        // Update is called once per frame
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead(); // navMeshAgent включено если не мертв, если мертв то проходят серез него насквозь и он нешелохнется, а иначе он двигался лежа когда его пинали
            ApdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction) // начинаем акцию для перемещения и атаки
        {
            GetComponent<ActionScheduler>().StartAction(this); // начинаем новую акцию StartAction вызывает Cancel(из файла файтинг и текущего) и обнуляет старую акцию
           // GetComponent<Fighter>().Cancel(); // вызываем функцию которая обнуляет таргет противника и останавливает нападение
            MoveTo(destination, speedFraction);
        }
        public void MoveTo(Vector3 destination, float speedFraction) // для перемещения к врагу
        {
            navMeshAgent.destination = destination; // destination берем из hit.point это точка куда кликнули, вычислили ее по Physics.Raycast(ray, out hit)
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction); // задаем скорость перемещения в множителе от максимальной, Mathf.Clamp01 клампит до десятых, тоесть если speedFraction=0.25 то скорость будет 20% от максимальной
            navMeshAgent.isStopped = false; // isStopped труе прописывают при остановке перед врагом, поэтому при дальнейшей хотьбе его надо переключить
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true; //останавливаем движение когда подошли близко к врагу
        }

   

        private void ApdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity; // получаем глобальную скорость движения персонажа
            Vector3 localVelocity = transform.InverseTransformDirection(velocity); //  получаем локальную скорость движения персонажа
            float speed = localVelocity.z; // скорость локального перемещения по z
            GetComponent<Animator>().SetFloat("forwardSpeed", speed); // устанавливаем компонент аниматора forwardSpeed равным speed чтобы при быстром перемещении проигрывалась анимация бега и тд

        }

        
        [System.Serializable]
        struct MoverSaveData // создаем новые переменные - структуры
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }
        public object CaptureState()  // захватывает статистику местопложения, это CaptureState из интерфейса скрипта ISaveable (система сохранения папка Saving)
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position); // класс в скрипте SerializableVector3 раскидывает вектор transform.position на отдельные x y z
            data.rotation = new SerializableVector3(transform.eulerAngles); // тут записываем поворот
            return data;

        }

        public void RestoreState(object state)  // востанавливает статистику (система сохранения папка Save)
        {
            MoverSaveData data = (MoverSaveData)state; // получаем переменную со значениями позиции и поворота
            GetComponent<NavMeshAgent>().enabled = false; // отключили NavMeshAgent тк передвигаем объект
            transform.position = data.position.ToVector(); // data.position.ToVector() возвращает позицию из статистики в скрипте SerializableVector3 
            transform.eulerAngles = data.rotation.ToVector(); // аналогично поворачиваем
            GetComponent<NavMeshAgent>().enabled = true; // включили NavMeshAgent опсле перемещения объекта
        }



        // Ray lastRay; // это переменная вектор направления
        // lastRay = Camera.main.ScreenPointToRay(Input.mousePosition); // вектору направления присваиваем анправление из  main камеры в точку клика мышки
        // Debug.DrawRay(lastRay.origin, lastRay.direction * 100); // меняем от текущего вектора к вектору напрвления умноженному на 100
        // перемещение к объекту таргет при условии что установили NavMeshAgent
        //GetComponent<NavMeshAgent>().destination = target.position;

    }
}
