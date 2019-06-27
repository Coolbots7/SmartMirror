using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RESTWebRequest;

namespace Todoist.API.REST
{
    public class v8
    {
        public static JObject GetTask(string TaskID, string BearerToken)
        {
            return JObject.Parse(RESTWebRequest.RESTWebRequest.GET("https://beta.todoist.com/API/v8/tasks/" + TaskID, "Bearer " + BearerToken));
        }

        public static JArray GetAllTasks(string BearerToken)
        {
            return JArray.Parse(RESTWebRequest.RESTWebRequest.GET("https://beta.todoist.com/API/v8/tasks", "Bearer " + BearerToken));
        }

        public static JObject GetProject(string ProjectID, string BearerToken)
        {
            return JObject.Parse(RESTWebRequest.RESTWebRequest.GET("https://beta.todoist.com/API/v8/projects/" + ProjectID, "Bearer " + BearerToken));
        }

        public static JArray GetAllProjects(string BearerToken)
        {
            return JArray.Parse(RESTWebRequest.RESTWebRequest.GET("https://beta.todoist.com/API/v8/projects", "Bearer " + BearerToken));
        }

        public static JObject GetLabel(string LabelID, string BearerToken)
        {
            return JObject.Parse(RESTWebRequest.RESTWebRequest.GET("https://beta.todoist.com/API/v8/labels/" + LabelID, "Bearer " + BearerToken));
        }

        public static JArray GetAllLabels(string BearerToken)
        {
            return JArray.Parse(RESTWebRequest.RESTWebRequest.GET("https://beta.todoist.com/API/v8/labels", "Bearer " + BearerToken));
        }

    }
}
