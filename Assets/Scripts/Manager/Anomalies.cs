using System.Collections;
using UnityEngine;

public class Anomalies : MonoBehaviour
{
    [Header("Posters")]
    [SerializeField] GameObject poster2_Normal;
    [SerializeField] GameObject poster4_Normal;
    [SerializeField] GameObject posterSet_Normal;
    [SerializeField] GameObject eyePoster_Normal;
    [Header("Alt Posters")]
    [SerializeField] GameObject poster2_ALt;
    [SerializeField] GameObject poster4_Alt;
    [SerializeField] GameObject posterSet_Alt;
    [SerializeField] GameObject eyePoster_Alt;
    [Header("Enviroment")]
    [SerializeField] GameObject Ceiling;
    Vector3 originalCPos;

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

    private void Start()
    {
        originalCPos = Ceiling.transform.position;
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
            if (type <= 4f)
            {
                Poster2();
                return;
            }
            else if (type > 4f && type <= 8f)
            {
                Poster4();
                return;
            }
            else if (type > 8f && type <= 12)
            {
                GigaPoster();
                return;
            }
            else if (type > 12f && type <= 16)
            {
                EyePoster();
                return;
            }
            else if (type > 16f && type < 21)
            {
                CeilingDown();
                return;
            }
            /*
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
    #region posters
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
    #endregion
    private void CeilingDown()
    {
        StartCoroutine(LerpCeiling());
    }
    IEnumerator LerpCeiling()
    {
        Vector3 startPos = Ceiling.transform.position;
        Vector3 targetPos = new (originalCPos.x, 2, originalCPos.z);

        float duration = 10f; 
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Ceiling.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Ceiling.transform.position = targetPos;
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
        #region Enviroment
        StopAllCoroutines();
        Ceiling.transform.position = originalCPos;
        #endregion
    }
}
