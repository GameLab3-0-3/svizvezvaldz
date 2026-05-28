using UnityEngine;

public class NPCSTARTER : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject NPC;
    public Vector3 spawnPoint;
    void Start()
    {
        spawnPoint = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            NPC.transform.position = spawnPoint;
            WaiPoint.Instance.WayPointIdle = 0;
        }
        
    }
}
