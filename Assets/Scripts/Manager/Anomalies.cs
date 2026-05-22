using UnityEngine;

public class Anomalies : MonoBehaviour
{
    [Header("Posters")]
    public GameObject poster2_Normal;
    public GameObject poster4_Normal;
    public GameObject posterSet_Normal;
    public GameObject eyePoster_Normal;
    [Header("----------")]
    public GameObject poster2_ALt;
    public GameObject poster4_Alt;
    public GameObject posterSet_Alt;
    public GameObject eyePoster_Alt;

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
        GameManager.OnAltDisabled += ResetAlt;
    }
    private void OnDisable()
    {
        Anomaly_Chooser.OnAnomalies -= ChooseAnomaly;
        GameManager.OnAltDisabled -= ResetAlt;
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
            if (type <= 5f)
            {
                Poster2();
                return;
            }
            else if (type > 5f && type <= 10f)
            {
                Poster4();
                return;
            }
            else if (type > 10f && type <= 15)
            {
                GigaPoster();
                return;
            }
            else if (type > 15f && type < 21)
            {
                EyePoster();
                return;
            }
            /*
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
            */
        }
    }

    private void Poster2()
    {
        poster2_Normal.SetActive(false);
        poster2_ALt.SetActive(true);
    }
    private void Poster4()
    {
        poster4_Normal.SetActive(false);
        poster4_Alt.SetActive(true);
    }

    private void GigaPoster()
    {
        posterSet_Normal.SetActive(false);
        posterSet_Alt.SetActive(true);
    }
    private void EyePoster()
    {
        eyePoster_Normal.SetActive(false);
        eyePoster_Alt.SetActive(true);
    }
    #endregion Anomalies
    private void ResetAlt()
    {
        #region posters
        //poster 2
        poster2_Normal.SetActive(true);
        poster2_ALt.SetActive(false);
        //poster 4
        poster4_Normal.SetActive(true);
        poster4_Alt.SetActive(false);
        //poster Set
        posterSet_Normal.SetActive(true);
        posterSet_Alt.SetActive(false);
        //poster Set
        eyePoster_Normal.SetActive(true);
        eyePoster_Alt.SetActive(false);
        #endregion
    }
}
