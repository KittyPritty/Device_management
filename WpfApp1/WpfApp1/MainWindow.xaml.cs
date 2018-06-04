using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Логіга взаємодії для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        HttpWebRequest devicesRequest; // обєкт запиту списку девайсів

        public MainWindow()
        {
            InitializeComponent();
            InitTimer();
        }

        // Ініціалізація таймеру для запитів кожної секунди
        private void InitTimer()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
        }

        // Ця функція виконується кожну секунду
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            MakeDevicesRequest(); // Запит списку девайсів

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }

        // Запит на сервер для списку девайсів
        private void MakeDevicesRequest()
        {
            devicesRequest = WebRequest.CreateHttp("https://infinite-bastion-64144.herokuapp.com/devices"); // Ініціалізуємо клас запиту
            devicesRequest.Method = "GET"; // Вказуємо ГЕТ метод
            devicesRequest.BeginGetResponse(OnGetDevices, devicesRequest); // робимо запит, вказуємо колбек
        }

        
        private void OnGetDevices(IAsyncResult asynchronousResult)
        {
            try
            {
                // Початок зчитування результату з запиту
                WebResponse resp = devicesRequest.EndGetResponse(asynchronousResult);
                HttpWebResponse response = (HttpWebResponse)resp;
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                string responseString = streamRead.ReadToEnd();
                streamResponse.Close();
                streamRead.Close();
                response.Close();
                // Кінець зчитування результату з запиту
                
                List<Device> devices = JsonConvert.DeserializeObject<List<Device>>(responseString); // Перетворюємо відповідь сервера у список девайсів
                devices.ForEach(delegate (Device d)
                {
                    updateDevice(d); // Для кожного девайсу зберігаємо дані в локальний файл
                });
                this.Dispatcher.Invoke(() => // Оновлюємо дані в основному потоці
                {
                    tbDevices.Text = "id | Name | Real °C | Target °C\n"; // ініціалізуємо шапку
                    devices.ForEach(delegate (Device d)
                    {
                        tbDevices.Text += d.deviceId.ToString() + " | " + d.name + " | " + d.realTemperature.ToString() + " | " + d.targetTemperature.ToString() + "\n";
                        // виводимо на екран кожен девайс
                    });
                });
                
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }

        // Функція, яка зберігає температуру кожного девайсу в файл
        private void updateDevice(Device d)
        {
            string mydocpath =
                 Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Devices"; // шлях до папки Документи/Девайси
            System.IO.Directory.CreateDirectory(mydocpath); // Перевіряємо чи існує директорія і створюємо

            // Виконуємо запис в файл
            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(mydocpath, d.deviceId.ToString() + "_device.txt")))
            {
                outputFile.Write(d.targetTemperature);
            }
            
            // Відсилаємо записану темературу на сервер
            UpdateRealTemperature(d);
        }

        private void UpdateRealTemperature(Device d)
        {
            // Виконуємо запит на перезапис дійсної температури пристрою
            WebRequest temperatureRequest = WebRequest.CreateHttp(String.Format("https://infinite-bastion-64144.herokuapp.com/updateRealTemperature?username=yurii&password=kot&deviceId={0}&temp={1}", d.deviceId, d.targetTemperature));
            temperatureRequest.Method = "GET";
            temperatureRequest.BeginGetResponse(OnTemperatureAnswer, temperatureRequest);
        }

        // Функція для результату запиту зміни дійсної температури пристрою
        private void OnTemperatureAnswer(IAsyncResult asynchronousResult)
        {
        }

        // Клас даних девайсу, який повертає сервер
        public class Device
        {
            public string name { get; set; }
            public int deviceId { get; set; }
            public int targetTemperature { get; set; }
            public int realTemperature { get; set; }
        }
    }

}
