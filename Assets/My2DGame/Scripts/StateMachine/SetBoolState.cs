using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// State에서 애니메이션 bool형 파라미터 값을 셋팅하는 클래스
    /// StateMachineBehaviour을 상속받은 클래스는 애니메이터의 상태, 상태머신에 부착되어 실행된다
    /// </summary>
    public class SetBoolState : StateMachineBehaviour
    {
        #region Variables
        public string boolName;         //bool형 파라미터 이름

        public bool enterValue;         //상태 들어올때 bool형 값
        public bool exitValue;          //상태 나갈때 bool형 값
        #endregion

        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //상태 들어올때 1번 호출
            Debug.Log($"OnStateEnter : {boolName} : {enterValue}");
            animator.SetBool(boolName, enterValue);
        }

        // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    //상태에 있을때 매 프레임마다 호출
        //}

        // OnStateExit is called before OnStateExit is called on any state inside this state machine
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //상태 나갈때 1번 호출
            Debug.Log($"OnStateMachineExit : {boolName} : {exitValue}");
            animator.SetBool(boolName, exitValue);
        }

        // OnStateMove is called before OnStateMove is called on any state inside this state machine
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateIK is called before OnStateIK is called on any state inside this state machine
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMachineEnter is called when entering a state machine via its Entry Node
        //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        //{
        //    //상태 머신 들어올때 1번 호출            
        //}

        // OnStateMachineExit is called when exiting a state machine via its Exit Node
        //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        //{
        //    //상태 머신 나갈때 1번 호출            
        //}
    }
}