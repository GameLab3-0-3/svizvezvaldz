using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Exit")]
    public GameObject enterTrigger;
    public GameObject exitTrigger;
    public bool anomaly;

    [Header("Counter")]
    public float maxCounter;
    public float counter;

    public static GameManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            return;
        }
        instance = this;
    }
    #region RriggerZones counter
    public void UpdateCounter()
    {
        if (!anomaly)
        {
            counter++;
            if (counter > maxCounter) counter = maxCounter;
            if (counter == maxCounter)
                exitTrigger.SetActive(false);
        }
        else
        {
            counter = 0;
        }
    }
    public void Loop()
    {
        if (counter > 0)
        {
            counter--;
        }

        if (counter < maxCounter)
            exitTrigger.SetActive(true);
    }

    private void Update()
    {
        if (counter == 0)
            enterTrigger.SetActive(false);
        else if (counter > 0)
            enterTrigger.SetActive(true);
    }
    #endregion
}
