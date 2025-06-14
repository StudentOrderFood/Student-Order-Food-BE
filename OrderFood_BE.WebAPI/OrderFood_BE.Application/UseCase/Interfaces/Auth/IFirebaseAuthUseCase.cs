using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;

namespace OrderFood_BE.Application.UseCase.Interfaces.Auth
{
    public interface IFirebaseAuthUseCase
    {
        Task<string> VerifyIdTokenAsync(string idToken);
    }
}
