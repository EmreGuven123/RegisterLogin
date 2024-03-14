using System.Diagnostics.Metrics;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using registerLogin.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace registerLogin.Controllers
{

	[ApiController]
	[Route("[controller]")]
	public class RegisterController : Controller
	{

		private readonly RegLogContext _context;

		public RegisterController(RegLogContext context)
		{
			_context = context;
		}
		[HttpGet("AdminPanel")]
		public async Task<IActionResult> AdminPanel()
		{
			var data = await _context.registerLogin.ToListAsync();
			return View(data);
		}
		[HttpPost("AdminPanel")]
		public async Task<IActionResult> AdminPanel(AdminInfo adminInfo)
		{
			AdminPanelResponse adminPanelResponse = new AdminPanelResponse();
			RegisterLogin openBlocker = _context.registerLogin.FirstOrDefault(o => o.Id == adminInfo.id);
			if (openBlocker != null)
			{
				openBlocker.IsBlocked = false;
				openBlocker.InvalidLoginCount = 0;
				await _context.SaveChangesAsync();
				adminPanelResponse.Message = "Kullanıcının blokesi kaldırıldı.";
			}
			adminPanelResponse = new AdminPanelResponse();
			adminPanelResponse.Message = "Kullanıcının maili hatalı.";
			adminPanelResponse.IsSuccess = true;

			return adminPanelResponse.JsonResult();
		}
		[HttpPost("Delete")]
		public async Task<IActionResult> Delete(DeleteRequest deleteRequest)
		{
			AdminPanelResponse adminPanelResponse = new AdminPanelResponse();

			RegisterLogin deleterUserData = _context.registerLogin.FirstOrDefault(x => x.Id == deleteRequest.Id);
			if (deleterUserData != null)
			{
				_context.registerLogin.Remove(deleterUserData);
				_context.SaveChanges();
			}
			return adminPanelResponse.JsonResult();
		}

		[HttpGet("ForgetPassword")]
		public async Task<IActionResult> ForgetPassword()
		{
			return View();
		}
		[HttpPost("ForgetPassword")]
		public async Task<IActionResult> ForgetPassword(EditInfo editinfo)
		{
			if (ModelState.IsValid)
			{
				UserPanelResponse userPanelResponse = new UserPanelResponse();
				RegisterLogin regMail = _context.registerLogin.FirstOrDefault(r => r.Mail == editinfo.Mail);

				if (string.IsNullOrEmpty(regMail.Mail))
				{
					userPanelResponse.Message = "Mail kısmı boş bırakılamaz";
					return userPanelResponse.JsonResult();
				}

				if (regMail.Mail != editinfo.Mail)
				{
					userPanelResponse.Message = "Böyle bir mail bulunamadı";
					return userPanelResponse.JsonResult();
				}
				RegisterLogin registerLogin = _context.registerLogin.FirstOrDefault(p => p.Id == regMail.Id);

				if (string.IsNullOrEmpty(registerLogin.Password))
				{
					userPanelResponse.Message = "Şifre kaydı bulunamadı";
					return userPanelResponse.JsonResult();
				}

				registerLogin.Password = editinfo.Password;
				await _context.SaveChangesAsync();
				return new JsonResult(editinfo);
			}
			return View(editinfo);
		}

		[HttpGet("Login")]
		public async Task<IActionResult> Login()
		{
			return View(await _context.registerLogin.ToListAsync());
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginInfo loginInfo)
		{
			LoginResponse response;
			if (string.IsNullOrEmpty(loginInfo.Email))
			{
				response = new LoginResponse(false, "Kullanıcı maili hatalı.");
				return response.JsonResult();
			}

			RegisterLogin mail = await _context.registerLogin.FirstOrDefaultAsync(r => r.Mail == loginInfo.Email);

			if (mail == null)
			{
				response = new LoginResponse(false, "Girdiğiz mail veri tabanında bulunamamaktadır .");
				return response.JsonResult();
			}

			if (string.IsNullOrEmpty(loginInfo.Password))
			{
				response = new LoginResponse(false, "Şifre boş olmamalı.");
				return response.JsonResult();
			}

			RegisterLogin registerLogin = _context.registerLogin.FirstOrDefault(p => p.Id == mail.Id);
			if (registerLogin.IsBlocked == true)
			{
				//throw new Exception("Hesabınızdaki Blokeyi yöneticilere başvurarak kaldırın lütfen.");
				response = new LoginResponse(false, "Hesabınızdaki Blokeyi yöneticilere başvurarak kaldırabilirsiniz.");
			}
			if (registerLogin.Password != loginInfo.Password)
			{
				registerLogin.InvalidLoginCount++;
				await _context.SaveChangesAsync();
				response = new LoginResponse(false, "Girdiğiz şifre hatalı .");
				if (registerLogin.InvalidLoginCount >= 3)
				{
					registerLogin.IsBlocked = true;
					await _context.SaveChangesAsync();
					response = new LoginResponse(false, "3 kez yanlış şifre girdiğiniz için hesabınız bloke edilmiştir.");
				}
				return response.JsonResult();
			}
			registerLogin.InvalidLoginCount = 0;
			await _context.SaveChangesAsync();
			response = new LoginResponse(true, string.Empty);
			return response.JsonResult(); ;
		}


		[HttpGet("Index")]
		public async Task<IActionResult> Index()
		{
			return View(await _context.registerLogin.ToListAsync());
		}

		[HttpPost("Index")]
		public async Task<IActionResult> Index(RegisterInfo info)
		{
			UserPanelResponse userPanelResponse = new UserPanelResponse();

			if (ModelState.IsValid)
			{
				if (_context.registerLogin.Any(r => r.Mail == info.Mail))
				{
					userPanelResponse.Message = "bu mail zaten kullanılıyor..";
					return userPanelResponse.JsonResult();
				}

				RegisterLogin registerUserInfo = new RegisterLogin();
				registerUserInfo.Mail = info.Mail;
				registerUserInfo.Name = info.Name;
				registerUserInfo.Surname = info.Surname;
				registerUserInfo.Password = info.Password;
				if (registerUserInfo == null)
				{
					userPanelResponse.Message = " İsim veya Soyisim  Boş ...";

					return userPanelResponse.JsonResult();
				}

				if (info.Password != info.ConfirmPassword)
				{
					userPanelResponse.Message = "şifreler birbirine eşit olmalı..";
					return userPanelResponse.JsonResult();
				}

				if (string.IsNullOrEmpty(registerUserInfo.Password))
				{
					userPanelResponse.Message = "Şifre Boş olmamalı...";
					return userPanelResponse.JsonResult();

				}
				_context.Add(registerUserInfo);
				await _context.SaveChangesAsync();
				return new JsonResult(info);
			}
			return View(info);
		}
	}
}


public class LoginInfo
{
	public string Email { get; set; }
	public string Password { get; set; }
}

public class RegisterInfo
{
	public int Id { get; set; }
	public string ConfirmPassword { get; set; }
	public string Password { get; set; }
	public string Mail { get; set; }
	public string Name { get; set; }
	public string Surname { get; set; }
}
public class AdminInfo
{
	public int id { get; set; }

}
public class EditInfo
{
	public int Id { get; set; }
	public string Mail { get; set; }
	public string ConfirmPassword { get; set; }
	public string Password { get; set; }
}

public class LoginResponse
{
	public bool IsSuccess { get; set; }
	public string Message { get; set; }

	public LoginResponse(bool isSuccess, string message)
	{
		IsSuccess = isSuccess;
		Message = message;
	}

	public JsonResult JsonResult()
	{
		return new JsonResult(this);
	}
}

public class AdminPanelResponse : Response
{

}

public class UserPanelResponse : Response
{

}
public class RegisterPanelResponse : Response 
{

}

public abstract class Response
{
	public bool IsSuccess { get; set; }
	public string Message { get; set; }
	public JsonResult JsonResult() { return new JsonResult(this); }
}

public class DeleteRequest
{
	public int Id { get; set; }
}
