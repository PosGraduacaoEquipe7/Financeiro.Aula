using Financeiro.Auth.Models.RefreshToken;
using MediatR;

namespace Financeiro.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
    {
        public RefreshTokenRequest Request { get; private set; }

        public RefreshTokenCommand(RefreshTokenRequest request)
        {
            Request = request;
        }
    }
}