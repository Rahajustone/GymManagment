using GymManagement.Application.Common.Behaviors;
using GymManagement.Application.Gyms.Commands.CreateGym;
using ErrorOr;
using GymManagement.Domain.Gyms;
using NSubstitute;
using FluentValidation;
using TestCommon.Gyms;
using MediatR;
using FluentValidation.Results;
using FluentAssertions;


namespace GymManagement.Application.UnitTests.Common.Behaviors;

public class ValidationBehaviorTests
{
    private readonly RequestHandlerDelegate<ErrorOr<Gym>> _mockNextBehaviour;
    private readonly IValidator<CreateGymCommand> _mockValidator;
    private readonly ValidationBehavior<CreateGymCommand, ErrorOr<Gym>> _validationBehaviour;

    public ValidationBehaviorTests()
    {
        // create a next behavior(mock)
        _mockNextBehaviour = Substitute.For<RequestHandlerDelegate<ErrorOr<Gym>>>();

        // Create vaidation mock
        _mockValidator = Substitute.For<IValidator<CreateGymCommand>>();

        // Create validation behaviour (SUT)
        _validationBehaviour = new ValidationBehavior<CreateGymCommand, ErrorOr<Gym>>(_mockValidator);
    }


    [Fact]
    public async Task InvokeBehavior_WhenValidatorResultIsValid_ShouldInvokeNextBehavior()
    {
        // Arrange
        var createGymRequest = GymCommandFactory.CreateCreateGymCommand();
        var gym = GymFactory.CreateGym();

        _mockValidator.ValidateAsync(createGymRequest, Arg.Any<CancellationToken>())
        .Returns(new ValidationResult());

        _mockNextBehaviour.Invoke().Returns(gym);

        // Act
        var result = await _validationBehaviour.Handle(createGymRequest, _mockNextBehaviour, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(gym);
    }
}