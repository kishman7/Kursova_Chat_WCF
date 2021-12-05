using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Chat_WCF
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)] // атрибут з параметром, який дозволяє клієнтам, розуміти що сервіс спільний для всіх

    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    
    public class ServiceChat : IServiceChat
    {
        static List<ServerUser> users = new List<ServerUser>(); //створюємо список обєктів ServerUser
        static int nextId = 1; //змінна, яка буде викорстовуватись для генерації ID

        public int Connect(string name) //реалізація методу для підключення user до  сервісу
        {
            ServerUser user = new ServerUser() //створюємо нового user
            {
                ID = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextId++; //при створенні нового user, його ID збільшуємо на 1
            
            users.Add(user); // добавляємо нового user в колекцію List

            SendMsg(": " + user.Name + " підключився до чату!", 0); //визиваємо метод для того, щоб інші user бачили повідомлення про підключення нового user
                                                           //ставимо параметер "0", щоб не посилати повідомлення самому собі, так як user з ID "0" в списку немає
            return user.ID; // повертає ID юзера
        }

        public void Disconnect(int id) //реалізація методу для відключення user від сервісу
        {
            var user = users.FirstOrDefault(i => i.ID == id); //за допомогою Linq шукаємо user з потрібним ID
            if (user != null) //якщо user не null
            {
                users.Remove(user); // то видаляємо цього user
                SendMsg(": " + user.Name + " залишив чат!", 0);
            }
        }

        public void SendMsg(string msg, int id) // метод для відправки повідомлення
        {
            foreach (var item in users) //перебираємо всіх users
            {
                string answer = DateTime.Now.ToShortTimeString(); // формуємо повідомлення, яке буде відповіддю від сервера для всіх наших user
                
                //додаємо в повідомлення ім’я user, який послав це повідомлення
                var user = users.FirstOrDefault(i => i.ID == id); //за допомогою Linq шукаємо user з потрібним ID
                if (user != null) //якщо user не null
                {
                    answer += ": " + user.Name + " "; //до повідомлення додаємо ім’я user
                }
                answer += msg; //до повідомлення додаємо повідомлення, яке зайшло вхідним параметром

                //після формування повідомлення, нам потрібно відправити це повідомлення для user, з яким працюємо в циклі foreach
                item.operationContext.GetCallbackChannel<IServerChatCallBack>().MsgCallBack(answer);
            }
        }
    }
}
