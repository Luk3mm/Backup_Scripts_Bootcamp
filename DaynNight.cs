using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DaynNight : MonoBehaviour
{
    [SerializeField]
    private Transform directionalLight;
    [SerializeField]
    [Tooltip("Aqui voce adiciona o dia em segundos")]
    private int dayDuration;
    [SerializeField]
    TextMeshProUGUI timeText;

    private float seconds;
    private float multiplier;


    // Start is called before the first frame update
    void Start()
    {
        multiplier = 86400 / dayDuration;
    }

    // Update is called once per frame
    void Update()
    {
        seconds += Time.deltaTime * multiplier;

        if(seconds >= 86400)
        {
            seconds = 0;
        }

        ProcessSky();
        CalculateTime();
    }

    private void ProcessSky()
    {
        float rotationX = Mathf.Lerp(-90, 270, seconds / 86400);

        directionalLight.rotation = Quaternion.Euler(rotationX, 0, 0);
    }

    private void CalculateTime()
    {
        timeText.text = TimeSpan.FromSeconds(seconds).ToString(@"hh\:mm");
    }
}
