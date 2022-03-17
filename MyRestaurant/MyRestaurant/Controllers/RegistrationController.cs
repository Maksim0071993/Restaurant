﻿using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MyRestaurant.BusinessLogic.Interfaces;
using MyRestaurant.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyRestaurant.Presentation.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IUserService _userService;

        public RegistrationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] UserViewModel model)
        {
            var destObject = model.Adapt<MyRestaurant.BusinessLogic.Models.UserModel>();
            var resultVerivicationEmail = _userService.PhoneVerifaction(model.PhoneNumber);
            if (resultVerivicationEmail == true)
            {
                ModelState.AddModelError("", "Пользователь с таким телефоном существует");
            }
            else if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Пароли должны совпадать");
            }
            else if (model.PhoneNumber == null)
            {
                ModelState.AddModelError("", "Поле 'Номер телефона' должно быть заполнено");
            }
            else
            {
                var result = _userService.Register(destObject);
                await Authenticate(result);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        private async Task Authenticate(int userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString())
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
