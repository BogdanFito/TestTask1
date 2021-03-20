using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ConsoleApp1
{
    class Monitor
    {
        private static List<Process> processes = null; //Все текущие процессы
        
        //Получаем все текущие процессы в список
        private void GetProcesses()
        {
            if (processes!=null) processes.Clear();
            processes = Process.GetProcesses().ToList<Process>();
        }

        //Убиваем процесс, если он жив больше, чем нужно
        public void KillProcess(String ProcessName, int LifeTime)
        {
            GetProcesses();
            Process[] process = processes.FindAll(item => item.ProcessName==ProcessName).ToArray(); //Ищем все текущие процессы с данным именем
            for (int i=0;i<process.Length;i++) { 
                if ((DateTime.Now - process[i].StartTime).TotalMilliseconds > LifeTime * 60000) {
                    process[i].Kill();
                    Console.WriteLine("Процесс " + ProcessName + " был убит в " + DateTime.Now); //Что-то типа логов
                }
            }
        }

       
        }
    

    class MainClass
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Введите название процесса, время его жизни и частоту проверки.");
                string[] parameters = Console.ReadLine().Split(' '); //Вводим нужные значения
                string ProcessName = "";
                int LifeTime = 0, Freq = 0;

                //Далее эти значения присваиваются переменным
                try
                {
                    ProcessName = parameters[0];
                    LifeTime = Convert.ToInt32(parameters[1]);
                    Freq = Convert.ToInt32(parameters[2]);
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("Введены некорректные данные!");
                    continue;

                }

                //Создаем объект класса нашего монитора
                Monitor monitor = new Monitor();

                //Запуск постоянного мониторинга процессов
                while (true)
                {
                    monitor.KillProcess(ProcessName, LifeTime);
                    Thread.Sleep(Freq * 60000); //выдерживаем частоту
                }
            }
        }
    }
}
