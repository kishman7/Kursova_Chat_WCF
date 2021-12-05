using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatHost
{
    class Program
    {
        static void Main(string[] args)
        {
            // описуємо реалізацію хоста
            //викорсит. using, щоб міг реалізуватись interface Dispos, коли нам буде потрібно, то щоб в нас всі ресурси звільнялись
            using (var host = new ServiceHost(typeof(Chat_WCF.ServiceChat)))
            {
                host.Open(); //відкриваємо наш host
                Console.WriteLine("Хост стартував!");
                Console.ReadLine();
            }
        }
    }
}
