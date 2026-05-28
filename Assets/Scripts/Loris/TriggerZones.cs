using System;
using UnityEngine;

public enum TriggerType
{
    Tp,
    Anomaly_Chooser,
    Light_Change,
    Stairs
}
public class TriggerZones : MonoBehaviour
{
    [Header("Type of Trigger")]
    [Tooltip("Qui si può scegliere che tipo di comportamento avrà la triggerZone")]
    public TriggerType type;
    [Header("Riferimenti")]
    [Tooltip("Qui mettiamo il Player per teletrasportarlo")]
    public GameObject Player;

    [Tooltip("Qui passiamo la posizione per teletrasportare il PLayer")]
    public Transform destination;

    [Tooltip("Distanza di sicurezza per evitare che il Player venga loopato dentro le TriggerZone")]
    public float tpOffset;


    public static event Action OnAnomalies;

    public static event Action OnNoLights;
    public static event Action OnRedLights;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            if (type == TriggerType.Tp)
            {
                Vector3 relativePos = Player.transform.position - transform.position;

                Vector3 destinationPos = destination.position + new Vector3(0, relativePos.y, relativePos.z);

                if (transform.position.x < destination.position.x)
                {
                    destinationPos.x -= tpOffset;
                    GameManager.instance.Loop();
                }
                else
                {
                    destinationPos.x += tpOffset;
                    GameManager.instance.UpdateCounter();
                }

                Player.transform.position = destinationPos;
            }

            if (type == TriggerType.Anomaly_Chooser)
            {
                if (GameManager.instance.triggerCounter == 0)
                {
                    OnAnomalies?.Invoke();
                }
                GameManager.instance.triggerCounter++;
                Vector3 relativePos = Player.transform.position - transform.position;
                Vector3 destinationPos = destination.position + new Vector3(0 + tpOffset, relativePos.y, relativePos.z);
                Quaternion destinationRot = Quaternion.Euler(Player.transform.rotation.x, Player.transform.rotation.y + 90, Player.transform.rotation.z);
                if (GameManager.instance.triggerCounter >= 2)
                {
                    Player.transform.SetPositionAndRotation(destinationPos, destinationRot);
                    GameManager.instance.UpdateChooser();
                }
            }

            if (type == TriggerType.Light_Change)
            {
                if (other.gameObject == Player)
                {
                    if (Anomalies.instance.noLight)
                        OnNoLights?.Invoke();
                    else if (Anomalies.instance.redLight)
                        OnRedLights?.Invoke();
                }
            }
            if (type == TriggerType.Stairs)
                GameManager.instance.Credits();
        }
    }
}
