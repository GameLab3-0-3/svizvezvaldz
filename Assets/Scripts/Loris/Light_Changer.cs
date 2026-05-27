using System;
using UnityEngine;

public class Light_Changer : MonoBehaviour
{
    [Header("Riferimenti")]
    [Tooltip("Qui mettiamo il Player per teletrasportarlo")]
    public GameObject Player;

    public static event Action OnNoLights;
    public static event Action OnRedLights;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            if(Anomalies.instance.noLight)
                OnNoLights?.Invoke();
            else if(Anomalies.instance.redLight)
                OnRedLights?.Invoke();
        }
    }
}
