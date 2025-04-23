using Application.DTOs.User;
using MediatR;

namespace Application.UseCases.Users.Auth;

public record LoginUserCommand(string Identity,string Password):IRequest<string>;