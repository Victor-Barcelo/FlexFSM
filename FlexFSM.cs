using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Víctor Barceló - 11/2013 - http://www.victorbarcelo.net
 * 
 * Finite State Machine based on the implementation found in Mat Buckland’s book "Programming Game AI by Example".
 * 
 *  Main differences with the FSM found in the Unity wiki (http://wiki.unity3d.com/index.php/Finit_State_Machine):
 *  - Doesn't make use of explicit transitions in the FSM declaration.
 *  - Includes blip states, which can be pushed and reverted.
 *  - Can assign a collection of triggers to states that will execute a predefined method (OnEvent) on the current 
 *      state. This is used to allow outside communication with the current state by addressing the NotifyEvent method of the FSM. 
 *  - Allows to define hierarchical FSM's without too much fuss.  
 * 
 *      Further examples and explanations can be found at http://www.victorbarcelo.net
 * 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE 
 * AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 **/

public class FlexFSM
{
    public GameObject Owner;
    private FlexState currentState;
    private FlexState previousToBlipState;
    private bool isActive;
    private Dictionary<Enum, FlexState> states;
    private FlexFSM rootFSM;

    public FlexFSM(GameObject _owner)
    {
        states = new Dictionary<Enum, FlexState>();
        isActive = false;
        currentState = null;
        previousToBlipState = null;
        Owner = _owner;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void SetRootFSM(FlexFSM _fsm)
    {
        rootFSM = _fsm;
    }

    public void NotifyEvent(Enum eventId, List<object> args)
    {
        if (!isActive)
        {
            Debug.LogError("Can't notify event: FSM is not active");
            return;
        }
        if (currentState.HasEvent(eventId))
        {
            currentState.OnEvent(eventId, args);
        }
    }

    private FlexState GetStateFromID(Enum id)
    {
        if (states[id] == null)
        {
            Debug.LogError("Can't get state from ID: state Not found");
            return null;
        }
        return states[id];
    }

    private Enum GetIDFromState(FlexState state)
    {
        if (!states.ContainsValue(state))
        {
            Debug.LogError("Can't get ID from state: StateID Not found");
            return null;
        }
        return states.FirstOrDefault(x => x.Value == state).Key;
    }

    public void AddState(Enum stateID, FlexState state)
    {
        if (states.ContainsValue(state) || states.ContainsKey(stateID))
        {
            Debug.LogError("Can't add state: state is already present");
            return;
        }
        state.SetFSM(this);
        state.Configure(Owner);
        states.Add(stateID, state);
    }

    public void Activate()
    {
        if (states.Count == 0)
        {
            Debug.LogError("Can't activate FSM: FSM has no states");
            return;
        }
        if (currentState == null)
        {
            Debug.LogError("Can't activate FSM: No starting state has been defined");
            return;
        }
        isActive = true;
    }

    public void DeActivate()
    {
        isActive = false;
    }

    internal void UpdateFSM()
    {
        if (isActive)
        {
            currentState.Reason(Owner);
            currentState.Act(Owner);
        }
        else
        {
            Debug.LogError("Can't update FSM: FSM is not active");
        }
    }

    public void ChangeState(Enum stateID)
    {
        if (currentState != null) currentState.OnExit(Owner);
        currentState = GetStateFromID(stateID);
        currentState.OnEnter(Owner);
    }

    private void ChangeState(FlexState state)
    {
        if (currentState != null) currentState.OnExit(Owner);
        currentState = state;
        currentState.OnEnter(Owner);
    }

    public void PushBlipState(Enum stateID)
    {
        if (IsCurrentState(stateID)) return;
        currentState.OnExit(Owner);
        previousToBlipState = currentState;
        ChangeState(stateID);
        currentState.OnEnter(Owner);
    }

    public void RevertBlipState()
    {
        if (previousToBlipState == null)
        {
            Debug.LogError("Attempting to revert a blip state that hasn't been defined");
            return;
        }
        currentState.OnExit(Owner);
        ChangeState(previousToBlipState);
        previousToBlipState = null;
        currentState.OnEnter(Owner);
    }

    public bool IsCurrentState(Enum stateID)
    {
        if (!isActive)
        {
            Debug.LogError("Can't check current state: FSM is not active");
            return false;
        }
        return currentState == GetStateFromID(stateID);
    }

    public string GetCurrentStateName()
    {
        if (!isActive)
        {
            Debug.LogError("Can't get current state name: FSM is not active");
            return "";
        }
        return GetIDFromState(currentState).ToString();
    }
}