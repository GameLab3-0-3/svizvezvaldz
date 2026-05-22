using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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


    public static event Action OnAltDisabled;

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
            exitTrigger.SetActive(true);
    }
    public void UpdateChooser()
    {
        //se sono presenti anomalie il trigger ti aumenta il counter e nel caso in cui il counter raggiunge il massimo disattiva l'uscita
        if (anomaly)
        {
            StartCoroutine(CorridorCounter());
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
        }
        anomaly = false;
    }

    IEnumerator CorridorCounter()
    {
        counter++;
        OnAltDisabled?.Invoke();
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
        SceneManager.LoadScene("Credits");
    }
    #endregion
}
