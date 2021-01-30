using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private PlayerController p;
    [System.Serializable]
    public struct DribbleAbility
    {
        public bool unlocked;
        public Dribble ability;
    }
    [System.Serializable]
    public struct StickyFeetAbility
    {
        public bool unlocked;
        public StickyFeet ability;
    }
    [System.Serializable]
    public struct BoostBallAbility
    {
        public bool unlocked;
        public BoostBall ability;
    }
    [System.Serializable]
    public struct BugBlastAbility
    {
        public bool unlocked;
        public BugBlast ability;
    }
    [System.Serializable]
    public struct Abilities
    {
        public DribbleAbility dribble;
        public StickyFeetAbility stickyFeet;
        public BoostBallAbility boostBall;
        public BugBlastAbility bugBlast;
    }
    public Abilities abilities;


    public void SetPlayerController(PlayerController p)
    {
        this.p = p;
    }

    public PlayerState CheckAbilities()
    {
        if (p.inputs.dribble && abilities.dribble.unlocked && p.physics.IsRoll())
        {
            return new DribbleState(p, abilities.dribble.ability);
        }
        else if (p.inputs.boostBall && abilities.boostBall.unlocked && p.physics.IsRoll())
        {
            return new BoostBallState(p, abilities.boostBall.ability);
        }
        return null;
    }

    public void UnlockAll()
    {
        abilities.dribble.unlocked = true;
        abilities.stickyFeet.unlocked = true;
        abilities.boostBall.unlocked = true;
        abilities.bugBlast.unlocked = true;
    }
    public void LockAll()
    {
        abilities.dribble.unlocked = false;
        abilities.stickyFeet.unlocked = false;
        abilities.boostBall.unlocked = false;
        abilities.bugBlast.unlocked = false;

    }
}