using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject PauseScreen;
    public GameObject USureScreen;
    public TMP_Text Usure_Text;
    public GameObject RestartYBtn;
    public GameObject QuitYBtn;

    public GameObject blackScreen;
    public static UIManager Instance;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
}
