using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginApp.Components;
using LoginApp.Models;
using LoginApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using static LoginApp.Components.Enums.Api;

namespace LoginApp.Controllers
{
    public class MembershipController : Controller
    {
        private UserRepository userRepository;
        public MembershipController(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]JToken req)
        {
            JToken data = req["data"];

            var phone = data.Value<string>("phone");
            var password = data.Value<string>("password");
            var passwordRepeat = data.Value<string>("passwordRepeat");

            if (string.IsNullOrEmpty(phone))
                return Json(new ApiResponse(Error.MissingData) { data = nameof(phone) });

            if (string.IsNullOrEmpty(password))
                return Json(new ApiResponse(Error.MissingData) { data = nameof(password) });

            if (password != passwordRepeat)
                return Json(new ApiResponse(Error.PasswordsMustMatch));

            if (!Validation.IsValidPhone(phone))
                return Json(new ApiResponse(Error.PhoneIsNotValid));

            if (!Validation.IsValidPassword(password))
                return Json(new ApiResponse(Error.PasswordIsNotValid));

            string activationCode = new Random().Next(100000, 999999).ToString();

            string token = Guid.NewGuid().ToString("n").Replace("=", "").Replace("+", "") + Guid.NewGuid().ToString("n").Replace("=", "").Replace("+", "");
            User u = new User()
            {
                phone = phone,
                password = password,
                token = token,
                activationCode = activationCode
            };

            int userId = await userRepository.RegisterAsync(u);

            if (userId <= 0)
                return Json(new ApiResponse(Error.PhoneIsAlreadyUsed));

            u.id = userId;

            return Json(new ApiResponse(new { }));
        }

        [HttpPost]
        [Route("verifyPhone")]
        public async Task<IActionResult> VerifyPhone([FromBody]JToken req)
        {
            JToken data = req["data"];

            var code = data.Value<string>("code");

            if (string.IsNullOrEmpty(code))
                return Json(new ApiResponse(Error.MissingData) { data = nameof(code) });

            User user = await userRepository.VerifyPhoneAsync(code);

            if (user == null)
                return Json(new ApiResponse(Error.CodeIsNotValid));


            return Json(new ApiResponse(user));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]JToken req)
        {
            JToken data = req["data"];

            var emailOrPhone = data.Value<string>("emailOrPhone");
            var password = data.Value<string>("password");

            string email = "", phone = "";
            if (emailOrPhone.Contains("@"))
                email = emailOrPhone;
            else
                phone = emailOrPhone;

            if (string.IsNullOrEmpty(emailOrPhone))
                return Json(new ApiResponse(Error.MissingData) { data = nameof(emailOrPhone) });

            if (string.IsNullOrEmpty(password))
                return Json(new ApiResponse(Error.MissingData) { data = nameof(password) });

            if (!string.IsNullOrEmpty(phone) && !Validation.IsValidPhone(phone))
                return Json(new ApiResponse(Error.PhoneIsNotValid));

            if (!string.IsNullOrEmpty(email) && !Validation.IsValidEmail(email))
                return Json(new ApiResponse(Error.EmailIsNotValid));

            var user = await userRepository.LoginAsync(phone, email);

            if (user == null)
                return Json(new ApiResponse(Error.EmailOrPhoneNotRegistered));
            else if (user.password != password)
                return Json(new ApiResponse(Error.PasswordIsWrong));

            return Json(new ApiResponse(user));
        }

    }
}