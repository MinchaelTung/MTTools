using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTFramework.Iso8583PackageUtil
{

    public abstract class AbstractMessageProtocol : BaseMessageProtocol
    {

        private SortedList<int, MessageFieldDefinition> messageFieldDefinitions = new SortedList<int, MessageFieldDefinition>();

        /// <summary>
        /// 添加或获取域定义
        /// </summary>
        /// <param name="definition"></param>
        public SortedList<int, MessageFieldDefinition> MessageFieldDefinitions
        {
            get { return messageFieldDefinitions; }
            set { messageFieldDefinitions = value; }
        }



        public MessageFieldDefinition GetFieldDefinition(int field)
        {
            return messageFieldDefinitions[field];
        }

        /// <summary>
        /// 添加域定义
        /// </summary>
        /// <param name="definition"></param>
        protected void AddFieldDefinition(MessageFieldDefinition definition)
        {
            messageFieldDefinitions.Add(definition.Index, definition);
        }


        public abstract int HeaderLength { get; }

        public abstract int MessageTypeIdLength { get; }

        public abstract int BitmapLength { get; }

        public abstract int MaxFieldCount { get; }

        public abstract bool IsBinary { get; }

        public abstract string Name { get; }
    }
}
