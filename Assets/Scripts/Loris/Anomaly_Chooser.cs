using System;
using UnityEngine;

public class Anomaly_Chooser : MonoBehaviour
{
    [Header("Riferimenti")]
    [Tooltip("Qui mettiamo il Player per teletrasportarlo")]
    public GameObject Player;

    [Tooltip("Qui passiamo la posizione per teletrasportare il PLayer")]
    public Transform destination;

    [Tooltip("Distanza di sicurezza per evitare che il Player venga loopato dentro le TriggerZone")]
    public float tpOffset;


    public static event Action OnAnomalies;
    private void Start()
    {
        GameManager.instance.spawnPoint = GameManager.instance.Index0.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            if (GameManager.instance.triggerCounter == 0)
            {
                OnAnomalies?.Invoke();
                if (!Anomalies.instance.NPCDeactivation)
                {
                    GameManager.instance.NPC.SetActive(true);
                    GameManager.instance.NPC.transform.position = GameManager.instance.spawnPoint;
                    WaiPoint.Instance.wayPointIndex = 0;
                }
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
