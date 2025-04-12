using System;

namespace Framework.CharacterStateMachine
{
    public abstract class CharacterStateBase<TType>
        where TType : Enum
    {
        public abstract TType Type { get; }

        public virtual bool CanEnter(TType prevState) => true;
        public virtual bool CanExit(TType nextState) => true;

        public virtual void OnEnter(TType prevState) { }
        public virtual void OnExit(TType nextState) { }
        
        public virtual void Update() { }
    }
}