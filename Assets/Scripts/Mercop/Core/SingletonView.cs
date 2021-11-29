using UnityEngine;

namespace Mercop.Core
{
    public class SingletonView<T> : View where T:View
    {
        private static T instance;
        
        public override void OnShow()
        {
        }

        public override void OnHide()
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