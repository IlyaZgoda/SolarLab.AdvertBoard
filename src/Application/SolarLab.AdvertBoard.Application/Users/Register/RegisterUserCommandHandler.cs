using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Users;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Users.Register
{
    public class RegisterUserCommandHandler(
        IUserManagerProvider userManagerProvider, 
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork) : ICommandHandler<RegisterUserCommand, UserIdResponse>
    {
        public async Task<Result<UserIdResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {  
            var firstNameResult = FirstName.Create(request.FirstName);
            var lastNameResult = LastName.Create(request.LastName);
            var middleNameResult = MiddleName.Create(request.MiddleName);
            var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber);
            var contactEmailResult = string.IsNullOrWhiteSpace(request.ContactEmail) 
                ? ContactEmail.Create(request.Email) 
                : ContactEmail.Create(request.ContactEmail);

            var userDataResult = Result.FirstFailureOrSuccess(firstNameResult, lastNameResult, middleNameResult, phoneNumberResult, contactEmailResult);

            if (userDataResult.IsFailure)
            {
                return Result.Failure<UserIdResponse>(userDataResult.Error);
            }

            var identityUserIdResult = await userManagerProvider.CreateIdentityUserAsync(request.Email, request.Password);

            if (identityUserIdResult.IsFailure)
            {
                return Result.Failure<UserIdResponse>(identityUserIdResult.Error);
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

            return new UserIdResponse(user.Id);
        }
    }
}
