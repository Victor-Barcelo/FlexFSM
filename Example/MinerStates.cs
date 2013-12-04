using UnityEngine;
using System.Collections;

namespace MinerStates
{
    public enum StateID
    {
        Mining,
        Drinking,
        Sleeping,
        MakingBankDeposit
    }

    //==============================================Mining
    //====================================================
    public class Mining : FlexState
    {
        private Miner miner;
        private MinerData minerData;

        public Mining(Miner _miner)
        {
            miner = _miner;
            minerData = miner.minerData;
        }

        public override void OnEnter(GameObject owner)
        {
            miner.LogAction("Entering mine");
        }

        public override void OnExit(GameObject owner)
        {
            miner.LogAction("Leaving mine with " + minerData.goldInPockets + " gold in my pockets");
        }

        public override void Reason(GameObject owner)
        {
            if (miner.IsFatigued()) ChangeState(StateID.Sleeping);
            if (miner.HasPocketsFull()) ChangeState(StateID.MakingBankDeposit);
            if (miner.IsThirsty()) ChangeState(StateID.Drinking);
        }

        public override void Act(GameObject owner)
        {
            minerData.fatigue += 5;
            minerData.goldInPockets += 1;
            minerData.thirst += 3;
        }
    }
    //============================================Drinking
    //====================================================
    public class Drinking : FlexState
    {
        private Miner miner;
        private MinerData minerData;

        public Drinking(Miner _miner)
        {
            miner = _miner;
            minerData = miner.minerData;
        }

        public override void OnEnter(GameObject owner)
        {
            miner.LogAction("Entering bar");
        }

        public override void OnExit(GameObject owner)
        {
            miner.LogAction("Leaving bar");
        }

        public override void Reason(GameObject owner)
        {
            if (minerData.thirst <= 0) ChangeState(StateID.Mining);
        }

        public override void Act(GameObject owner)
        {
            minerData.thirst--;
        }
    }
    //============================================Sleeping
    //====================================================
    public class Sleeping : FlexState
    {
        private Miner miner;
        private MinerData minerData;

        public Sleeping(Miner _miner)
        {
            miner = _miner;
            minerData = miner.minerData;
        }

        public override void OnEnter(GameObject owner)
        {
            miner.LogAction("Entering home to take a nap");
        }

        public override void OnExit(GameObject owner)
        {
            miner.LogAction("Leaving home");
        }

        public override void Reason(GameObject owner)
        {
            if (minerData.fatigue <= 0) ChangeState(StateID.Mining);
        }

        public override void Act(GameObject owner)
        {
            minerData.fatigue--;
        }
    }
    //===================================MakingBankDeposit
    //====================================================
    public class MakingBankDeposit : FlexState
    {
        private Miner miner;
        private MinerData minerData;

        public MakingBankDeposit(Miner _miner)
        {
            miner = _miner;
            minerData = miner.minerData;
        }

        public override void OnEnter(GameObject owner)
        {
            miner.LogAction("Entering bank");
            minerData.goldInBank += minerData.goldInPockets;
        }

        public override void OnExit(GameObject owner)
        {
            miner.LogAction("Leaving bank, my account sums " + minerData.goldInBank + " pieces of gold");
        }

        public override void Reason(GameObject owner)
        {
            if (minerData.goldInPockets == 0) ChangeState(StateID.Mining);
        }

        public override void Act(GameObject owner)
        {
            minerData.goldInPockets = 0;
        }
    }
}
