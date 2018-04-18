using System;

namespace MTFramework.SimpleDataProxy.ORM
{
    /// <summary>
    /// 存储过程事件
    /// </summary>
    public class ProcedureEventArgs : EventArgs
    {
        /// <summary>
        /// 存储过程事件
        /// </summary>
        /// <param name="procedure">存储过程</param>
        public ProcedureEventArgs(Procedure procedure)
        {
            this.procedure = procedure;
        }

        private Procedure procedure;
        /// <summary>
        /// 存储过程
        /// </summary>
        public Procedure Procedure
        {
            get
            {
                return this.procedure;
            }
            set
            {
                this.procedure = value;
            }
        }
    }
}
