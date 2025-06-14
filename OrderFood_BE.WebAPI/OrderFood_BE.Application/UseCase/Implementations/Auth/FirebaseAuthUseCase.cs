using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using OrderFood_BE.Application.UseCase.Interfaces.Auth;

namespace OrderFood_BE.Application.UseCase.Implementations.Auth
{
    public class FirebaseAuthUseCase : IFirebaseAuthUseCase
    {
        private readonly FirebaseApp _firebaseApp;
        public FirebaseAuthUseCase()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                _firebaseApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("D:\\student-order-food-firebase-adminsdk-fbsvc-e0b64bf4df.json")
                });
            }
            else
            {
                _firebaseApp = FirebaseApp.DefaultInstance;
            }
        }
        public async Task<string> VerifyIdTokenAsync(string idToken)
        {
            var auth = FirebaseAuth.GetAuth(_firebaseApp);
            var decodeToken = await auth.VerifyIdTokenAsync(idToken);
            string uid = decodeToken.Uid;
            return uid;
        }
    }
}
