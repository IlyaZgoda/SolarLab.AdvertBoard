using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Users;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Users.Register
{
    public class RegisterUserCommandHandler(
        IIdentityService identityService, 
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork) : ICommandHandler<RegisterUserCommand, UserResponse>
    {
        public async Task<Result<UserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {  
            var firstNameResult = FirstName.Create(request.FirstName);
            var lastNameResult = LastName.Create(request.LastName);
            var middleNameResult = MiddleName.Create(request.MiddleName);
            var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber);
            var contactEmailResult = ContactEmail.Create(request.ContactEmail ?? request.Email);

            var userDataResult = Result.FirstFailureOrSuccess(firstNameResult, lastNameResult, middleNameResult, phoneNumberResult, contactEmailResult);

            if (userDataResult.IsFailure)
            {
                return Result.Failure<UserResponse>(userDataResult.Error);
            }

            var identityUserIdResult = await identityService.CreateIdentityUserAsync(request.Email, request.Password);

            if (identityUserIdResult.IsFailure)
            {
                return Result.Failure<UserResponse>(identityUserIdResult.Error);
            }

            var user = User.Create(
                identityUserIdResult.Value, 
                firstNameResult.Value, 
                lastNameResult.Value, 
                middleNameResult.Value, 
                contactEmailResult.Value, 
                phoneNumberResult.Value);

            userRepository.Add(user);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new UserResponse(user.Id);
        }
    }
}
