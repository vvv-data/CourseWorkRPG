using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables; // библиотека для корректной работы тригера с Плейболс Дирректор

namespace RPG.Cinematics
{
    public class CinemaricTrigger : MonoBehaviour
    {
        bool alredyTriggered = false; // срабатывал ли тригер
        private void OnTriggerEnter(Collider other)
        {
            if (!alredyTriggered && other.gameObject.tag == "Player") // если тригер не срабатывал и столкновение с игроком то запускаем анимацию моментов игры
            {
                GetComponent<PlayableDirector>().Play();
                alredyTriggered = true;
            }
            
        }
    }

}