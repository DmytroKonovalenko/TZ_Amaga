using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CharacterData SelectedCharacter { get; private set; }

    private int power;
    public int Power
    {
        get => power;
        private set
        {
            power = value;
            OnPowerChanged?.Invoke(power); 
        }
    }

    public event Action<int> OnPowerChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSelectedCharacter(CharacterData character)
    {
        SelectedCharacter = character;
    }

    public void AddPower(int amount) => Power += amount;
    public void RemovePower(int amount) => Power = Mathf.Max(0, Power - amount);
    public void SetPower(int value) => Power = Mathf.Max(0, value);
}
