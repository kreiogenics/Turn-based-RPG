using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public TextMeshProUGUI levelText, bits;

    private int currentCharacterSelection = 0;
    public Image characterSprite;
    public RectTransform healthBar;
    public Image healthBarColor;
    public RectTransform mpBar;

    public float testHealth = 0.5f;

    private void Update()
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        if (testHealth < 0.01f)
        {
            testHealth = .01f;
        }

        if (testHealth > 1)
        {
            testHealth = 1;
        }

        if (testHealth <= 0.3f)
        {
            healthBarColor.color = new Color32(255, 0, 0, 255);
        }
        else
        {
            healthBarColor.color = new Color32(23, 166, 20, 255);
        }
        healthBar.localScale = new Vector3(testHealth, 1, 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
