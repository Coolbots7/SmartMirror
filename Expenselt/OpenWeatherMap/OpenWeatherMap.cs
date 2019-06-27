using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RESTWebRequest;

namespace OpenWeatherMap
{
    public class OpenWeatherMap
    {
        public enum Format {JSON, XML, HTML };
        public enum Unit { Standard, Metric, Imperial };

        public static JObject CurrentWeather(string cityID, string appid, Format mode = Format.JSON, Unit unit = Unit.Imperial)
        {
            //string data = RESTWebRequest.RESTWebRequest.GET("https://samples.openweathermap.org/data/2.5/weather?id=" + cityID + "&appid=" + appid + "&mode=" + mode.ToString().ToLower() + "&units=" + unit.ToString().ToLower());
            string data = RESTWebRequest.RESTWebRequest.GET("https://api.openweathermap.org/data/2.5/weather?id=" + cityID + "&appid=" + appid + "&units=" + unit.ToString().ToLower());

            if (!string.IsNullOrEmpty(data))
                return JObject.Parse(data);
            else
                return null;
        }

        public static JObject FiveDayForecast(string cityID, string appid, Format mode = Format.JSON, Unit unit = Unit.Imperial)
        {
            string data = RESTWebRequest.RESTWebRequest.GET("https://api.openweathermap.org/data/2.5/forecast?id=" + cityID + "&appid=" + appid + "&units=" + unit.ToString().ToLower());

            if (!string.IsNullOrEmpty(data))
                return JObject.Parse(data);
            else
                return null;
        }
    }
}
