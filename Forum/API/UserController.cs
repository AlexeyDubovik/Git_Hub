using Forum.DAL.Context;
using Forum.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Forum.API
{
    [Route("api/User")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IntroContext _context;
        private readonly HttpRequestGuidCheck _GuidCheck;
        private readonly DAO_Worker_Facade _DAO;
        private readonly IHasher _hasher;
        public UserController(IntroContext context, IHasher hasher, DAO_Worker_Facade DAO, HttpRequestGuidCheck guidCheck)
        {
            _context = context;
            _hasher = hasher;
            _GuidCheck = guidCheck;
            _DAO = DAO;
        }

        // GET: api/<UserController>
        [HttpGet]
        public string Get(string? login, string? password)
        {
            if(string.IsNullOrEmpty(login))
            {
                HttpContext.Response.StatusCode = 409;
                return "Conflict: login required";
            }
            if (string.IsNullOrEmpty(password))
            {
                HttpContext.Response.StatusCode = 409;
                return "Conflict: password required";
            }
            DAL.Entities.User user = _context.Users?.Where(u => u.Login == login).FirstOrDefault()!;
            if(user == null)
            {
                HttpContext.Response.StatusCode = 401;
                return "Unauthorized: credentials rejected";
            }
            else
            {
                var Pass = _hasher.Hash(password + user.PassSalt);
                var UserFromPass = _context.Users?.Where(
                    u => u.Login.Equals(login) &&
                    u.PassHash.Equals(Pass)).FirstOrDefault();
                if (UserFromPass != null)
                    return UserFromPass.ID.ToString();
                return $"{ login ?? "--" } { password ?? "--" }";
            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public object Get(String? id)
        {
            Guid guid = _GuidCheck.Check(HttpContext, id);
            if (guid == Guid.Empty)
                return "Conflict: invalid id format (GUID required)";
            var user = _context.Users!.Find(guid);
            if(user == null) {
                HttpContext.Response.StatusCode = 401;
                return "Unauthorized: credentials rejected";
            }
            return user with { PassHash = "***", PassSalt = "***"};
        }

        // POST api/<UserController>
        [HttpPost]
        public string Post([FromBody] string value)
        {
            return $"POST {value}";
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public String Put(string id, [FromForm] Models.RegUserModel userData)
        {
            string Avatar = "";
            Guid guid = _GuidCheck.Check(HttpContext, id);
            if (guid == Guid.Empty)
                return "Conflict: invalid id format (GUID required)";
            var user = _context.Users!.Find(guid);
            String[] Result = new String[5];
            if(user == null) {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: user with this id does not exist";
            }
            if (userData == null) {
                HttpContext.Response.StatusCode = 409;
                return "Conflict:Data is null";
            }
            if (userData.Login != null) {
                Result[0] = _DAO.ChangeUserLogin(user, userData.Login);
            }
            if (userData.RealName != null) {
                Result[1] = _DAO.ChangeUserRealName(user, userData.RealName);
            }
            if (userData.Email != null) {
                Result[2] = _DAO.ChangeUserEmail(user, userData.Email);
            }
            if (userData.Avatar != null) {
                Result[3] = _DAO.ChangeUserAvatar(user, userData.Avatar);
                Avatar = user.Avatar;
            }
            if (userData.Password1 != null && userData.Password2 != null) {
                Models.ChUserPasswordModel PassForm = new Models.ChUserPasswordModel();
                PassForm.OldPassword  = userData.Password1;
                PassForm.NewPassword1 = userData.Password2;
                PassForm.NewPassword2 = userData.Password2;
                Result[4] = _DAO.ChangeUserPassword(user, PassForm);
            }
            var userToJson = user with 
            {
                Login    = user.Login    + $" [{Result[0]}]",
                RealName = user.RealName + $" [{Result[1]}]",
                Email    = user.Email    + $" [{Result[2]}]",
                Avatar   = user.Avatar   + $" [{Result[3]}]",
                PassHash = "***"         + $" [{Result[4]}]",
                PassSalt = "***" 
            };
            return JsonSerializer.Serialize(userToJson);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return id.ToString();
        }
    }
}
