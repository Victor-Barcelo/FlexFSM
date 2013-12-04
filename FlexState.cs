using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

public abstract class FlexState
{
    public FlexFSM fsm;
    private List<Enum> events;

    protected FlexState()
    {
        events = new List<Enum>();
    }

    public void AddEvent(Enum eventId)
    {
        if (events.Contains(eventId))
        {
            Debug.LogError("Can't add event: event is already present");
            return;
        }
        events.Add(eventId);
    }

    public void GetEvents()
    {
        foreach (Enum @event in events)
        {
            Debug.Log(@event);
        }
    }

    public bool HasEvent(Enum eventId)
    {
        return events.Any(item => item.Equals(eventId));
    }

    public void SetFSM(FlexFSM _fsm)
    {
        fsm = _fsm;
    }

    public void ChangeState(Enum stateID)
    {
        fsm.ChangeState(stateID);
    }

    public GameObject GetOwner()
    {
        return fsm.Owner;
    }

    public virtual void Configure(GameObject owner) { }

    public virtual void OnEvent(Enum eventId, List<object> args){}

    public virtual void OnEnter(GameObject owner) { }

    public virtual void Reason(GameObject owner) { }

    public virtual void Act(GameObject owner) { }

    public virtual void OnExit(GameObject owner) { }
}
