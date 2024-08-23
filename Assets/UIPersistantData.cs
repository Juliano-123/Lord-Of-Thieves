using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPersistantData : MonoBehaviour
{
    public static UIPersistantData Instance;

    private int MaxHealth = 3;
    private int CurrentHealth = 3;
    private int PuntosTotales = 0;

    [SerializeField]
    private int Level = 1;
    private int MostrosStompeados = 0;
    private int MostrosFaltaDestruir = 0;

    public int GetMaxHealth() { return MaxHealth; }
    public int GetCurrentHealth() {  return CurrentHealth; }
    public int GetPuntosTotales() {  return PuntosTotales; }
    public int GetLevel() { return Level; }
    public int GetMostrosStompeados() { return MostrosStompeados; }
    public int GetMostrosFaltaDestruir() { return MostrosFaltaDestruir; }

    public void SetMaxHealth(int maxHealth) { MaxHealth = maxHealth; }
    public void SetCurrentHealth(int currentHealth) { CurrentHealth = currentHealth; }
    public void SetPuntosTotales(int puntosTotales) { PuntosTotales = puntosTotales; }
    public void SetLevel(int level) { Level = level; }
    public void SetMostrosStompeados (int mostrosStompeados) { MostrosStompeados = mostrosStompeados; }
    public void SetMostrosFaltaDestruir(int mostrosfaltadestruir) { MostrosFaltaDestruir = mostrosfaltadestruir; }


    public void ResetAllData()
    {
        MaxHealth = 3;
        CurrentHealth = 3;
        PuntosTotales = 0;
        Level = 1;
        MostrosStompeados = 0;
        MostrosFaltaDestruir = 0;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    
    }


}
