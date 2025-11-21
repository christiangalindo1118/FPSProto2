using UnityEngine;
using UnityEngine.SceneManagement;

public class PrimaryObjective : MonoBehaviour
{
    [Header("Vehicle button")] 
    [SerializeField] private KeyCode vehicleButton = KeyCode.F;

    [Header("Generator Sound Effects and radius")]
    private float radius = 3f;

    public PlayerScript player;

    private void Update()
    {
        if (Input.GetKeyDown(vehicleButton) && Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
            //objective complete
        }
    }
}

