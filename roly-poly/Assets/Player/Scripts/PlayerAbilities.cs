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
        public bool usedInAir;
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
    public void Update()
    {
        if(p.physics.IsGrounded() && abilities.bugBlast.usedInAir)
            abilities.bugBlast.usedInAir = false;

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
        else if(p.inputs.bugBlast && abilities.bugBlast.unlocked && !abilities.bugBlast.usedInAir && !p.physics.IsRoll() && !p.physics.IsGrounded())
        {
            abilities.bugBlast.usedInAir = true;
            return new BugBlastState(p, abilities.bugBlast.ability);
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