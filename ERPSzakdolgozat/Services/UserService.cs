using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Services
{
	public interface IUserService
	{
		//User Authenticate(string username, string password); // lehet törölni kell majd...
		IEnumerable<User> GetAll();
		User GetById(int id);
	}

	public class UserService : IUserService
	{
		private List<User> _users = new List<User>();

		private readonly AppSettings _appSettings;

		public UserService(IOptions<AppSettings> appSettings) // dependency injection
		{
			_appSettings = appSettings.Value;
		}

		//public User Authenticate(string username, string password)
		//{
		//	var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);

		//	// return null if user not found
		//	if (user == null)
		//		return null;

		//	// authentication successful so generate jwt token
		//	var tokenHandler = new JwtSecurityTokenHandler();
		//	var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
		//	var tokenDescriptor = new SecurityTokenDescriptor
		//	{
		//		Subject = new ClaimsIdentity(new Claim[]
		//		{
		//			new Claim(ClaimTypes.Name, user.Id.ToString()),
		//			new Claim(ClaimTypes.Role, user.Role)
		//		}),
		//		Expires = DateTime.UtcNow.AddDays(7),
		//		SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		//	};
		//	var token = tokenHandler.CreateToken(tokenDescriptor);
		//	user.Token = tokenHandler.WriteToken(token);

		//	// remove password before returning
		//	user.Password = null;

		//	return user;
		//}

		public IEnumerable<User> GetAll()
		{
			return _users;
		}

		public User GetById(int id)
		{
			return _users.FirstOrDefault(u => u.Id == id);
		}
	}
}
