using System.Globalization;

namespace Forum.Services
{
    public class dateTimeFormat : ITimeManage
    {
        public string Date => System.DateTime.Now.Date.ToString("D",
                 CultureInfo.CreateSpecificCulture("pt-BR"));
        public string Time => System.DateTime.Now.ToShortTimeString();
    }
}
