using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMState
{

    

    public virtual void InitState(FSMController c) {}

    public virtual void EnterState() {}

    public virtual void Tick() {}

    public virtual void ExitState() {}
}

