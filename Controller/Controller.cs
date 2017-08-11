using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace proj
{
	public class projController:Controller
	{
		private readonly DbConnector _dbConnector;

		public projController(DbConnector connect)
		{
			_dbConnector = connect;
		}

		[HttpGet]
		[Route("")]
		public IActionResult Index()
		{
			List<Dictionary<string, object>> AllMessages = _dbConnector.Query("select * from messages");
			foreach(var i in AllMessages)
			{
				i.Add("user", _dbConnector.Query("select first from users where id = "+i["user_id"]));
				i.Add("cmts", _dbConnector.Query("select * from comments where message_id = " + i["id"]));
				foreach(var j in (List<Dictionary<string, object>>)i["cmts"])
					j.Add("user", _dbConnector.Query("select first from users where id = "+j["user_id"]));
			}
			if(HttpContext.Session.GetString("user")==null)
				return View(AllMessages);
			return View("Indexlogged",AllMessages);
		}


		[HttpGet]
		[Route("display")]
		public IActionResult Display()
		{
			List<Dictionary<string, object>> AllUsers = _dbConnector.Query("select first, last from users");
			List<User> all = new List<User>();
			return View(AllUsers);
		}

		[HttpGet]
		[Route("register")]
		public IActionResult Register()
		{
			ViewBag.Err = ModelState.Values;
			return View();
		}

		[HttpPost]
		[Route("register")]
		public IActionResult postReg(User user)
		{
			if(ModelState.IsValid)
			{
				_dbConnector.Execute("insert into users(first, last, email, password) values(\""+user.FirstName+"\", \""+user.LastName+"\", \""+user.Email+"\", \""+user.Password+"\");");
			}
			else
			{
				ViewBag.Err = ModelState.Values;
				return View("Register");
			}
			return RedirectToAction("index");
		}

		[HttpGet]
		[Route("logout")]
		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("index");
		}

		[HttpGet]
		[Route("login")]
		public IActionResult Loginget()
		{
			return View("Login");
		}

		[HttpPost]
		[Route("login")]
		public IActionResult Login(string email, string password)
		{
			List<Dictionary<string, object>> LoginInfo = _dbConnector.Query("select id, email, password from users where Email=\""+email+"\" and Password=\""+password+"\"");
			ViewBag.msg = "login failed";
			foreach(var i in LoginInfo)
			{
				if((string)i["email"]==email && (string)i["password"]==password)
				{
					HttpContext.Session.SetInt32("user", (int)i["id"]);
					return RedirectToAction("index");
				}
			}
			ViewBag.Err = "Login Failed";
			return View();
		}

		[HttpPost]
		[Route("newmessage")]
		public IActionResult Message(string msg)
		{
			_dbConnector.Execute("insert into messages(user_id, message) values( "+HttpContext.Session.GetInt32("user")+", \""+msg+"\");");
			return RedirectToAction("index");
		}

		[HttpPost]
		[Route("newcomment")]
		public IActionResult Message(int msg, string cmt)
		{
			_dbConnector.Execute("insert into comments(user_id, message_id, comment) values( "+HttpContext.Session.GetInt32("user")+", "+msg+", \""+cmt+"\");");
			return RedirectToAction("index");
		}
	}
}

