using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void OnPlayButton()
   {
      SceneManager.LoadScene("Mission");
   }

   public void OnQuitButton()
   {
      Application.Quit();
      Debug.Log("Quitting Game ...");
   }
}
