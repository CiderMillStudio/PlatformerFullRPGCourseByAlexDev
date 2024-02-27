using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointParent : MonoBehaviour
{
    public void DisableAllCheckpointButtons()
    {
        UI_CheckpointActivationButton[] checkpointButtons;
        checkpointButtons = FindObjectsOfType<UI_CheckpointActivationButton>();

        foreach (UI_CheckpointActivationButton button in checkpointButtons)
        {
            button.DisableCheckpointButton();
        }
    }
}
