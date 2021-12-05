using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatClient.ServiceChat; //підключаємо простір імен сервісу ServiceChat

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ServiceChat.IServiceChatCallback //реалізовуємо інтерфейс IServiceChatCallback через сервіс ServiceChat
    {                                                                          //для того, щоб в класі реалізувати метод MsgCallBack,
        bool IsConnected = false;                                              //який буде приймати повідомлення на стороні сервера
        ServiceChat.ServiceChatClient client;//створюємо обєкт нашого хоста(сервісу) в нашому клієнті, для того, щоб можливо було взаємодіяти з його методами,
        //так як ми підключили наш сервіс в "Ссилки" нашого клієнта, то ми вже можемо з ними працювати
        int ID; //ID для клієнта
        public MainWindow()
        {
            InitializeComponent();
        }

        private void load_Loaded(object sender, RoutedEventArgs e) //подія загрузка вікна інтерфейсу
        {
            
        }

        void ConnectUser()//метод підключення
        {
            if (!IsConnected)//якщо не підєднанні
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));//відбувається ініціалізація обєкта під час завантаження вікна
                                                                                              //в параметри ми передаємо обєкт типу InstanceContext, щоб ми могли там визивать CallBack
                                                                                              //this для того так як ми в цьому класі реалізуємо інтерфейс IServiceChatCallback
                                                                                              //тепер ми можемо реалізувати всі методи, якф реалізовані в нашому сервісі
                ID = client.Connect(tbUserName.Text); //визиваємо метод Connect (присвоює ID клієнта в момент підключення), який реалізований в сервісі і передаємо клієнту ім’я
                tbUserName.IsEnabled = false; //відключаємо можливість змінювати назву User шляхом блокування textBox
                ConDiscon.Content = "Disconnect"; //встановлюємо назву кнопки
                IsConnected = true;
            }
        }

        void DisconnectUser()//метод відключення
        {
            if (IsConnected)//якщо підєднанні
            {
                client.Disconnect(ID); //визиваємо метод Disconnect з сервісу, щоб сервер знав, з яким ID клієнта виключати зі списку
                client = null; //якщо відєднуємось, то клієнт стає null
                tbUserName.IsEnabled = true; //включаємо можливість змінювати назву User шляхом розблокування textBox
                ConDiscon.Content = "Connect";//встановлюємо назву кнопки
                IsConnected = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
        }

        public void MsgCallBack(string msg)
        {
            lbChat.Items.Add(msg); //додаємо в список нашого listBox нове повідомлення
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count - 1]); //включаємо прокрутку listBox до тих пір поки не дійдемо до останнього елемента
        }

        private void load_Closing(object sender, System.ComponentModel.CancelEventArgs e) //подія на закриття вікна
        {
            DisconnectUser();// відбувається відєднання клієнта, коли вікно клієнта буде закрите
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)//подія натиснення клавіш в TextBox
        {
            if (e.Key == Key.Enter)//якщо натиснена клавіша enter, то це значить, що ми ввели нове повідомлення і нам потрібно його надіслати
            {
                if (client!=null)//якщо клієнт не null, то значить ми вже підєднались
                {
                    client.SendMsg(tbMessage.Text, ID);//з сервіса визиваємо метод SendMsg, в параметри передаємо текст повдомлення з TextBox та ID клієнта
                    tbMessage.Text = string.Empty; //якщо повідомлення відіслано, то ми в TextBox поміщаємо порожній рядок
                }
            }
        }
    }
}
