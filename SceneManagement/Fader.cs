using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup; // получили переменную с классом канвас групп в нем можно менять прозрачность от 0 до 1
        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>(); // получили переменную с классом канвас групп в нем можно менять прозрачность от 0 до 1
            
        }
        
        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }
        public IEnumerator FadeOut(float time)
        {
            while(canvasGroup.alpha < 1) // alfa is not 1
            {
                canvasGroup.alpha += Time.deltaTime / time; // каждый кадр прибавляем к альфе коофициент изменения в зависимости от прошедшего времени между кадрами, деленное на общее время за которое долно произойти изменение прозрачности
                yield return null;
                //time -= time.deltaTime;
            }
        }
        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0) // alfa is not 1
            {
                canvasGroup.alpha -= Time.deltaTime / time; // каждый кадр прибавляем к альфе коофициент изменения в зависимости от прошедшего времени между кадрами, деленное на общее время за которое долно произойти изменение прозрачности
                yield return null;
                //time -= time.deltaTime;
            }
        }
    }

}