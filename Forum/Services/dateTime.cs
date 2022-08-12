
namespace Forum.Services
{
    //Это мой сервис для показа даты
    public class dateTime : ITimeManage
    {
        public string Date => System.DateTime.Now.Date.ToString();
        public string Time => System.DateTime.Now.ToShortTimeString();
       
    }
}
