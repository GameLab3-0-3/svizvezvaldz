using UnityEngine;

public class Anomalies : MonoBehaviour
{
    public GameObject Poster2Normal;
    public GameObject Poster2ALt;
    public GameObject Poster4Normal;
    public GameObject Poster4Alt;
    public static Anomalies instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            return;
        }
        instance = this;
    }
    private void OnEnable()
    {
        Anomaly_Chooser.OnAnomalies += ChooseAnomaly;
        Anomaly_Chooser.OnAltDisabled += ResetAlt;
    }
    private void OnDisable()
    {
        Anomaly_Chooser.OnAnomalies -= ChooseAnomaly;
        Anomaly_Chooser.OnAltDisabled -= ResetAlt;
    }

    #region Anomalies
    private void ChooseAnomaly()
    {
        int anomaly = Random.Range(0, 101);
        Debug.Log(anomaly);
        if (anomaly <= 50)
            GameManager.instance.anomaly = false;
        else if (anomaly > 50 && anomaly <= 100)
        {
            GameManager.instance.anomaly = true;
            float type = Random.Range(0, 21);
            if (type == 0)
            {
                Poster2();
                return;
            }
            else if (type == 1)
            {
                Poster4();
                return;
            }
            else if (type == 2)
            {
                Debug.Log("poster grande");
                return;
            }
            else if (type == 3)
            {
                Debug.Log("poster occhi");
                return;
            }
            else if (type == 4)
            {
                Debug.Log("tetto scende");
                return;
            }
            else if (type == 5)
            {
                Debug.Log("luci rosse");
                return;
            }
            else if (type == 6)
            {
                Debug.Log("luci spente");
                return;
            }
            else if (type == 7)
            {
                Debug.Log("segnale uscita al contrario");
                return;
            }
            else if (type == 8)
            {
                Debug.Log("NPC manca");
                return;
            }
            else if (type == 9)
            {
                Debug.Log("NPC veloce");
                return;
            }
            else if (type == 10)
            {
                Debug.Log("NPC grande");
                return;
            }
            else if (type == 11)
            {
                Debug.Log("telecamere che si muovono");
                return;
            }
            else if (type == 12)
            {
                Debug.Log("testa che gira");
                return;
            }
            else if (type == 13)
            {
                Debug.Log("ethel");
                return;
            }
            else if (type == 14)
            {
                Debug.Log("porta aperta");
                return;
            }
            else if (type == 15)
            {
                Debug.Log(".");
                return;
            }
            else if (type == 16)
            {
                Debug.Log("-");
                return;
            }
            else if (type == 17)
            {
                Debug.Log(".-");
                return;
            }
            else if (type == 18)
            {
                Debug.Log("-.");
                return;
            }
            else if (type == 19)
            {
                Debug.Log(".-.");
                return;
            }
            else if (type == 20)
            {
                Debug.Log("-.-");
                return;
            }
        }
    }

    private void Poster2()
    {
            Debug.Log("Poster2");
            Poster2Normal.SetActive(false);
            Poster2ALt.SetActive(true);
    }
    private void Poster4()
    {
        Debug.Log("Poster4");
        Poster4Normal.SetActive(false);
        Poster4Alt.SetActive(true);
    }
    #endregion Anomalies
    private void ResetAlt()
    {
        #region poster2/4
        //poster 2
        Poster2Normal.SetActive(true);
        Poster2ALt.SetActive(false);
        //poster 4
        Poster4Normal.SetActive(true);
        Poster4Alt.SetActive(false);
        #endregion
    }
}
