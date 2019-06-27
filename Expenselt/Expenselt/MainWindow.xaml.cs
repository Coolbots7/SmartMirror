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
using System.Threading;
using LiveCharts;
using LiveCharts.Wpf;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Configuration;
using Newtonsoft.Json.Linq;
using OpenWeatherMap;
using System.Globalization;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;

namespace Expenselt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        private bool Maximized = false;

        #region Weather
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public string CityID;
        public string OpenWeatherMapAppID;

        private static string weather_sunny = "M12,7A5,5 0 0,1 17,12A5,5 0 0,1 12,17A5,5 0 0,1 7,12A5,5 0 0,1 12,7M12,9A3,3 0 0,0 9,12A3,3 0 0,0 12,15A3,3 0 0,0 15,12A3,3 0 0,0 12,9M12,2L14.39,5.42C13.65,5.15 12.84,5 12,5C11.16,5 10.35,5.15 9.61,5.42L12,2M3.34,7L7.5,6.65C6.9,7.16 6.36,7.78 5.94,8.5C5.5,9.24 5.25,10 5.11,10.79L3.34,7M3.36,17L5.12,13.23C5.26,14 5.53,14.78 5.95,15.5C6.37,16.24 6.91,16.86 7.5,17.37L3.36,17M20.65,7L18.88,10.79C18.74,10 18.47,9.23 18.05,8.5C17.63,7.78 17.1,7.15 16.5,6.64L20.65,7M20.64,17L16.5,17.36C17.09,16.85 17.62,16.22 18.04,15.5C18.46,14.77 18.73,14 18.87,13.21L20.64,17M12,22L9.59,18.56C10.33,18.83 11.14,19 12,19C12.82,19 13.63,18.83 14.37,18.56L12,22Z";
        private static string weather_cloudy = "M6,19A5,5 0 0,1 1,14A5,5 0 0,1 6,9C7,6.65 9.3,5 12,5C15.43,5 18.24,7.66 18.5,11.03L19,11A4,4 0 0,1 23,15A4,4 0 0,1 19,19H6M19,13H17V12A5,5 0 0,0 12,7C9.5,7 7.45,8.82 7.06,11.19C6.73,11.07 6.37,11 6,11A3,3 0 0,0 3,14A3,3 0 0,0 6,17H19A2,2 0 0,0 21,15A2,2 0 0,0 19,13Z";
        private static string weather_partly_cloudy = "M12.74,5.47C15.1,6.5 16.35,9.03 15.92,11.46C17.19,12.56 18,14.19 18,16V16.17C18.31,16.06 18.65,16 19,16A3,3 0 0,1 22,19A3,3 0 0,1 19,22H6A4,4 0 0,1 2,18A4,4 0 0,1 6,14H6.27C5,12.45 4.6,10.24 5.5,8.26C6.72,5.5 9.97,4.24 12.74,5.47M11.93,7.3C10.16,6.5 8.09,7.31 7.31,9.07C6.85,10.09 6.93,11.22 7.41,12.13C8.5,10.83 10.16,10 12,10C12.7,10 13.38,10.12 14,10.34C13.94,9.06 13.18,7.86 11.93,7.3M13.55,3.64C13,3.4 12.45,3.23 11.88,3.12L14.37,1.82L15.27,4.71C14.76,4.29 14.19,3.93 13.55,3.64M6.09,4.44C5.6,4.79 5.17,5.19 4.8,5.63L4.91,2.82L7.87,3.5C7.25,3.71 6.65,4.03 6.09,4.44M18,9.71C17.91,9.12 17.78,8.55 17.59,8L19.97,9.5L17.92,11.73C18.03,11.08 18.05,10.4 18,9.71M3.04,11.3C3.11,11.9 3.24,12.47 3.43,13L1.06,11.5L3.1,9.28C3,9.93 2.97,10.61 3.04,11.3M19,18H16V16A4,4 0 0,0 12,12A4,4 0 0,0 8,16H6A2,2 0 0,0 4,18A2,2 0 0,0 6,20H19A1,1 0 0,0 20,19A1,1 0 0,0 19,18Z";
        private static string weather_fog = "M3,15H13A1,1 0 0,1 14,16A1,1 0 0,1 13,17H3A1,1 0 0,1 2,16A1,1 0 0,1 3,15M16,15H21A1,1 0 0,1 22,16A1,1 0 0,1 21,17H16A1,1 0 0,1 15,16A1,1 0 0,1 16,15M1,12A5,5 0 0,1 6,7C7,4.65 9.3,3 12,3C15.43,3 18.24,5.66 18.5,9.03L19,9C21.19,9 22.97,10.76 23,13H21A2,2 0 0,0 19,11H17V10A5,5 0 0,0 12,5C9.5,5 7.45,6.82 7.06,9.19C6.73,9.07 6.37,9 6,9A3,3 0 0,0 3,12C3,12.35 3.06,12.69 3.17,13H1.1L1,12M3,19H5A1,1 0 0,1 6,20A1,1 0 0,1 5,21H3A1,1 0 0,1 2,20A1,1 0 0,1 3,19M8,19H21A1,1 0 0,1 22,20A1,1 0 0,1 21,21H8A1,1 0 0,1 7,20A1,1 0 0,1 8,19Z";
        private static string weather_snowy = "M6,14A1,1 0 0,1 7,15A1,1 0 0,1 6,16A5,5 0 0,1 1,11A5,5 0 0,1 6,6C7,3.65 9.3,2 12,2C15.43,2 18.24,4.66 18.5,8.03L19,8A4,4 0 0,1 23,12A4,4 0 0,1 19,16H18A1,1 0 0,1 17,15A1,1 0 0,1 18,14H19A2,2 0 0,0 21,12A2,2 0 0,0 19,10H17V9A5,5 0 0,0 12,4C9.5,4 7.45,5.82 7.06,8.19C6.73,8.07 6.37,8 6,8A3,3 0 0,0 3,11A3,3 0 0,0 6,14M7.88,18.07L10.07,17.5L8.46,15.88C8.07,15.5 8.07,14.86 8.46,14.46C8.85,14.07 9.5,14.07 9.88,14.46L11.5,16.07L12.07,13.88C12.21,13.34 12.76,13.03 13.29,13.17C13.83,13.31 14.14,13.86 14,14.4L13.41,16.59L15.6,16C16.14,15.86 16.69,16.17 16.83,16.71C16.97,17.24 16.66,17.79 16.12,17.93L13.93,18.5L15.54,20.12C15.93,20.5 15.93,21.15 15.54,21.54C15.15,21.93 14.5,21.93 14.12,21.54L12.5,19.93L11.93,22.12C11.79,22.66 11.24,22.97 10.71,22.83C10.17,22.69 9.86,22.14 10,21.6L10.59,19.41L8.4,20C7.86,20.14 7.31,19.83 7.17,19.29C7.03,18.76 7.34,18.21 7.88,18.07Z";
        private static string weather_snowy_rainy = "M6,14A1,1 0 0,1 7,15A1,1 0 0,1 6,16A5,5 0 0,1 1,11A5,5 0 0,1 6,6C7,3.65 9.3,2 12,2C15.43,2 18.24,4.66 18.5,8.03L19,8A4,4 0 0,1 23,12A4,4 0 0,1 19,16H18A1,1 0 0,1 17,15A1,1 0 0,1 18,14H19A2,2 0 0,0 21,12A2,2 0 0,0 19,10H17V9A5,5 0 0,0 12,4C9.5,4 7.45,5.82 7.06,8.19C6.73,8.07 6.37,8 6,8A3,3 0 0,0 3,11A3,3 0 0,0 6,14M7.88,18.07L10.07,17.5L8.46,15.88C8.07,15.5 8.07,14.86 8.46,14.46C8.85,14.07 9.5,14.07 9.88,14.46L11.5,16.07L12.07,13.88C12.21,13.34 12.76,13.03 13.29,13.17C13.83,13.31 14.14,13.86 14,14.4L13.41,16.59L15.6,16C16.14,15.86 16.69,16.17 16.83,16.71C16.97,17.24 16.66,17.79 16.12,17.93L13.93,18.5L15.54,20.12C15.93,20.5 15.93,21.15 15.54,21.54C15.15,21.93 14.5,21.93 14.12,21.54L12.5,19.93L11.93,22.12C11.79,22.66 11.24,22.97 10.71,22.83C10.17,22.69 9.86,22.14 10,21.6L10.59,19.41L8.4,20C7.86,20.14 7.31,19.83 7.17,19.29C7.03,18.76 7.34,18.21 7.88,18.07Z";
        private static string weather_hail = "M6,14A1,1 0 0,1 7,15A1,1 0 0,1 6,16A5,5 0 0,1 1,11A5,5 0 0,1 6,6C7,3.65 9.3,2 12,2C15.43,2 18.24,4.66 18.5,8.03L19,8A4,4 0 0,1 23,12A4,4 0 0,1 19,16H18A1,1 0 0,1 17,15A1,1 0 0,1 18,14H19A2,2 0 0,0 21,12A2,2 0 0,0 19,10H17V9A5,5 0 0,0 12,4C9.5,4 7.45,5.82 7.06,8.19C6.73,8.07 6.37,8 6,8A3,3 0 0,0 3,11A3,3 0 0,0 6,14M10,18A2,2 0 0,1 12,20A2,2 0 0,1 10,22A2,2 0 0,1 8,20A2,2 0 0,1 10,18M14.5,16A1.5,1.5 0 0,1 16,17.5A1.5,1.5 0 0,1 14.5,19A1.5,1.5 0 0,1 13,17.5A1.5,1.5 0 0,1 14.5,16M10.5,12A1.5,1.5 0 0,1 12,13.5A1.5,1.5 0 0,1 10.5,15A1.5,1.5 0 0,1 9,13.5A1.5,1.5 0 0,1 10.5,12Z";
        private static string weather_rainy = "M6,14A1,1 0 0,1 7,15A1,1 0 0,1 6,16A5,5 0 0,1 1,11A5,5 0 0,1 6,6C7,3.65 9.3,2 12,2C15.43,2 18.24,4.66 18.5,8.03L19,8A4,4 0 0,1 23,12A4,4 0 0,1 19,16H18A1,1 0 0,1 17,15A1,1 0 0,1 18,14H19A2,2 0 0,0 21,12A2,2 0 0,0 19,10H17V9A5,5 0 0,0 12,4C9.5,4 7.45,5.82 7.06,8.19C6.73,8.07 6.37,8 6,8A3,3 0 0,0 3,11A3,3 0 0,0 6,14M14.83,15.67C16.39,17.23 16.39,19.5 14.83,21.08C14.05,21.86 13,22 12,22C11,22 9.95,21.86 9.17,21.08C7.61,19.5 7.61,17.23 9.17,15.67L12,11L14.83,15.67M13.41,16.69L12,14.25L10.59,16.69C9.8,17.5 9.8,18.7 10.59,19.5C11,19.93 11.5,20 12,20C12.5,20 13,19.93 13.41,19.5C14.2,18.7 14.2,17.5 13.41,16.69Z";
        private static string weather_pouring = "M9,12C9.53,12.14 9.85,12.69 9.71,13.22L8.41,18.05C8.27,18.59 7.72,18.9 7.19,18.76C6.65,18.62 6.34,18.07 6.5,17.54L7.78,12.71C7.92,12.17 8.47,11.86 9,12M13,12C13.53,12.14 13.85,12.69 13.71,13.22L11.64,20.95C11.5,21.5 10.95,21.8 10.41,21.66C9.88,21.5 9.56,20.97 9.7,20.43L11.78,12.71C11.92,12.17 12.47,11.86 13,12M17,12C17.53,12.14 17.85,12.69 17.71,13.22L16.41,18.05C16.27,18.59 15.72,18.9 15.19,18.76C14.65,18.62 14.34,18.07 14.5,17.54L15.78,12.71C15.92,12.17 16.47,11.86 17,12M17,10V9A5,5 0 0,0 12,4C9.5,4 7.45,5.82 7.06,8.19C6.73,8.07 6.37,8 6,8A3,3 0 0,0 3,11C3,12.11 3.6,13.08 4.5,13.6V13.59C5,13.87 5.14,14.5 4.87,14.96C4.59,15.43 4,15.6 3.5,15.32V15.33C2,14.47 1,12.85 1,11A5,5 0 0,1 6,6C7,3.65 9.3,2 12,2C15.43,2 18.24,4.66 18.5,8.03L19,8A4,4 0 0,1 23,12C23,13.5 22.2,14.77 21,15.46V15.46C20.5,15.73 19.91,15.57 19.63,15.09C19.36,14.61 19.5,14 20,13.72V13.73C20.6,13.39 21,12.74 21,12A2,2 0 0,0 19,10H17Z";
        private static string weather_lightning = "M6,16A5,5 0 0,1 1,11A5,5 0 0,1 6,6C7,3.65 9.3,2 12,2C15.43,2 18.24,4.66 18.5,8.03L19,8A4,4 0 0,1 23,12A4,4 0 0,1 19,16H18A1,1 0 0,1 17,15A1,1 0 0,1 18,14H19A2,2 0 0,0 21,12A2,2 0 0,0 19,10H17V9A5,5 0 0,0 12,4C9.5,4 7.45,5.82 7.06,8.19C6.73,8.07 6.37,8 6,8A3,3 0 0,0 3,11A3,3 0 0,0 6,14H7A1,1 0 0,1 8,15A1,1 0 0,1 7,16H6M12,11H15L13,15H15L11.25,22L12,17H9.5L12,11Z";
        private static string weather_lightning_rainy = "M4.5,13.59C5,13.87 5.14,14.5 4.87,14.96C4.59,15.44 4,15.6 3.5,15.33V15.33C2,14.47 1,12.85 1,11A5,5 0 0,1 6,6C7,3.65 9.3,2 12,2C15.43,2 18.24,4.66 18.5,8.03L19,8A4,4 0 0,1 23,12A4,4 0 0,1 19,16A1,1 0 0,1 18,15A1,1 0 0,1 19,14A2,2 0 0,0 21,12A2,2 0 0,0 19,10H17V9A5,5 0 0,0 12,4C9.5,4 7.45,5.82 7.06,8.19C6.73,8.07 6.37,8 6,8A3,3 0 0,0 3,11C3,12.11 3.6,13.08 4.5,13.6V13.59M9.5,11H12.5L10.5,15H12.5L8.75,22L9.5,17H7L9.5,11M17.5,18.67C17.5,19.96 16.5,21 15.25,21C14,21 13,19.96 13,18.67C13,17.12 15.25,14.5 15.25,14.5C15.25,14.5 17.5,17.12 17.5,18.67Z";
        #endregion

        #region Google
        static string ApplicationName = "Google Calendar API .NET Quickstart";
        UserCredential credential;
        CalendarService CaneldarService;
        TasksService TaskService;
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly, TasksService.Scope.TasksReadonly };
        #endregion

        #region Calendar
        private static Dictionary<string, string> GoogleCalendars = new Dictionary<string, string>()
        {
            {"Coolbots7", "primary" },
            {"BarPalace", "hnq23ejd84at95m71bq291a7uo@group.calendar.google.com" },
            {"Work", "aopn8feea7v26ov8sbte4ebrs8@group.calendar.google.com" },
            {"School", "quvqcm12ct2vb2skpke5hm4vmk@group.calendar.google.com" },
            {"Holidays", "en.usa#holiday@group.v.calendar.google.com" }
        };
        private Dictionary<string, Google.Apis.Calendar.v3.Data.Calendar> GoogleCalendarObjects = new Dictionary<string, Google.Apis.Calendar.v3.Data.Calendar>();
        private Dictionary<string, string> EventCalendars = new Dictionary<string, string>();
        private List<Event> Events = new List<Event>();

        private static Color Coolbots7CalendarColor = Color.FromRgb(23, 40, 85);
        private static Color BarPalaceCalendarColor = Color.FromRgb(93, 22, 114);
        private static Color WorkCalendarColor = Color.FromRgb(26, 85, 23);
        private static Color SchoolCalendarColor = Color.FromRgb(201, 91, 28);
        private static Color HolidayCalendarColor = Color.FromRgb(22, 114, 97);
        #endregion

        #region Todoist
        private string TodoistKey = ConfigurationManager.AppSettings["TodoistKey"];
        #endregion
        #endregion

        public MainWindow()
        {
            #region Initialize
            InitializeComponent();
            CreateGoogleCredential();

            CityID = ConfigurationManager.AppSettings["OpenWeatherMapCityID"];
            OpenWeatherMapAppID = ConfigurationManager.AppSettings["OpenWeatherMapAppID"];
            #endregion


            #region DateTime
            new Thread(new ThreadStart(TimeThread)).Start();
            #endregion

            #region Weather
            InitializeForecastGraph();
            new Thread(new ThreadStart(CurrentWeatherThread)).Start();
            new Thread(new ThreadStart(ForecastThread)).Start();
            #endregion

            #region Todoist
            new Thread(new ThreadStart(TodoistThread)).Start();
            #endregion

            #region Calendar
            new Thread(new ThreadStart(CalendarThread)).Start();
            #endregion

            #region GoogleTasks
            //// Define parameters of request.
            //TasklistsResource.ListRequest listRequest = TaskService.Tasklists.List();
            //listRequest.MaxResults = 10;

            //// List task lists.
            //IList<TaskList> taskLists = listRequest.Execute().Items;
            //Console.WriteLine("Task Lists:");
            //if (taskLists != null && taskLists.Count > 0)
            //{
            //    foreach (var taskList in taskLists)
            //    {
            //        Console.WriteLine("{0} ({1})", taskList.Title, taskList.Id);
            //        TasksResource.ListRequest taskRequest = TaskService.Tasks.List(taskList.Id);
            //        taskRequest.ShowCompleted = false;
            //        taskRequest.ShowDeleted = false;
            //        taskRequest.ShowHidden = true;

            //        IList<Google.Apis.Tasks.v1.Data.Task> tasks = taskRequest.Execute().Items;
            //        if (tasks != null && tasks.Count > 0)
            //        {
            //            foreach (Google.Apis.Tasks.v1.Data.Task task in tasks)
            //            {
            //                Console.WriteLine("\t{0} ({1}) [{2}]", task.Title, task.DueRaw, task.Notes);
            //            }
            //        }
            //        else
            //        {
            //            Console.WriteLine("\tNo Tasks in list.");
            //        }

            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No task lists found.");
            //}

            #endregion

        }

        #region ThreadLoops
        private void TimeThread()
        {
            while (true)
            {
                UpdateTimeText();
                UpdateDateText();
                Thread.Sleep(1000);
            }
        }

        private void CurrentWeatherThread()
        {
            while (true)
            {
                UpdateCurrentWeather();
                Thread.Sleep(15 * 1000); // 15 seconds
            }
        }

        private void ForecastThread()
        {
            while (true)
            {
                UpdateWeatherForecast();
                Thread.Sleep(1 * 60 * 60 * 1000);
            }
        }

        private void TodoistThread()
        {
            while (true)
            {
                UpdateTodoist();
                Thread.Sleep(30 * 1000); //30 seconds
            }
        }

        private void CalendarThread()
        {
            while(true)
            {
                UpdateGoogleCalendar();
                Thread.Sleep(30 * 1000); //30 seconds
            }
        }
        #endregion
        
        #region DisplayUpdateFunctions
        private void SetRainAmount(int height)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new DateTextMethodInvoker(UpdateDateText));
                return;
            }
          (new TextRange(RainAmount.Document.ContentStart, RainAmount.Document.ContentEnd)).Text = height + "in"; ;
        }

        delegate void ForecastMethodInvoker();
        private void UpdateWeatherForecast()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new ForecastMethodInvoker(UpdateWeatherForecast));
                return;
            }
            JObject Forecast = OpenWeatherMap.OpenWeatherMap.FiveDayForecast(CityID, OpenWeatherMapAppID);

            JArray Points = new JArray(((JArray)Forecast.GetValue("list")).OrderBy(p => (long)p["dt"]));

            List<string> labels = new List<string>();
            ChartValues<double> values = new ChartValues<double>();
            for (int i = 0; i < 8; i++)
            {
                JObject point = (JObject)Points[i];
                values.Add((int)Convert.ToDouble(point["main"]["temp"].ToString()));
                labels.Add(UnixTimeStampToDateTime(Convert.ToInt64(point["dt"].ToString())).ToString("HH:mm"));
            }

            SeriesCollection[0].Values = values;
            Labels = labels.ToArray();

            WeatherForecast.Update();

        }

        delegate void TimeTextMethodInvoker();
        private void UpdateTimeText()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new TimeTextMethodInvoker(UpdateTimeText));
                return;
            }
            (new TextRange(TimeText.Document.ContentStart, TimeText.Document.ContentEnd)).Text = DateTime.Now.ToLocalTime().Hour.ToString().PadLeft(2, '0') + ":" + DateTime.Now.ToLocalTime().Minute.ToString().PadLeft(2, '0') + ":" + DateTime.Now.ToLocalTime().Second.ToString().PadLeft(2, '0');
        }

        delegate void DateTextMethodInvoker();
        private void UpdateDateText()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new DateTextMethodInvoker(UpdateDateText));
                return;
            }
            (new TextRange(DateText.Document.ContentStart, DateText.Document.ContentEnd)).Text = DateTime.Now.ToLocalTime().DayOfWeek + " " + DateTime.Now.ToLocalTime().Year + "-" + DateTime.Now.ToLocalTime().Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.ToLocalTime().Day.ToString().PadLeft(2, '0');
        }

        delegate void WindSpeedMethodInvoker(double speed, double Angle);
        private void SetWindSpeedText(double speed, double Angle)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new WindSpeedMethodInvoker(SetWindSpeedText), new Object[] { speed, Angle });
                return;
            }
            (new TextRange(WindSpeed.Document.ContentStart, WindSpeed.Document.ContentEnd)).Text = speed.ToString() + " MPH " + AngleToDirection(Angle);
        }

        delegate void HumidityMethodInvoker(int humidity);
        private void SetHumidity(int humidity)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new HumidityMethodInvoker(SetHumidity), new Object[] { humidity });
                return;
            }
            (new TextRange(Humidity.Document.ContentStart, Humidity.Document.ContentEnd)).Text = humidity.ToString() + "%";
        }

        delegate void SunriseTimeMethodInvoker(long unixTime);
        private void SetSunriseTime(long unixTime)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new SunriseTimeMethodInvoker(SetSunriseTime), new Object[] { unixTime });
                return;
            }
            (new TextRange(SunriseTime.Document.ContentStart, SunriseTime.Document.ContentEnd)).Text = UnixTimeStampToDateTime(unixTime).ToString("HH:mm");
        }

        delegate void SunsetTimeMethodInvoker(long unixTime);
        private void SetSunsetTime(long unixTime)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new SunsetTimeMethodInvoker(SetSunsetTime), new Object[] { unixTime });
                return;
            }
            (new TextRange(SunsetTime.Document.ContentStart, SunsetTime.Document.ContentEnd)).Text = UnixTimeStampToDateTime(unixTime).ToString("HH:mm");
        }

        delegate void WeatherIconMethodInvoker(int WeatherCode);
        private void SetWeatherIcon(int WeatherCode)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new WeatherIconMethodInvoker(SetWeatherIcon), new Object[] { WeatherCode });
                return;
            }
            string path = string.Empty;
            if (WeatherCode > 800)
            {
                //Clouds
                if (WeatherCode > 802)
                    path = weather_cloudy;
                else
                    path = weather_partly_cloudy;
            }
            else if (WeatherCode == 800)
            {
                //Clear
                path = weather_sunny;
            }
            else if (WeatherCode >= 700)
            {
                //Atmosphere
                path = weather_fog;
            }
            else if (WeatherCode >= 600)
            {
                //Snow
                if (WeatherCode >= 615)
                {
                    //Snow + Rain
                    path = weather_snowy_rainy;
                }
                else if (WeatherCode >= 612)
                {
                    //Sleet / Hail
                    path = weather_hail;
                }
                else
                {
                    //Just Snow
                    path = weather_snowy;
                }
            }
            else if (WeatherCode >= 500)
            {
                //Rain
                if (WeatherCode >= 511)
                {
                    //Pouring
                    path = weather_pouring;
                }
                else
                {
                    //Rain
                    path = weather_rainy;
                }
            }
            else if (WeatherCode >= 300)
            {
                //Drizzle
                path = weather_pouring;
            }
            else if (WeatherCode >= 200)
            {
                //Thunderstorm
                if (WeatherCode >= 230)
                {
                    //thunder + drizzle
                    path = weather_lightning_rainy;
                }
                else if (WeatherCode >= 210)
                {
                    //thunder
                    path = weather_lightning;
                }
                else
                {
                    //thunder + rain
                    path = weather_lightning_rainy;
                }
            }
            Geometry Path = Geometry.Parse(path);
            WeatherIcon.Data = Path;
        }

        delegate void CurrentTemperatureMethodInvoker(int temperature);
        private void SetCurrentTemperature(int temperature)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new CurrentTemperatureMethodInvoker(SetCurrentTemperature), new Object[] { temperature });
                return;
            }
            (new TextRange(CurrentTemperature.Document.ContentStart, CurrentTemperature.Document.ContentEnd)).Text = temperature.ToString() + "°F";
        }

        delegate void GoogleCalendarMethodInvoker();
        private void UpdateGoogleCalendar()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new GoogleCalendarMethodInvoker(UpdateGoogleCalendar));
                return;
            }

            CalendarElements.Children.Clear();

            GetCalendars();
            GetAllCalendarEvents();
            this.Events = this.Events.OrderBy(e => (e.Start.DateTime == null) ? DateTime.ParseExact(e.Start.Date.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture) : e.Start.DateTime.Value).ToList();


            if (Events.Count() > 0)
            {
                int i = 0;
                foreach (Event Event in Events)
                {

                    string CalendarName = GoogleCalendars.FirstOrDefault(x => x.Value == EventCalendars[Event.Id]).Key;
                    string when = string.Empty;
                    DateTime date = (Event.Start.DateTime == null) ? DateTime.ParseExact(Event.Start.Date.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture) : Event.Start.DateTime.Value;


                    if (date.Date == DateTime.Now.Date)
                    {
                        when = "Today";
                    }
                    else if (date.Date == DateTime.Now.AddDays(1).Date)
                    {
                        when = "Tomorrow";
                    }
                    else
                    {
                        when = date.DayOfWeek.ToString();
                    }

                    if (Event.Start.DateTime != null)
                    {
                        when += " " + Event.Start.DateTime.Value.ToString("HH:mm") + " - " + Event.End.DateTime.Value.ToString("HH:mm");
                    }
                    else
                    {
                        when += " All Day";
                    }


                    Rectangle rect = new Rectangle();
                    rect.Width = 250;
                    rect.Height = 34;
                    rect.RadiusX = 3;
                    rect.RadiusY = 3;
                    rect.Margin = new Thickness(0, (10 + 34) * i, 0, 0); //L,R,T,B
                    rect.Fill = new SolidColorBrush(Color.FromRgb(120, 120, 120));
                    CalendarElements.Children.Add(rect);


                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = 10;
                    ellipse.Height = 10;
                    ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                    ellipse.VerticalAlignment = VerticalAlignment.Top;
                    ellipse.Margin = new Thickness(5, 5 + ((10 + 34) * i), 0, 0);
                    switch (CalendarName)
                    {
                        case "Coolbots7":
                            ellipse.Fill = new SolidColorBrush(Coolbots7CalendarColor);
                            break;
                        case "BarPalace":
                            ellipse.Fill = new SolidColorBrush(BarPalaceCalendarColor);
                            break;
                        case "Work":
                            ellipse.Fill = new SolidColorBrush(WorkCalendarColor);
                            break;
                        case "School":
                            ellipse.Fill = new SolidColorBrush(SchoolCalendarColor);
                            break;
                        case "Holidays":
                            ellipse.Fill = new SolidColorBrush(HolidayCalendarColor);
                            break;
                        default:
                            ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                            break;
                    }
                    CalendarElements.Children.Add(ellipse);


                    TextBox text = new TextBox();
                    text.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    text.Width = 240;
                    text.Height = 30;
                    text.Margin = new Thickness(17, 2 + ((10 + 34) * i), 0, 0);
                    text.Text = Event.Summary + Environment.NewLine + when;
                    text.FontFamily = new FontFamily("Arial Narrow");
                    text.FontSize = 12;
                    text.Background = null;
                    text.BorderBrush = null;
                    CalendarElements.Children.Add(text);


                    i++;
                }
            }
            //else
            //{

            //    Rectangle rect = new Rectangle();
            //    rect.Width = 250;
            //    rect.Height = 50;
            //    rect.RadiusX = 3;
            //    rect.RadiusY = 3;
            //    rect.Fill = new SolidColorBrush(Color.FromRgb(120, 120, 120));

            //    CalendarElements.Children.Add(rect);

            //    TextBox text = new TextBox();
            //    text.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            //    text.Width = 240;
            //    text.Height = 40;
            //    text.Margin = new Thickness(5, 5, 0, 0);
            //    text.Text = "No Upcoming Events";
            //    text.FontFamily = new FontFamily("Arial Narrow");
            //    text.FontSize = 16;
            //    CalendarElements.Children.Add(text);

            //}
        }

        delegate void TodoistMethodInvoker();
        private void UpdateTodoist()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new TodoistMethodInvoker(UpdateTodoist));
                return;
            }
            TaskElements.Children.Clear();

            IOrderedEnumerable<JObject> TodoistItems = Todoist.API.Sync.v7.Miscellaneous.GetAllTasksInfo("9cf9d77b581099abf0d99feccda144a201fdf0c3").Children<JObject>().Where(i => ((JToken)i["item"]["due_date_utc"]).Type != JTokenType.Null && DateTime.Parse(i["item"]["due_date_utc"].ToString()) >= DateTime.Now && DateTime.Parse(i["item"]["due_date_utc"].ToString()).Date <= DateTime.Now.Date.AddDays(5)).OrderBy(i => DateTime.Parse(i["item"]["due_date_utc"].ToString()).ToLocalTime());
            if (TodoistItems.Count() > 0)
            {
                int i = 0;
                foreach (JObject item in TodoistItems)
                {
                    Console.WriteLine(item.ToString());

                    Rectangle rect = new Rectangle();
                    rect.Width = 250;
                    rect.Height = 34;
                    rect.RadiusX = 3;
                    rect.RadiusY = 3;
                    rect.Margin = new Thickness(0, (10 + 34) * i, 0, 0);
                    rect.Fill = new SolidColorBrush(Color.FromRgb(120, 120, 120));
                    TaskElements.Children.Add(rect);



                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = 10;
                    ellipse.Height = 10;
                    ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                    ellipse.VerticalAlignment = VerticalAlignment.Top;
                    ellipse.Margin = new Thickness(5, 5 + ((10 + 34) * i), 0, 0);
                    int projectColorID = Convert.ToInt32(Todoist.API.Sync.v7.Miscellaneous.GetProjectInfo(item["item"]["project_id"].ToString(), "9cf9d77b581099abf0d99feccda144a201fdf0c3")["project"]["color"].ToString());

                    switch (projectColorID)
                    {
                        case 8:
                            ellipse.Fill = new SolidColorBrush(Color.FromRgb(201, 44, 28));
                            break;
                        case 9:
                            ellipse.Fill = new SolidColorBrush(Color.FromRgb(201, 169, 28));
                            break;
                        default:
                            ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                            break;
                    }
                    
                    TaskElements.Children.Add(ellipse);


                    string when = string.Empty;
                    DateTime duedate = DateTime.Parse(item["item"]["due_date_utc"].ToString()).ToLocalTime();

                    if (duedate.Date == DateTime.Now.Date)
                    {
                        when = "Today";
                    }
                    else if (duedate.Date == DateTime.Now.AddDays(1).Date)
                    {
                        when = "Tomorrow";
                    }
                    else
                    {
                        when = duedate.DayOfWeek.ToString();
                    }

                    if (Convert.ToBoolean(item["item"]["all_day"].ToString()) == true)
                    {
                        when += " All Day";
                    }
                    else
                    {
                        when += duedate.ToString(" HH:mm");
                    }

                    TextBox text = new TextBox();
                    text.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    text.Width = 230;
                    text.Height = 30;
                    text.Margin = new Thickness(17, 2 + ((10 + 34) * i), 0, 0);
                    text.Text = item["item"]["content"].ToString() + Environment.NewLine + when;
                    text.FontFamily = new FontFamily("Arial Narrow");
                    text.FontSize = 12;
                    text.Background = null;
                    text.BorderBrush = null;
                    TaskElements.Children.Add(text);


                    i++;
                }
            }
        }
        #endregion


        #region SupportFunctions
        private void CreateGoogleCredential()
        {
            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = System.IO.Path.Combine(credPath, ".credentials/calendar-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            CaneldarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            TaskService = new TasksService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        private void InitializeForecastGraph()
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries {
                    PointGeometry = DefaultGeometries.None,
                    LineSmoothness = 1,
                    Stroke = Brushes.White,
                    Fill = Brushes.Transparent,
                    StrokeThickness = 2,
                    DataLabels = true,
                    LabelPoint = point => point.Y + "°F",
                    Foreground = Brushes.White
                }

            };

            DataContext = this;
        }
        
        private void UpdateCurrentWeather()
        {

            JObject CurrentWeather = OpenWeatherMap.OpenWeatherMap.CurrentWeather(CityID, OpenWeatherMapAppID);
            //TODO Rain Percentage
            if (CurrentWeather.ContainsKey("precipitation"))
            {
                SetRainAmount(Convert.ToInt32(CurrentWeather["precipitation"]["value"].ToString()));
            }
            else
            {
                SetRainAmount(0);
            }
            SetWindSpeedText(Convert.ToDouble(CurrentWeather["wind"]["speed"].ToString()), Convert.ToDouble(CurrentWeather["wind"]["deg"]));
            SetHumidity(Convert.ToInt32(CurrentWeather["main"]["humidity"].ToString()));
            SetSunriseTime(Convert.ToInt64(CurrentWeather["sys"]["sunrise"]));
            SetSunsetTime(Convert.ToInt64(CurrentWeather["sys"]["sunset"].ToString()));
            SetWeatherIcon(Convert.ToInt32(CurrentWeather["weather"][0]["id"].ToString()));
            SetCurrentTemperature((int)Math.Round(Convert.ToDouble(CurrentWeather["main"]["temp"].ToString())));
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private string AngleToDirection(double Angle)
        {
            string Direction = string.Empty;

            if (Angle > 330 || Angle <= 30)
            {
                //North
                Direction = "N";
            }
            else if (Angle > 30 && Angle <= 60)
            {
                //North East
                Direction = "NE";
            }
            else if (Angle > 60 && Angle <= 120)
            {
                //East
                Direction = "E";
            }
            else if (Angle > 120 && Angle <= 150)
            {
                //South East
                Direction = "SE";
            }
            else if (Angle > 150 && Angle <= 210)
            {
                //South
                Direction = "S";
            }
            else if (Angle > 210 && Angle <= 240)
            {
                //South West
                Direction = "SW";
            }
            else if (Angle > 240 && Angle <= 300)
            {
                //West
                Direction = "W";
            }
            else if (Angle > 300 && Angle <= 330)
            {
                //Notrh West
                Direction = "NW";
            }


            return Direction;
        }

        private void GetAllCalendarEvents()
        {
            EventCalendars.Clear();
            Events.Clear();
            foreach (string name in GoogleCalendars.Keys)
            {
                string calendarId = GoogleCalendars[name];

                List<Event> events = this.GetNext24HourCalendarEvents(calendarId);
                foreach (Event e in events)
                {
                    EventCalendars.Add(e.Id, calendarId);
                }
                Events.AddRange(events);
            }
        }

        private void GetCalendars()
        {
            GoogleCalendarObjects.Clear();
            foreach (string name in GoogleCalendars.Keys)
            {
                GoogleCalendarObjects.Add(name, this.GetGoogleCalendar(GoogleCalendars[name]));
            }
        }

        private Google.Apis.Calendar.v3.Data.Calendar GetGoogleCalendar(string calendarId)
        {
            CalendarsResource.GetRequest request = CaneldarService.Calendars.Get(calendarId);
            Google.Apis.Calendar.v3.Data.Calendar calendar = request.Execute();

            if (calendar != null)
                return calendar;
            else
                return null;
        }

        private List<Event> GetNext24HourCalendarEvents(string calendarID)
        {
            EventsResource.ListRequest request = CaneldarService.Events.List(calendarID);
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;

            request.TimeMax = DateTime.Now.AddHours(24 * 3);
            //request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();

            if (events.Items != null)
                return events.Items.ToList();
            else
                return new List<Event>();
        }
        #endregion


        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
            Environment.Exit(0);
            return;
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Maximized)
            {
                WindowState = WindowState.Normal;
                WindowStyle = WindowStyle.SingleBorderWindow;

            }
            else
            {
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
            }

            Maximized = !Maximized;
        }
    }
}
