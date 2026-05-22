using UnityEngine;

public class Anomalies : MonoBehaviour
{
    [Header("Posters")]
    public GameObject Poster2_Normal;
    public GameObject Poster4_Normal;
    public GameObject PosterSet_Normal;
    [Header("----------")]
    public GameObject Poster2_ALt;
    public GameObject Poster4_Alt;
    public GameObject PosterSet_Alt;

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
            if (type <= 6.66f)
            {
                Poster2();
                return;
            }
            else if (type > 6.66f && type <= 13.33f)
            {
                Poster4();
                return;
            }
            else if (type > 13.33f && type < 21)
            {
                GigaPoster();
                return;
            }
            /*
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
            */
        }
    }

    private void Poster2()
    {
        Poster2_Normal.SetActive(false);
        Poster2_ALt.SetActive(true);
    }
    private void Poster4()
    {
        Poster4_Normal.SetActive(false);
        Poster4_Alt.SetActive(true);
    }

    private void GigaPoster()
    {
        PosterSet_Normal.SetActive(false);
        PosterSet_Alt.SetActive(true);
    }
    #endregion Anomalies
    private void ResetAlt()
    {
        #region poster2/4
        //poster 2
        Poster2_Normal.SetActive(true);
        Poster2_ALt.SetActive(false);
        //poster 4
        Poster4_Normal.SetActive(true);
        Poster4_Alt.SetActive(false);
        //poster Set
        PosterSet_Normal.SetActive(true);
        PosterSet_Alt.SetActive(false);
        #endregion
    }
}
