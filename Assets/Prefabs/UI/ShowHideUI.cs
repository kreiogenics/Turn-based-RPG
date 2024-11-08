using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowHideUI : MonoBehaviour
{
    [SerializeField] KeyCode toggleKey = KeyCode.Escape;
    [SerializeField] GameObject uiContainer = null;
    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        uiContainer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            AudioManager.PlayOpenMenu();
            uiContainer.SetActive(!uiContainer.activeSelf);
        }

        if (uiContainer.activeInHierarchy == true)
        {
            player.SetActive(false);
        }
        else
        {
            player.SetActive(true);
        }
    }
}
