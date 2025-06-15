using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderFood_BE.Application.Models.Requests.Auth;
using OrderFood_BE.Application.Models.Response.Auth;

namespace OrderFood_BE.Application.UseCase.Interfaces.Auth
{
    public interface IAuthenticationUseCase
    {
        Task<TokenResponse> StudentLoginAsync(IdTokenRequest request);
        Task<TokenResponse> GetNewAccessToken(TokenRequest request);
    }
}
