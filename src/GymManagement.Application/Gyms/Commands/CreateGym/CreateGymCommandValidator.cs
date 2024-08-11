using FluentValidation;

namespace GymManagement.Application.Gyms.Commands.CreateGym;

public class CreateGymCommandValidator : AbstractValidator<CreateGymCommand>
{
    public CreateGymCommandValidator()
    {
        RuleFor(x => x.Name)
        .NotEmpty()
        .MinimumLength(3)
        .MaximumLength(100);
    }
}