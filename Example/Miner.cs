using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using MinerStates;

public class Miner: MonoBehaviour
{
    public string currentState;
    public FlexFSM fsm;
    public MinerData minerData;

    void Start()
    {
        InitializeMinerData();
        MakeFSM();
    }

    void InitializeMinerData()
    {
        minerData = new MinerData();
        minerData.fatigue = 0;
        minerData.goldInPockets = 0;
        minerData.thirst = 0;
    }

    void MakeFSM()
    {
        fsm = new FlexFSM(gameObject);

        Mining mining = new Mining(this);
        Drinking drinking = new Drinking(this);
        Sleeping sleeping = new Sleeping(this);
        MakingBankDeposit makingBankDeposit = new MakingBankDeposit(this);
        
        fsm.AddState(StateID.Mining, mining);
        fsm.AddState(StateID.Drinking, drinking);
        fsm.AddState(StateID.Sleeping, sleeping);
        fsm.AddState(StateID.MakingBankDeposit, makingBankDeposit);

        fsm.ChangeState(StateID.Mining);

        fsm.Activate();
    }

    void Update()
    {
        if (fsm != null)
        {
            fsm.UpdateFSM();
            currentState = fsm.GetCurrentStateName();
        }
    }

    public void LogAction(string msg)
    {
        Debug.Log(System.DateTime.Now.ToString("HH:mm:ss") + " - " + msg);
    }
    public bool IsFatigued()
    {
        if (minerData.fatigue >= 500) return true;
        return false;
    }
    public bool HasPocketsFull()
    {
        if (minerData.goldInPockets >= 200) return true;
        return false;
    }
    public bool IsThirsty()
    {
        if (minerData.thirst >= 600) return true;
        return false;
    }
}