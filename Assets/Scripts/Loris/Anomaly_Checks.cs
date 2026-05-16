using UnityEngine;

public class Anomaly_Checks : MonoBehaviour
{
    [Header("Riferimenti")]
    [Tooltip("Qui mettiamo il Player per teletrasportarlo")]
    public GameObject Player;

    [Tooltip("Qui passiamo la posizione per teletrasportare il PLayer")]
    public Transform destination;

    [Tooltip("Distanza di sicurezza per evitare che il Player venga loopato dentro le TriggerZone")]
    public float tpOffset;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
