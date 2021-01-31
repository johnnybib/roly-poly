using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public List<GameObject> abilityIcons;
    public GameObject healthIconParent;
    public GameObject healthIconPrefab;
    private Dictionary<int, HealthIcon> healthIcons; // Uses 1 as base index
    private int currentHealth = 0;
    private int maxHealth = 0;

    void Start()
    {
        healthIcons = new Dictionary<int, HealthIcon>();
        foreach (GameObject obj in abilityIcons)
        {
            obj.SetActive(false);
        }
    }

    public void SetMaxHealth(int maxHealth)
    {
        if (this.maxHealth == maxHealth)
            return;

        // When new health is bigger than current
        if (maxHealth > healthIcons.Count)
        {
            for (int i = healthIcons.Count + 1; i <= maxHealth; i++)
            {
                GameObject newIconObject = Instantiate(healthIconPrefab);
                newIconObject.transform.parent = healthIconParent.transform;

                healthIcons.Add(i, newIconObject.GetComponent<HealthIcon>());
            }
        }
        else // When new health is smaller 
        {
            for (int i = healthIcons.Count; i > maxHealth; i--)
            {
                Destroy(healthIcons[i].gameObject);
            }
        }

        this.maxHealth = maxHealth;
    }

    public void SetCurrentHealth(int currentHealth)
    {
        if (this.currentHealth == currentHealth)
            return;
        this.currentHealth = currentHealth;

        for (int i = 1; i <= maxHealth; i++)
        {
            if (i > currentHealth)
            {
                healthIcons[i].SetEmpty();
            }
            else
            {
                healthIcons[i].SetFull();
            }
        }
    }

    public void UnlockAbility(AbilitiesToUnlock ability)
    {
        abilityIcons[(int)ability].SetActive(true);
    }

}
