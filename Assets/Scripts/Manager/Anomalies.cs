using System.Collections;
using TMPro;
using UnityEngine;

public class Anomalies : MonoBehaviour
{
    [Header("npc")]
    public float timer;
    public bool NPCDeactivation;
    public GameObject NPCHead;
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
    [SerializeField] GameObject LightChanger;
    public bool noLight;
    public bool redLight;
    [SerializeField] Light GameLight1;
    [SerializeField] Light GameLight2;
    [SerializeField] Light GameLight3;

    [Header("UI")]
    [SerializeField] TMP_Text anomaly_Text;
    string anomalyType;

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
        Light_Changer.OnNoLights += NoLight;
        Light_Changer.OnRedLights += RedLight;
    }
    private void OnDisable()
    {
        Anomaly_Chooser.OnAnomalies -= ChooseAnomaly;
        GameManager.OnAltDisabled -= ResetAlt;
        Light_Changer.OnNoLights -= NoLight;
        Light_Changer.OnRedLights -= RedLight;;
    }

    private void Start()
    {
        originalCPos = Ceiling.transform.position;
        anomalyType = "None";
    }
    private void Update()
    {
        if(anomalyType == "NPC manca")
        {
            timer += Time.deltaTime;
        }
        anomaly_Text.text = "Anomaly: " + anomalyType;
    }
    #region Anomalies
    private void ChooseAnomaly()
    {
        int anomaly = Random.Range(0, 101);
        Debug.Log(anomaly);
        if (anomaly <= 25)
        {
            GameManager.instance.anomaly = false;
            anomalyType = "None";
        }
        else if (anomaly > 25 && anomaly <= 100)
        {
            GameManager.instance.anomaly = true;
            float type = Random.Range(9, 9);
            switch (type)
            {
                case 0:
                    Poster2();
                    break;
                case 1:
                    Poster4();
                    break;
                case 2:
                    GigaPoster();
                    break;
                case 3:
                    EyePoster();
                    break;
                case 4:
                    CeilingDown();
                    break;
                case 5:
                    anomalyType = "No Light";
                    LightChanger.SetActive(true);
                    noLight = true;
                    break;
                case 6:
                    anomalyType = "Red Light";
                    LightChanger.SetActive(true);
                    redLight = true;
                    break;
                case 7:
                    anomalyType = "Poster mancanti";
                    MissingPoster();
                    break;
                case 8:
                    anomalyType = "NPC manca";
                    NPCMissing();
                    break;
                case 9:
                    anomalyType = "NPC BIGHEAD";
                    BIGHEAD();
                    break;
                    /*
                    case 10:
                        Debug.Log("NPC grande");
                        break;
                    case 11:
                        Debug.Log("telecamere che si muovono");
                        break;
                    case 12:
                        Debug.Log("testa che gira");
                        break;
                    case 13:
                        Debug.Log("ethel");
                        break;
                    case 14:
                        Debug.Log("porta aperta");
                        break;
                    case 15:
                        Debug.Log("segnale uscita al contrario");
                        break;
                    case 16:
                        Debug.Log("-");
                        break;
                    case 17:
                        Debug.Log(".-");
                        break;
                    case 18:
                        Debug.Log("-.");
                        break;
                    case 19:
                        Debug.Log(".-.");
                        break;
                    case 20:
                        Debug.Log("-.-");
                        break;
                    */
            }
        }
    }
    #region posters
    private void Poster2()
    {
        poster2_Normal.SetActive(false);
        poster2_ALt.SetActive(true);
        anomalyType = "Poster 2";
    }
    private void Poster4()
    {
        poster4_Normal.SetActive(false);
        poster4_Alt.SetActive(true);
        anomalyType = "Poster 4";
    }
    private void GigaPoster()
    {
        posterSet_Normal.SetActive(false);
        posterSet_Alt.SetActive(true);
        anomalyType = "Sizes";
    }
    private void EyePoster()
    {
        eyePoster_Normal.SetActive(false);
        eyePoster_Alt.SetActive(true);
        anomalyType = "Eye Poster";
    }
    private void MissingPoster()
    {
        posterSet_Normal.SetActive(false);
    }
    #endregion
    #region Enviroment
    private void CeilingDown()
    {
        StartCoroutine(LerpCeiling());
        anomalyType = "Ceiling";
    }
    private void NoLight()
    {
        GameLight1.color = Color.black;
        GameLight2.color = Color.black;
        GameLight3.color = Color.black;
    }
    private void RedLight()
    {
        GameLight1.color = Color.red;
        GameLight2.color = Color.red;
        GameLight3.color = Color.red;
    }
    #endregion

    IEnumerator LerpCeiling()
    {
        Vector3 startPos = Ceiling.transform.position;
        Vector3 targetPos = new(originalCPos.x, 2, originalCPos.z);

        float duration = 10f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Ceiling.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Ceiling.transform.position = targetPos;
        if (Ceiling.transform.position == targetPos - new Vector3(0, targetPos.y - 2, 0))
        {
            GameManager.instance.Dead();
        }
    }
    #region NPC
    private void NPCMissing()
    {
        NPCDeactivation = true;
        
    }
    #region NPC testa
    private void BIGHEAD()
    {
        NPCHead.transform.localScale = new Vector3(0.0199999996f, 0.0199999996f, 0.0199999996f);
    }
    #endregion
    #endregion
    #endregion Anomalies
    private void ResetAlt()
    {
        anomalyType = "None";
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
        //Eye poster
        eyePoster_Normal.SetActive(true);
        eyePoster_Alt.SetActive(false);
        #endregion
        #region Enviroment
        StopAllCoroutines();
        Ceiling.transform.position = originalCPos;
        LightChanger.SetActive(false);
        noLight = false;
        redLight = false;
        GameLight1.color = Color.white;
        GameLight2.color = Color.white;
        GameLight3.color = Color.white;
        #endregion
        #region NPC
        NPCDeactivation = false;
        NPCHead.transform.localScale = new Vector3(0.00775404554f, 0.00342674972f, 0.00701166457f);
        #endregion
    }
}
