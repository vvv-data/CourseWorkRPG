using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player;
        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl; // если проигрывается анимация моментов то вызывается функция DisableControl и игрок не управляется
            GetComponent<PlayableDirector>().stopped += EnableControl; // если остановилась проигрываться анимация моментов то вызывается функция EnableControl и игрок контролируется
            player = GameObject.FindWithTag("Player"); // присваиваем гейм обжект игрока по тэгу

        }
        void DisableControl(PlayableDirector pd)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction(); // останавливаем текущую акцию
            player.GetComponent<PlayerController>().enabled = false; // отключаем скрипт PlayerController чтобы персонаж не реагировал на щелчки во время просмотра моментов игры 
        }
        void EnableControl(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled = true; // включаем скрипт PlayerController чтобы персонаж снова реагировал

        }
    }

}