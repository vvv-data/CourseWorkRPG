using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction; // переменная интерфейса IAction

        public void StartAction(IAction action) // это общий переключатель акций файта и хотьбы, его вызывают в скриптах файта и мувера
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                currentAction.Cancel(); // используем функцию тк она объявлена в интерфейсе IAction ее определения выполняются из разных файлов
            }
            currentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }








    }
}
