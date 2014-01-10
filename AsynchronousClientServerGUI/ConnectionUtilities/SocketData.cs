using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectionUtilities
{
    [Serializable]
    public class SocketData
    {
        public enum DataType { RequestPing, RespondToPing, Chat, Boot, UserInformation, Null };

        public DataType dataType = DataType.Null;
        public object data = null;

        public SocketData(DataType dataType, object data)
        {
            this.dataType = dataType;
            this.data = data;
        }
    }
}
