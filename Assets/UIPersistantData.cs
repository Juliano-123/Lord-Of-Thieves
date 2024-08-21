using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPersistantData : MonoBehaviour
{
    public static UIPersistantData Instance;

    private static int MaxHealth = 3;
    private static int CurrentHealth = 3;
    private static int PuntosTotales = 0;

    public int GetMaxHealth() { return MaxHealth; }
    public int GetCurrentHealth() {  return CurrentHealth; }
    public int GetPuntosTotales() {  return PuntosTotales; }


    public void SetMaxHealth(int maxHealth) { MaxHealth = maxHealth; }

    public void SetCurrentHealth(int currentHealth) { CurrentHealth = currentHealth; }

    public void SetPuntosTotales(int puntosTotales) { PuntosTotales = puntosTotales; }

    public void ResetAllData()
    {
        MaxHealth = 3;
        CurrentHealth = 3;
        PuntosTotales = 0;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


}
