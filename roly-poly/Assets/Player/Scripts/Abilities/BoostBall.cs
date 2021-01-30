using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Ability/BoostBall")]
public class BoostBall : ScriptableObject
{
    public float forcePerSecond;
    public float maxChargeTime;
}