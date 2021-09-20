//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.Fsm {
	public interface IFsmState {
		void OnEnter(params object[] args);
		void OnUpdate();
		void OnExit(params object[] args);
	}

	public abstract class FsmStateBase<T> : IFsmState {
		public T Owner;
		
		public FsmStateBase(T owner) {
			Owner = owner;
		}
		public virtual void OnEnter(params object[] args) { }
		public virtual void OnUpdate() { }
		public virtual void OnExit(params object[] args) { }
	}
}