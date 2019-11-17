using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FSM : MonoBehaviour
{
    /*
    public enum Transition
    {
        NullTransition = 0, // Use this transition to represent a non-existing transition in your system
    }
    */
    public class Transition
    {
        public static readonly Transition NullTransition = new Transition("NullTransition", StateID.NullStateID);

        public Transition(string transionName, StateID stateID)
        {
            this.TransionName = transionName;
            this.NextStateID = stateID;
        }

        public override string ToString()
        {
            return this.TransionName;
        }
        public string TransionName { get; private set; }
        public StateID NextStateID { get; private set; }
    }

    public class StateID
    {
        protected static int stateAmount = 0;
        public static readonly StateID NullStateID = new StateID(stateAmount++);

        protected StateID(int value)
        {
            this.Value = value;
        }

        public int Value { get; private set; }
    }

    protected List<State> states;
    public State currentState;
    public StateID currentStateID;
    public bool transitionLock = false;

    protected virtual void Awake()
    {
        states = new List<State>();
    }

    protected virtual void Update()
    {
        currentState.Act();
    }

    public void AddState(State s)
    {
        // Check for Null reference before deleting
        if (s == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }

        // First State inserted is also the Initial state,
        //   the state the machine is in when the simulation begins
        if (states.Count == 0)
        {
            states.Add(s);
            currentState = s;
            return;
        }

        // Add the state to the List if it's not inside it
        foreach (State state in states)
        {
            if (state.stateID == s.stateID)
            {
                Debug.LogError("FSM ERROR: Impossible to add state " + s.stateID.ToString() +
                               " because state has already been added");
                return;
            }
        }
        states.Add(s);
    }

    public void DeleteState(StateID id)
    {
        // Check for NullState before deleting
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        // Search the List and delete the state if it's inside it
        foreach (State state in states)
        {
            if (state.stateID == id)
            {
                states.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
                       ". It was not on the list of states");
    }
    public void ReleaseTransitionLock()
    {
        transitionLock = false;
    }
    public void PerformTransition(string transitionName, bool transitionLockParam = false)
    {
        transitionLock = transitionLockParam;
        Debug.Log("Perform Transition...");
        // Check for NullTransition before changing the current state
        if (transitionName == "NullTransition")
        {
            Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }


        // Check if the currentState has the transition passed as argument
        StateID id = currentState.GetOutputState(transitionName);
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
                           " for transition " + transitionName);
            return;
        }

        // Update the currentStateID and currentState		
        currentStateID = id;
        foreach (State state in states)
        {
            if (state.stateID == currentStateID)
            {
                StartCoroutine(PerformTransitionCoroutine(state));
                // Do the post processing of the state before setting the new one

                break;
            }
        }

    } // PerformTransition()

    IEnumerator PerformTransitionCoroutine(State state)
    {
        while (transitionLock)
        {
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        currentState.DoBeforeLeaving();

        currentState = state;
        // Reset the state to its desired condition before it can reason or act
        currentState.DoBeforeEntering();
    }

    public class State
    {
        public List<Transition> transitions = new List<Transition>();
        public StateID stateID;

        public State()
        {
            transitions.Add(new Transition("NullTransition", StateID.NullStateID));
        }

        public void AddTransition(string transitionName, StateID id)
        {
            // Check if anyone of the args is invalid
            if (transitionName == "NullTransition")
            {
                Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
                return;
            }

            if (id == StateID.NullStateID)
            {
                Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
                return;
            }

            foreach (var tran in transitions)
            {
                if (tran.TransionName == transitionName)
                {
                    Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has transition " + tran.ToString() +
                               "Impossible to assign to another state");
                    return;
                }
            }

            transitions.Add(new Transition(transitionName, id));
        }

        /// <summary>
        /// This method deletes a pair transition-state from this state's map.
        /// If the transition was not inside the state's map, an ERROR message is printed.
        /// </summary>
        public void DeleteTransition(string transitionName)
        {
            // Check for NullTransition
            if (transitionName == "NullTransition")
            {
                Debug.LogError("FSMState ERROR: NullTransition is not allowed");
                return;
            }
            foreach (var tran in transitions)
            {
                if (tran.TransionName == transitionName)
                {
                    transitions.Remove(tran);
                    return;
                }
            }
            Debug.LogError("FSMState ERROR: Transition " + transitionName.ToString() + " passed to " + stateID.ToString() +
                           " was not on the state's transition list");

        }

        /// <summary>
        /// This method returns the new state the FSM should be if
        ///    this state receives a transition and 
        /// </summary>
        public StateID GetOutputState(string transitionName)
        {
            foreach (var tran in transitions)
            {
                if (tran.TransionName == transitionName)
                {
                    return tran.NextStateID;
                }
            }
            return StateID.NullStateID;
        }

        /// <summary>
        /// This method is used to set up the State condition before entering it.
        /// It is called automatically by the FSMSystem class before assigning it
        /// to the current state.
        /// </summary>
        public virtual void DoBeforeEntering()
        {
            Debug.Log(this.ToString() + " Enter");
        }

        /// <summary>
        /// This method is used to make anything necessary, as reseting variables
        /// before the FSMSystem changes to another one. It is called automatically
        /// by the FSMSystem before changing to a new state.
        /// </summary>
        public virtual void DoBeforeLeaving()
        {
            Debug.Log(this.ToString() + " Leave");
        }

        /// <summary>
        /// This method decides if the state should transition to another on its list
        /// NPC is a reference to the object that is controlled by this class
        /// </summary>
        public virtual void Reason(GameObject player, GameObject npc) { }

        /// <summary>
        /// This method controls the behavior of the NPC in the game World.
        /// Every action, movement or communication the NPC does should be placed here
        /// NPC is a reference to the object that is controlled by this class
        /// </summary>
        public virtual void Act() { }

    } // class FSMState

} //class FSMSystem
