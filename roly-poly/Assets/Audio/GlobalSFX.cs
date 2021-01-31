using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GlobalSFX : AudioController
{
    private static GlobalSFX instance;
    public static GlobalSFX Instance
    {
        get
        {
            return instance;
        }
    }

    [Serializable]
    private class SFXList
    {
        public SFX gameStart = null;
        public SFX gameSave = null;
        public SFX hitGround = null;
        public SFX switchMode = null;
        public SFX playerHurt = null;
        public SFX playerDead = null;
        public SFX unlockAbility = null;
        public SFX killEnemy = null;
        public SFX dribble = null;
        public SFX boostBallCharge = null;
        public SFX boostBallRelease = null;
        public SFX bugBlast = null;
    }

    [SerializeField]
    private SFXList soundEffects = new SFXList();
    private List<SFX> sfxQueue;
    private IEnumerator playSFXAtEndOfFrame;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        sfxQueue = new List<SFX>();
    }
    public void PlayGameStart()
    {
        QueueSFX(soundEffects.gameStart);
    }
    public void PlayGameSave()
    {
        QueueSFX(soundEffects.gameSave);
    }

    public void PlayHitGround()
    {
        QueueSFX(soundEffects.hitGround);
    }

    public void PlaySwitchMode()
    {
        QueueSFX(soundEffects.switchMode);
    }
    public void PlayUnlockAbility()
    {
        QueueSFX(soundEffects.unlockAbility);
    }
    public void PlayKillEnemy()
    {
        QueueSFX(soundEffects.killEnemy);
    }
    public void PlayPlayerHurt()
    {
        QueueSFX(soundEffects.playerHurt);
    }
    public void PlayPlayerDead()
    {
        QueueSFX(soundEffects.playerDead);
    }
    public void PlayDribble()
    {
        QueueSFX(soundEffects.dribble);
    }
    public void PlayBoostBallCharge()
    {
        QueueSFX(soundEffects.boostBallCharge);
    }
    public void PlayBoostBallRelease()
    {
        QueueSFX(soundEffects.boostBallRelease);
    }
    public void PlayBugBlast()
    {
        QueueSFX(soundEffects.bugBlast);
    }


    //Allows only one sfx of each type to played in a single frame to prevent very loud sounds
    private void QueueSFX(SFX sfx)
    {
        if (!sfxQueue.Contains(sfx))
            sfxQueue.Add(sfx);
        if (playSFXAtEndOfFrame == null)
        {
            playSFXAtEndOfFrame = PlaySFXAtEndOfFrame();
            StartCoroutine(playSFXAtEndOfFrame);
        }
    }

    private IEnumerator PlaySFXAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        foreach (SFX sfx in sfxQueue)
        {
            PlayLingeringSFX(sfx);
        }
        playSFXAtEndOfFrame = null;
        sfxQueue.Clear();
    }



}
