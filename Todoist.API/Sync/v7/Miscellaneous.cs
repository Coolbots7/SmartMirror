using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RESTWebRequest;

namespace Todoist.API.Sync.v7
{
    public class Miscellaneous
    {
        public static JObject GetTaskInfo(string item_id, string token)
        {
            return JObject.Parse(RESTWebRequest.RESTWebRequest.GET("https://todoist.com/api/v7/items/get?item_id=" + item_id + "&token=" + token));
        }

        public static JObject GetProjectInfo(string project_id, string token)
        {
            return JObject.Parse(RESTWebRequest.RESTWebRequest.GET("https://todoist.com/api/v7/projects/get?project_id=" + project_id + "&token=" + token));
        }

        public static JObject GetProjectData(string project_id, string token)
        {
            return JObject.Parse(RESTWebRequest.RESTWebRequest.GET("https://todoist.com/api/v7/projects/get_data?project_id=" + project_id + "&token=" + token));
        }

        public static JArray GetAllTasksInfo(string token)
        {
            JArray Items = new JArray();

            foreach(JObject item in Todoist.API.REST.v8.GetAllTasks(token))
            {
                Items.Add(GetTaskInfo(item["id"].ToString(), token));
            }

            return Items;
        }
    }
}
