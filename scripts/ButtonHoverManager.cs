using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonHoverManager : MonoBehaviour
{
    private List<ButtonHover> buttonHovers = new List<ButtonHover>();

    private void Awake()
    {
        // Find and store all ButtonHover components in the scene
        ButtonHover[] buttons = FindObjectsOfType<ButtonHover>();
        buttonHovers.AddRange(buttons);
    }

    public void SetHoverEnabledForAll(bool enabled)
    {
        foreach (ButtonHover buttonHover in buttonHovers)
        {
            buttonHover.SetHoverEnabled(enabled);
        }
    }

    public void SetHoverForOne(ButtonHover buttonHover, bool enabled)
    {
        buttonHover.SetHoverEnabled(enabled);
    }
}