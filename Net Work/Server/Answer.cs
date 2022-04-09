using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    public class Answer
    {
        String data;
        BullshitGenerator bsg;
        public Answer(String _data)
        {
            data = _data;
            bsg = new BullshitGenerator();
        }
        public Library.Response buildResponse(ref Library.Response response)
        {
            try
            {
                data = DateTime.Now.ToString() + "\n" + bsg.getGreeting() + bsg.getNextSentence() + bsg.getEnding();
                response.data = data;
                response.success = true;
                response.status = Library.ResponseCodes.Ok;
                response.statusText = "Ok";
            }
            catch (Exception ex)
            {
                response.success = false;
                response.status = Library.ResponseCodes.Error;
                response.statusText = ex.Message;
            }
            return response;
        }
    }
}
