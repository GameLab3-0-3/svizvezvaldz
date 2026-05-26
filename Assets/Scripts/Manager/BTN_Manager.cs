using UnityEngine;
using UnityEngine.SceneManagement;

public class BTN_Manager : MonoBehaviour
{
    public void USure()
    {
        if(!UIManager.Instance.USureScreen.activeSelf)
            UIManager.Instance.USureScreen.SetActive(true);
        else
            UIManager.Instance.USureScreen.SetActive(false);
            UIManager.Instance.RestartYBtn.SetActive(false);
            UIManager.Instance.QuitYBtn.SetActive(false);
    }
    public void RestartBtn()
    {
        UIManager.Instance.Usure_Text.text = "Are You Sure you want to Restart the Game?";
        UIManager.Instance.RestartYBtn.SetActive(true);
    }
    public void QuitBtn()
    {
        UIManager.Instance.Usure_Text.text = "Are You Sure you want to Quit the Game?";
        UIManager.Instance.QuitYBtn.SetActive(true);
    }
    
    public void Restart()
    {
        SceneManager.LoadScene("MainLvl");
    }
    public void Quitgame()
    {
        Application.Quit();
    }
}
