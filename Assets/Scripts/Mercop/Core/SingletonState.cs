using UnityEngine;

namespace Mercop.Core
{
    public class SingletonState<T> : State where T:State
    {
        private static T instance;
        
        public override void OnStateEnter()
        {
        }

        public override void OnStateExit()
        {
        }
        
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (T) FindObjectOfType(typeof(T));
                    if (instance == null)
                    {
                        instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                    }
                }
                return instance;
            }
        }
        
        protected virtual void Awake()
        {
            instance = this as T;
        }
        
    }
}