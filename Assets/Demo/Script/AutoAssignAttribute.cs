//author:kuribayashi   2016年8月31日05:08:53
using System;
namespace KLFrame{
    /// <summary>
    /// 设置可以添加到成员变量
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    //特性类,为成员变量添加特性,使其获得自动赋值能力
    public class AutoAssignAttribute : Attribute
    {
        public bool autoAdd { get; set; } 
        public AutoAssignAttribute() { }
        public AutoAssignAttribute(bool AUTOADD) {
            autoAdd = AUTOADD;
        }
    }
}