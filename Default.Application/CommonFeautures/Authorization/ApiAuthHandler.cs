using Default.Domain.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Default.Application.CommonFeautures.Authorization
{
	public class ApiAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
	{

		private readonly ClaimsPrincipal _id;

		// Constructor
		public ApiAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
		{
			var id = new ClaimsIdentity("Api");
			id.AddClaim(new Claim(ClaimTypes.Name, "", ClaimValueTypes.String, "Api"));
			_id = new ClaimsPrincipal(id);
		}

		// Handle authorization
		/// <summary>
		/// Check quyền tại hàm này luôn
		/// </summary>
		/// <returns></returns>
		protected override  Task<AuthenticateResult> HandleAuthenticateAsync()
		{

			var HeaderKeyName = AuthorizationHeader._Defaul;


			//if (!Request.Headers.ContainsKey("Authorization"))
			//	return await Task.Run(() =>
   //             {
			//		return AuthenticateResult.Fail("No authorization header");
			//	});
			

			//try
			//{
			//	var authHeader = AuthenticationHeaderValue.Parse(Request.Headers[HeaderKeyName]).Parameter;
			

			//	if (await CheckToken(authHeader))
			//	{
			//		var claims = new[] { new Claim(ClaimTypes.Name, username) };
			//		var identity = new ClaimsIdentity(claims, Scheme.Name);
			//		var principal = new ClaimsPrincipal(identity);
			//		var ticket = new AuthenticationTicket(principal, Scheme.Name);
			//	}
			//}
			//catch { }


			//var routeValues = Request.RouteValues;
			//if (routeValues.TryGetValue("controller", out object name))
			//{ // Note: Check "controller", not "action"
			//	string action = ((string)name).ToLower();
			//	if ((new List<string> { "list", "view", "add", "edit", "delete" }).Contains(action))
			//	{
			//		var secu = CreateSecurity();
			//		secu.LoadUserLevel();
			//		Security = secu; // Save the security object so the page does not need to create again
			//		string table = Config.ProjectId + (string)routeValues["table"];
			//		bool allowed = action switch
			//		{
			//			"list" => secu.AllowList(table),
			//			"view" => secu.AllowView(table),
			//			"add" => secu.AllowAdd(table),
			//			"edit" => secu.AllowEdit(table),
			//			"delete" => secu.AllowDelete(table),
			//			_ => false
			//		};

			//	}
			//}
			var allowed = true;
			if (allowed)
				return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(_id, "Api")));

			return Task.FromResult(AuthenticateResult.NoResult());
		}

		
	}
}
