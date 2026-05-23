using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("MainLvl");
    }
    public void Quitgame()
    {
        Application.Quit();
    }
}
