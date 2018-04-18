using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTFramework.Iso8583PackageUtil
{
    public class MessagePackageProtocol : AbstractMessageProtocol
    {
        private static MessagePackageProtocol instance = new MessagePackageProtocol();

        public static MessagePackageProtocol GetInstance()
        {
            return instance;
        }

        private MessagePackageProtocol()
        {
        }

        public override int HeaderLength
        {
            get { return 11; }
        }

        public override int MessageTypeIdLength
        {
            get { return 4; }
        }

        public override int BitmapLength
        {
            get { return 8; }
        }

        public override int MaxFieldCount
        {
            get { return 64; }
        }

        public override bool IsBinary
        {
            get { return true; }
        }

        public override string Name
        {
            get { return "package"; }
        }
    }
}
