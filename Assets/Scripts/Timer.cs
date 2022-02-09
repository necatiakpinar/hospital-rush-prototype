using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //Timer
    public float duration;
    public Image fillImage; 
    public bool timerFinished;
    public bool IsTimerRunning;
    private float time;
    public IEnumerator CO_Timer(float duration)
    {
        IsTimerRunning = true;
        float startTime = Time.time;
        time = duration;
        float value = 1;

        while(Time.time - startTime < duration)
        {
            time -= Time.deltaTime;
            value = time / duration;
            fillImage.fillAmount = value;
            if (!fillImage.gameObject.activeSelf)
            {
                StopPickingCounter();
                break;
            }
            yield return null;
        }
        
        if (fillImage.gameObject.activeSelf)  FinishPickingCounter();
    }

    //Timer
    public void StartPickingCounter()
    {
        timerFinished = false;
        IsTimerRunning = true;
        fillImage.gameObject.SetActive(true);
        fillImage.fillAmount = 1;
        StartCoroutine(CO_Timer(duration));
    }
    public void StopPickingCounter() //Reset Timer
    {
        time = duration; 
        fillImage.gameObject.SetActive(false);
        StopCoroutine("CO_Timer");
        IsTimerRunning = false;
    }

    public void FinishPickingCounter() //Reset Timer
    {
        timerFinished = true;
        IsTimerRunning = false;
        time = duration; 
        fillImage.gameObject.SetActive(false);
    }
}
