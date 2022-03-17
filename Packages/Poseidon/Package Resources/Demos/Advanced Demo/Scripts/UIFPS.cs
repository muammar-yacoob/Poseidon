using UnityEngine;
using UnityEngine.UI;

public class UIFPS : MonoBehaviour
{
    private Text uiFPS;
    [SerializeField] [Range(0.2f, 5f)] private float refreshRate = 1f;
    private float timer;
    [SerializeField] private int maxSampleSize = 100;
    private int takenSamples = 0;
    private float accumelativeFPS;


    private void Awake() => uiFPS = GetComponent<Text>();

    private void Update()
    {

        if (timer >= refreshRate)
        {
            timer = 0;

            if (takenSamples >= maxSampleSize)
            {
                takenSamples = 0;
                accumelativeFPS = 0;
            }

            accumelativeFPS += 1 / Time.deltaTime;
            takenSamples++;

            uiFPS.text = $"\tFPS: {Mathf.RoundToInt(1 / Time.deltaTime)}" +
                         $"\n\tAvg FPS: {Mathf.RoundToInt(accumelativeFPS / takenSamples)}";
        }
        timer += Time.deltaTime;
    }
}
