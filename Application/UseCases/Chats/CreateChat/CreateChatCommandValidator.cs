using FluentValidation;

namespace Application.UseCases.Chats.CreateChat;

public class CreateChatCommandValidator:AbstractValidator<CreateChatCommand>
{
    public CreateChatCommandValidator()
    {
        RuleFor(x=>x.ChatName)
            .NotEmpty()
            .WithMessage("Chat name is required")
            .MaximumLength(150)
            .WithMessage("Chat name cannot exceed 150 characters")
            .Matches(@"^[a-zA-Z0-9_\-\.]+$");
    }
}