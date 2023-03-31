using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int currentHealth;

    int amountOfHealth = 3;

    [SerializeField] Image[] healthImage;

    void Update()
    {
        HealthSystem();
    }

    void HealthSystem()
    {
        if (currentHealth > amountOfHealth)
        {
            currentHealth = amountOfHealth;
        }

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        for (int i = 0; i < healthImage.Length; i++)
        {
            if (i < currentHealth)
            {
                healthImage[i].gameObject.SetActive(true);
            }
            else
            {
                healthImage[i].gameObject.SetActive(false);
            }
        }
    }
}
