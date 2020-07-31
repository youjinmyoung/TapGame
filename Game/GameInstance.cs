using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : UnitySingleton<GameInstance>
{
    public bool isReincarnation = false;
    public int CurrentDiamond { get; set; }
    public int[] CurrentPassiveLevel { get; set; }
    public float coolDownTime = 0.0f;

    GameInstance()
    {
        CurrentPassiveLevel = new int[20];
    }

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);
    }
}