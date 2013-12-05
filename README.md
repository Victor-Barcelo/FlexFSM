FlexFSM - A Unity3D Finite State Machine
========================================

Víctor Barceló - 11/2013

Finite State Machine based on the implementation found in Mat Buckland’s book "Programming Game AI by Example".

Main differences with the FSM found in the Unity wiki ( http://wiki.unity3d.com/index.php/Finit_State_Machine ):
- Doesn't make use of explicit transitions in the FSM declaration.
- Includes blip states, which can be pushed and reverted.
- Can assign a collection of triggers to states that will execute a predefined method (OnEvent) on the current 
state. This is used to allow outside communication with the current state by addressing the NotifyEvent method of the FSM.
- Allows to define hierarchical FSM's without too much fuss.  

Further examples and explanations can be found at http://www.victorbarcelo.net

 
  
  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE 
  AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
  
 
