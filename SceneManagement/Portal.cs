using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI; 

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {   enum DestinationIdentifier  // создаем список мест назначения, порталов
        {
            A, B, C, D, E // это буквы порталов они должны совпадать у тех портплов кто в лруг друга переходит
        }
        
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint; // переменная координаты точки которая перед порталом
        [SerializeField] DestinationIdentifier destination;  // вывели список в инспектор, можно выбирать куда перемещасться на какой следующий портал
        [SerializeField] float fadeOutTime = 1f; // время затемнения при транзите в другую сцену
        [SerializeField] float fadeInTime = 2f; // время убирания затемнения после транзита в другую сцену
        [SerializeField] float fadeWaitTime = 0.5f; // ожидание затемнения после транзита в другую сцену?????
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                StartCoroutine(Transition()); //запускаем куратин транзита между сценами
            }
        }
        private IEnumerator Transition() // создаем куратин набор действий, который будет выполняться каждый кадр
        {
            if(sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set."); // если сцену загрузки не установили то останавливаем курантил
                yield break; 
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>(); // присваиваем переменной скрипт fader он при транзите сцен делает затемнение
            yield return fader.FadeOut(fadeOutTime);

            // save current lavel
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>(); // получили компоненты скрипта SavingWrapper он висит на префабе Saving
            wrapper.Save(); // сохранили данные прошлой сцены в файл
            
            yield return SceneManager.LoadSceneAsync(sceneToLoad);  // асинхронная загрузка LoadSceneAsync, загружаем следующую сцену

            // load current lavel
            wrapper.Load(); // загрузили сохраненные данные из файла в новую сцену
            
            Portal otherPortal = GetOtherPortal(); //получаем данные по новому порталу в новой сцене
            
            UpdatePlayer(otherPortal);  //передвигаем игрока в новй сцене согласно новому порталу

            wrapper.Save(); // сохранили данные новой сцены в файл

            yield return new WaitForSeconds(fadeWaitTime); // делаем задержку на fadeWaitTime

            yield return fader.FadeIn(fadeInTime); // убираем затемнение после загрузки новой сцены и установки игрока в нужно еместо

            Destroy(gameObject);  //разрушаем объект старого портала
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false; // отключили NavMeshAgent перед изменением позиции
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position); // чтобы правильно встал на новом месте перемещаем на точку функцией Warp
            player.transform.rotation = otherPortal.spawnPoint.rotation; // повернули игрока на spawnPoint
            player.GetComponent<NavMeshAgent>().enabled = true; // включили NavMeshAgent после изменением позиции
        }
        private Portal GetOtherPortal()
        {
           foreach(Portal portal in FindObjectsOfType<Portal>()) // тк тут буде 2 портала тот который старый не разрушен еще и новый, то нужен цикл чтобы найти новый
            {
                if (portal == this) continue; //если портал старый текущий то пропускаем
                if (portal.destination != destination) continue; // если значение destination портала не равно значению destination порталу назначения, то пропускаем
                return portal; // возвращаем если портал не текущий, значит новый
            }
            return null; // если портал не нашелся
        }

    }
}



