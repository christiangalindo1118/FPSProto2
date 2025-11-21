using UnityEngine;
using System.Collections;

public class Computer : MonoBehaviour
{
    [Header("Computer On/Off")]
    public bool LightsOn = true;

    private float radius = 2.5f;

    public Light lights;
   
    [Header("Computer Assign Things")]
    public PlayerScript player;

    [SerializeField] private GameObject ComputerUI;
    [SerializeField] private int showComputerUIFor = 5;

    private void Awake()
    {
        lights = GetComponent<Light>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(ShowComputerUI());
                LightsOn = false;
                lights.intensity = 0;
                //objective completed
                //sound effect
            }
        }
    }

    IEnumerator ShowComputerUI()
    {
        ComputerUI.SetActive(true);
        yield return new WaitForSeconds(showComputerUIFor);
        ComputerUI.SetActive(false);
    }
}