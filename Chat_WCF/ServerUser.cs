using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;


namespace Chat_WCF
{
    class ServerUser
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public OperationContext operationContext { get; set; } // в цій властивості буде зберігатись інформація про підключення клієнта до нашого сервісу,
                                                                // щоб ми могли, коли user вже підключався, звертатись до нього зі сторони нашого сервісу
    }
}
