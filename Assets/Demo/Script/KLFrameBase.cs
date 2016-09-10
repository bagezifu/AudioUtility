//author:kuribayashi    2016年8月31日05:09:17
using UnityEngine;
using System.Reflection;
using System;

namespace KLFrame
{
    public class KLFrameBase<T> : MonoBehaviour where T : class
    {
        private static T instance;
        private static bool isSingleton = true;
        private static string mine;
        private SingletonType singletonState = SingletonType.Other;
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static T GetInstance()
        {
            if (CheckSingleton()==SingletonType.Undefine) {
                Debug.LogWarning(mine+"类未开启单例模式!");
                return null;
            }
            if (isSingleton) return instance;
            else
            {
                Debug.LogErrorFormat("{0}类不止一个实例,无法返回单例!", mine);
                return null;
            }
        }
        /// <summary>
        /// 在构造时初始化单例与派生类名字
        /// </summary>
        public KLFrameBase()
        {
            instance = this as T;
            mine = instance.GetType().Name;
        }
        /// <summary>
        /// 检测场景中是否只存在一个
        /// </summary>
        void Main()
        {
            singletonState = CheckSingleton();
            if (singletonState!=SingletonType.Complete) {
                return;
            }
            UnityEngine.Object[] objs = GameObject.FindObjectsOfType(this.GetType());
            if (objs.Length > 1)
            {
                instance = null;
                isSingleton = false;
            }
        }



        /// <summary>
        /// 当脚本被添加到Gameobject上初始化时自动赋值
        /// </summary>
        public void Reset()
        {
            AutoAssgin();
        }

        private static SingletonType CheckSingleton()
        {
            if (instance == null) {
               // Debug.LogErrorFormat("{0}类不止一个实例,无法返回单例!", mine);
                return SingletonType.Multiple;
            } 
            object[] objAttrs = instance.GetType().GetCustomAttributes(false);
            if (objAttrs.Length > 0)
            {
                foreach (object obj in objAttrs)
                {
                    if (obj.GetType() == typeof(SingletonAttribute))
                    {
                        return SingletonType.Complete;
                    }
                }
            }
            return SingletonType.Undefine;
        }


        /// <summary>
        /// 初始化时通过反射寻找Attribute并且自动赋值
        /// </summary>
        void AutoAssgin()
        {
            //  Debug.LogWarning("init");
            FieldInfo[] fields = this.GetType().GetFields();
            foreach (FieldInfo f in fields)
            {
                object[] objAttrs = f.GetCustomAttributes(typeof(AutoAssignAttribute), true);
                if (objAttrs.Length > 0)
                {
                    foreach (object obj in objAttrs)
                    {
                        if (obj.GetType() == typeof(AutoAssignAttribute))
                        {
                            AutoAssignAttribute attr = obj as AutoAssignAttribute;
                            UnityEngine.Object uobj = GetComponent(f.FieldType);
                            if (uobj)
                                f.SetValue(this, uobj);
                            else
                            {
                                if (attr.autoAdd)
                                {
                                    f.SetValue(this, this.gameObject.AddComponent(f.FieldType));
                                    Debug.LogWarningFormat("{0}类成员自动获取{1}组件失败,自动添加属性为True.将自动添加组价到Gameobject!", this.GetType().Name, f.FieldType.Name);
                                }
                                else
                                {
                                    Debug.LogWarningFormat("{0}类成员自动获取{1}组件失败,自动添加属性为Flase.请确定当前Gameobject是否拥有此组件!", this.GetType().Name, f.FieldType.Name);
                                }
                            }
                        }
                    }


                }
            }
        }

    }
    public enum SingletonType {
        Complete,
        Undefine,
        Multiple,
        Other
    }

}