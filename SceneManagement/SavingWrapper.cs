using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save"; // название файла сохранения
        [SerializeField] float fadeInTime = 0.2f;

        IEnumerator Start()  // куратин автоматически стартанет при старте
        {
            Fader fader = FindObjectOfType<Fader>(); // лоступ к скрипту затемнения
            fader.FadeOutImmediate(); // мнгновенное затемнение
            yield return  GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile); // при старте сохраняем прошлую сцену
            yield return fader.FadeIn(fadeInTime); // убираем затемнение
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load(); // если нажали клавишу L лоад то сохраняем в файл
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save(); // если нажали клавишу S сейф то сохраняем в файл
            }

        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile); // вызываем функцию из скрипта SavingSystem загрузки данных из файла
        }

        public void Load()
        {
            // call to saving system load
            GetComponent<SavingSystem>().Load(defaultSaveFile); // вызываем функцию из скрипта SavingSystem сохранения в файл
        }
    }
}
