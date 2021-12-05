using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Chat_WCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract(CallbackContract = typeof(IServerChatCallBack))] // атрибути для взаємодії роботи інтерфейса з сервісом
                                                                      //CallbackContract - параметр, який дає розуміння сервісу, що він може визивати інтерфейс для роботи з сервером
    public interface IServiceChat //в цьому інтерфейсі будуть описуватись функції, якими буде користуватись наш клієнт, який буде працювати з нашим сервером
    {
        [OperationContract] // даний атрибут має бути для кожного метода в інтерфейсі, тоді цей метод буде видно для клієнта

        int Connect(string user); // за допомогою цього методу ми будемо підключатись до нашого сервісу, приймає ім’я user, який підключається

        [OperationContract]

        void Disconnect(int id); // метод буде визиватись, коли клієнт буде покидати чат, або натискати кнопку "Завершити чат"

        [OperationContract(IsOneWay = true)] // IsOneWay даний параметр дозволяє не очікувати відповіді від сервера, що він щось отримав

        void SendMsg(string msg, int id); //метод, який буде приймати строкове повідомлення, та id юзера, щоб було видно від кого приходить повідомлення

    }

    public interface IServerChatCallBack //інтерфейс, який буде описувати функції на сервері
    {
        [OperationContract(IsOneWay = true)]

        void MsgCallBack(string msg); //метод, який буде приймати повідомлення на стороні сервера
    }
}
