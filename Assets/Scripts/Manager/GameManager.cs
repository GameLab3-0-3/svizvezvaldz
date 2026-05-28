using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Status
{
    Running,
    Pause
}

public class GameManager : MonoBehaviour
{
    Status state;
    InputMap inputs;

    [Header("Exit")]
    [Tooltip("Questo � il trigger d'entarta, quello che ti teletrasporta alla fine del corridoio precedente")]
    public GameObject enterTrigger;
    [Tooltip("Questo � il trigger d'uscita, quello che ti teletrasporta all'inizio del corridoio successivo")]
    public GameObject exitTrigger;
    [Tooltip("Questo � il trigger che decide se andare avanti o dietro, attivva/disattiva il bool, decide quale anomalia deve esserci (se il bool � attivo) e ti teletrasporta all'entrata se torni indietro")]
    public GameObject anomalyChooser;
    [Tooltip("questa � la light source che si attiva una volta che sei passato x volte (dove x � il maxCounter), funziona come trigger per i titoli di coda")]
    public GameObject lightSource;
    [Tooltip("bool utilizzato per far sapere al gioco se o meno nel corridoio ci sono anomalie")]
    public bool anomaly;

    [Header("Counter")]
    [Tooltip("il numero massimo di volte che il Player deve raggiungere per finire il gioco")]
    public float maxCounter;
    [Tooltip("Counter che aumenta nel caso in cui andiamo nella direzione giusta, si resetta se sbagliamo, e disattiva il trigger in fondo per permettere al giocatore di finire il gioco")]
    public float counter;
    [Tooltip("Counter necessario per sapere quante volte il Player � entrato nell'Anomaly Chooser in modo da far funzionare il Game loop")]
    public int triggerCounter;

    [Header("UI")]
    [SerializeField] TMP_Text progress_Text;
    [SerializeField] float duration;

    [Header("Sounds")]
    [SerializeField] AudioClip bgSound;

    [Header("NPC")]
    public GameObject NPC;
    public GameObject Index0;
    public Vector3 spawnPoint;

    public static event Action OnAltDisabled;

    public static GameManager instance;
    private void Awake()
    {
        inputs = new InputMap();

        if (instance != null)
        {
            Destroy(instance);
            return;
        }
        instance = this;

        state = Status.Running;
    }
    private void Start()
    {
        StartCoroutine(BgMusic());
        
    }
    private void OnEnable()
    {
        if (inputs == null)
        {
            inputs = new InputMap();
        }
        inputs.Enable();
        inputs.Player.Pause.started += ChangeState;
        if (state == Status.Running)
            Running();
        else if (state == Status.Pause)
            Pause();
    }
    private void OnDisable()
    {
        inputs.Disable();
        inputs.Player.Pause.started -= ChangeState;
    }

    private void Update()
    {
        //funzione di debug, da cancellare prima di consegnare la build
        progress_Text.text = counter.ToString();
    }

    #region Pause_Functions
    private void ChangeState(InputAction.CallbackContext context)
    {
        if (state == Status.Running)
            Pause();
        else if (state == Status.Pause)
            Running();
    }
    public void Running()
    {
        state = Status.Running;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        if (UIManager.Instance == null || UIManager.Instance.PauseScreen == null) return;
        UIManager.Instance.PauseScreen.SetActive(false);
        UIManager.Instance.USureScreen.SetActive(false);
        UIManager.Instance.RestartYBtn.SetActive(false);
        UIManager.Instance.QuitYBtn.SetActive(false);
    }
    private void Pause()
    {
        state = Status.Pause;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        if (UIManager.Instance == null || UIManager.Instance.PauseScreen == null) return;
        UIManager.Instance.PauseScreen.SetActive(true);
    }
    #endregion
    
    #region TriggerZones counter
    //questa funzione viene messa in "Anomaly_Checks" nel trigger d'entrata per creare una sorta di loop
    public void Loop()
    {
        //se il counter � maggiore di 0 e non � presente nessuna anomalia il counter scende di uno, impedendo al Player di sfruttare il tp come metodo di fine veloce, sostanzialmente creando di fatto il loop
        if (counter >= 0 && !anomaly)
            counter--;
        //altrimenti se � presente un'anomalia il counter aumenta (questa parte � da spostare nel trigger "anomalyChooser" per finalizzare il game loop)
        //else if (counter > 0 && anomaly)
        //    counter++;
        //questa ultimo controllo � per evitare, un'altra volta, che il Player possa sfruttare il tp come metodo veloce per finire il gioco. infatti se volesse ritornare indietro, il Player tornerebbe alla fine del corridoio precedente e il trigger d'uscita si riattiverebbe impedendo di andare alla fine senza aver percorso l'ultimo corridoio
        if (counter < maxCounter)
        {
            anomalyChooser.SetActive(true);
            exitTrigger.SetActive(true);
        }
    }
    public void UpdateChooser()
    {
        NPCActivation();
        //se sono presenti anomalie il trigger ti aumenta il counter e nel caso in cui il counter raggiunge il massimo disattiva l'uscita
        if (anomaly)
        {
            StartCoroutine(CorridorCounter());
            OnAltDisabled?.Invoke();
        }
        //se non � presente un'anomalia resetta il counter (skill issue negro)
        else
        {
            counter = 0;
        }
        triggerCounter = 0;
    }
    public void UpdateCounter()
    {
        NPCActivation();
        triggerCounter = 0;
        //se non sono presenti anomalie il trigger ti aumenta il counter e nel caso in cui il counter raggiunge il massimo disattiva l'uscita
        if (!anomaly)
        {
            StartCoroutine(CorridorCounter());
        }
        //se � presente un'anomalia resetta il counter (skill issue negro)
        else
        {
            counter = 0;
            OnAltDisabled?.Invoke();
        }
        anomaly = false;
    }

    //funzione assegnata alla trigger "Stairs_Trigger"
    public void Credits()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Credits");
    }
    IEnumerator CorridorCounter()
    {
        counter++;
        if (counter > maxCounter) counter = maxCounter;
        if (counter == maxCounter)
        {
            anomalyChooser.SetActive(false);
            exitTrigger.SetActive(false);
            lightSource.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        anomaly = false;
    }
    #endregion
    #region GameOver
    public void Dead()
    {
        StartCoroutine(GameOver());
    }
    IEnumerator GameOver()
    {
        UIManager.Instance.blackScreen.SetActive(true);
        float time = 0;
        Image bSImg = UIManager.Instance.blackScreen.GetComponent<Image>();
        Color color = bSImg.color;
        while (time < duration)
        {
            time += Time.deltaTime;
            float opacity = Mathf.Lerp(0f, 1f, time / duration);

            color.a = opacity;
            bSImg.color = color;

            yield return null;
        }
        //in alternativa s� pu� anche ressettare il counter al posto di ricaricare la scena
        //counter = 0;
        SceneManager.LoadScene("MainLvl");

    }
    #endregion
    IEnumerator BgMusic()
    {
        while (true)
        {
            SoundManager.instance.PlaySfx(bgSound);
            

            yield return new WaitForSeconds(4.85f);
        }
    }
    private void NPCActivation()
    {
        NPC.SetActive(false);
        NPC.transform.position = spawnPoint;
    }
}
