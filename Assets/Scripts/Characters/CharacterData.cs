using System;
using UnityEngine;

namespace CGames
{
    public class CharacterData
    {
        public bool IsEnemy { get; private set; }
        public Vector2 InputVector { get; private set; }
        public bool IsAttacking { get; private set; }
        public bool IsAirborne { get; private set; }

        public event Action OnAirBorneEnter;
        public event Action OnAirBorneExit;

        public event Action OnAttackEnter;
        public event Action OnAttackExit;

        public bool HasHorizontalInput => Mathf.Abs(InputVector.x) > Mathf.Epsilon;

        public CharacterData(bool isEnemy)
        {
            IsEnemy = isEnemy;
            InputVector = new(0, 0);
            IsAttacking = false;
        }

        public void SetNewInput(Vector2 newInput) => InputVector = newInput;
        public void ResetInput() => InputVector = new(0, 0);

        public void EnterAttack()
        {
            if(IsAttacking)
                return;

            IsAttacking = true;

            OnAttackEnter?.Invoke();
        }

        public void ExitAttack()
        {
            if(IsAttacking == false)
                return;

            IsAttacking = false;

            OnAttackExit?.Invoke();
        }        

        public void EnterAirborne()
        {
            if(IsAirborne)
                return;

            IsAirborne = true;

            OnAirBorneEnter?.Invoke();
        }

        public void ExitAirborne()
        {
            if(IsAirborne == false)
                return;

            IsAirborne = false;

            OnAirBorneExit?.Invoke();
        }
    }
}