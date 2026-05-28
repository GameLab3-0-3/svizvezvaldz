using UnityEngine;

public class CameraALT : MonoBehaviour
{
    [SerializeField] float lookRadius = 100f;
    [SerializeField] float turnSpeed = 5f;
    Transform target;
    void Start()
    {
        target = GameManager.instance.Player.transform;
    }
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if(distance <= lookRadius)
        {
            FaceTarget();
        }
    }
    void FaceTarget()
    {
        //prende le informazioni del target e le usa per trasformare la rotazione dell'enemy
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
