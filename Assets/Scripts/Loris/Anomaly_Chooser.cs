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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            if (GameManager.instance.triggerCounter == 0)
            {
                int anomaly = Random.Range(0, 101);
                Debug.Log(anomaly);
                if (anomaly <= 50)
                    GameManager.instance.anomaly = false;
                else if (anomaly > 50 && anomaly <= 100)
                    GameManager.instance.anomaly = true;
            }
            GameManager.instance.triggerCounter++;
            Vector3 relativePos = Player.transform.position - transform.position;
            Vector3 destinationPos = destination.position + new Vector3(0, relativePos.y, relativePos.z);

            if (GameManager.instance.triggerCounter >= 2)
            {
                destinationPos.x += tpOffset;
                Player.transform.rotation = Quaternion.Euler(Player.transform.rotation.x,Player.transform.rotation.y + 90,Player.transform.rotation.z);
                GameManager.instance.UpdateChooser();
                Player.transform.position = destinationPos;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
