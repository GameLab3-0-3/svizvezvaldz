using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    [Tooltip("Questo è il trigger d'entarta, quello che ti teletrasporta alla fine del corridoio precedente")]
    public GameObject enterTrigger;
    [Tooltip("Questo è il trigger d'uscita, quello che ti teletrasporta all'inizio del corridoio successivo")]
    public GameObject exitTrigger;
    [Tooltip("Questo è il trigger che decide se andare avanti o dietro, attivva/disattiva il bool, decide quale anomalia deve esserci (se il bool è attivo) e ti teletrasporta all'entrata se torni indietro")]
    public GameObject anomalyChooser;
    [Tooltip("questa è la light source che si attiva una volta che sei passato x volte (dove x è il maxCounter), funziona come trigger per i titoli di coda")]
    public GameObject lightSource;
    [Tooltip("bool utilizzato per far sapere al gioco se o meno nel corridoio ci sono anomalie")]
    public bool anomaly;

    [Header("Counter")]
    [Tooltip("il numero massimo di volte che il Player deve raggiungere per finire il gioco")]
    public float maxCounter;
    [Tooltip("Counter che aumenta nel caso in cui andiamo nella direzione giusta, si resetta se sbagliamo, e disattiva il trigger in fondo per permettere al giocatore di finire il gioco")]
    public float counter;
    [Tooltip("Counter necessario per sapere quante volte il Player è entrato nell'Anomaly Chooser in modo da far funzionare il Game loop")]
    public int triggerCounter;

    [Header("UI")]
    [SerializeField] TMP_Text progress_Text;
    
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
        progress_Text.text = "Corridor: " + counter;
    }
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
        //inputs.Player.Pause.started += ChangeState;
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
        //inputs.Player.Pause.started += ChangeState;
        if (UIManager.Instance == null || UIManager.Instance.PauseScreen == null) return;
        UIManager.Instance.PauseScreen.SetActive(true);
    }
    #region TriggerZones counter


    //questa funzione viene richiamata in "Anomaly_Checks" nel trigger d'uscita per far funzionare il game Loop
    //questa funzione viene messa in "Anomaly_Checks" nel trigger d'entrata per creare una sorta di loop
    public void Loop()
    {
        //se il counter è maggiore di 0 e non è presente nessuna anomalia il counter scende di uno, impedendo al Player di sfruttare il tp come metodo di fine veloce, sostanzialmente creando di fatto il loop
        if (counter > 0 && !anomaly)
            counter--;
        //altrimenti se è presente un'anomalia il counter aumenta (questa parte è da spostare nel trigger "anomalyChooser" per finalizzare il game loop)
        //else if (counter > 0 && anomaly)
        //    counter++;
        //questa ultimo controllo è per evitare, un'altra volta, che il Player possa sfruttare il tp come metodo veloce per finire il gioco. infatti se volesse ritornare indietro, il Player tornerebbe alla fine del corridoio precedente e il trigger d'uscita si riattiverebbe impedendo di andare alla fine senza aver percorso l'ultimo corridoio
        if (counter < maxCounter)
        {
            anomalyChooser.SetActive(true);
            exitTrigger.SetActive(true);
        }
    }
    public void UpdateChooser()
    {
        //se sono presenti anomalie il trigger ti aumenta il counter e nel caso in cui il counter raggiunge il massimo disattiva l'uscita
        if (anomaly)
        {
            StartCoroutine(CorridorCounter());
            OnAltDisabled?.Invoke();
        }
        //se non è presente un'anomalia resetta il counter (skill issue negro)
        else
        {
            counter = 0;
        }
        triggerCounter = 0;
    }
    public void UpdateCounter()
    {
        triggerCounter = 0;
        //se non sono presenti anomalie il trigger ti aumenta il counter e nel caso in cui il counter raggiunge il massimo disattiva l'uscita
        if (!anomaly)
        {
            StartCoroutine(CorridorCounter());
        }
        //se è presente un'anomalia resetta il counter (skill issue negro)
        else
        {
            counter = 0;
            OnAltDisabled?.Invoke();
        }
        anomaly = false;
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

    public void Credits()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Credits");
    }
    #endregion
}
