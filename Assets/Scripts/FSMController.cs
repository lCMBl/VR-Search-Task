using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMController : MonoBehaviour
{
    protected FSMState currentState;

    public abstract FSMState GetInitialState();

    void Awake() {
        SetState(GetInitialState());
    }

    // Update is called once per frame
    void Update()
    {
        MonoBehaviourUpdate();
        if (currentState != null) {
            currentState.Tick();
        }
    }

    protected virtual void MonoBehaviourUpdate() {
        // this is called before the current state ticks every frame, in case the controller needs to do something
        // with it's own update call.
    }

    protected void SetState(FSMState state) {
        if (currentState != null) {
            currentState.ExitState();
        }
        currentState = state;
        currentState.InitState(this);
        currentState.EnterState();
    }

    

    void OnDestroy() {
        if (currentState != null) {
            currentState.ExitState();
        }
    }

    public class FSMState
    {

        protected FSMController controller;
        public virtual void InitState(FSMController c) {
            this.controller = c;
        }

        public virtual void EnterState() {}

        public virtual void Tick() {}

        public virtual void ExitState() {}

        protected void SetState(FSMState state) {
            controller.SetState(state);
        }
    }
}
