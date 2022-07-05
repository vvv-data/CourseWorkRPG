using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab; // переменная гейм обж который будем брать с префаба, в сцене его нет пока

        static bool hasSpawned = false; // переменна показывает объект уже добавлен в сцену или нет

        private void Awake()
        {
            if (hasSpawned) return;  // если уже объект добавлен то возвращаемся ничего не делаем

            SpawnPersistentObjects(); // добавляем объект в сцену

            hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab); // добавляем объект в сцену из префаба persistentObjectPrefab
            DontDestroyOnLoad(persistentObject); // не разрушаем объект при перезагрузке новой сцены для музыки или затемнения и тд.
        }
    }

}