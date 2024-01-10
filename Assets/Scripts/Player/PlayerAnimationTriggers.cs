using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    void AnimationMiddleTrigger1()
    {
        player.AnimationMiddleTrigger1();
    }
}
