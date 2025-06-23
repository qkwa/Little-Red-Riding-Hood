using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    private List<TimedEffect> activeEffects = new List<TimedEffect>();
    private PlayerInput playerInput;

    // ��������, ������� ������������� ��������� ������� �������� ������
    public bool HasBuff => activeEffects.Count > 0;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        // ������������ ������� � �������� �������
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].Update(Time.deltaTime);

            if (!activeEffects[i].IsActive)
            {
                activeEffects[i].RemoveEffect(playerInput);
                activeEffects.RemoveAt(i);
                Debug.Log("Effect removed. HasBuff: " + HasBuff); // ��� �������
            }
        }
    }

    public void ApplyEffect(TimedEffect effect)
    {
        effect.ApplyEffect(playerInput);
        activeEffects.Add(effect);
        Debug.Log("Effect applied. HasBuff: " + HasBuff); // ��� �������
    }
}