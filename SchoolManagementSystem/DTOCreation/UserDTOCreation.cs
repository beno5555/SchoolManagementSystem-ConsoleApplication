using SchoolManagementSystem.ConsoleDisplay;
using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Service.DTOs.User.Auth;

namespace SchoolManagementSystem.DTOCreation;

public static class UserDTOCreation
{
    public static BaseRegisterDTO PromptBaseRegisterDTO()
    {
        return new StudentRegisterDTO
        {
            FirstName = LayoutHelper.GetInput("First Name"),
            LastName = LayoutHelper.GetInput("Last Name"),
            PhoneNumber = LayoutHelper.GetInput("Phone Number"),
            Address = LayoutHelper.GetInput("Address"),
            PrivateId = LayoutHelper.GetInput("National Id number"),
            Email = LayoutHelper.GetInput("Email"),
            DateOfBirth = LayoutHelper.GetDateInput("Birth Date"),
            Password = LayoutHelper.GetInput("Password", secret: true)
        };
    }
    
    /// <summary>
    /// admin registering another user
    /// </summary>
    public static AdminRegisterDTO PromptAdminRegistration(int roleId)
    {
        var baseRegisterDTO = PromptBaseRegisterDTO();
        return PromptRegistration<AdminRegisterDTO>(baseRegisterDTO, registerDTO => registerDTO.RoleId = roleId);
    }

    public static AdminRegisterDTO PromptAdminRegisterDTO(BaseRegisterDTO baseRegisterDTO, int roleId)
    {
        return PromptRegistration<AdminRegisterDTO>(
            baseRegisterDTO, 
            adminRegisterDTO => adminRegisterDTO.RoleId = roleId);
    }

    public static AdminRegisterDTO PromptPrincipalRegistration(BaseRegisterDTO baseRegisterDTO, int roleId)
    {
        return PromptRegistration<AdminRegisterDTO>(
            baseRegisterDTO,
            principalRegisterDTO =>
            {
                principalRegisterDTO.RoleId = roleId;
                if (int.TryParse(LayoutHelper.GetInput("Office Room Id:"), out int officeRoomId))
                {
                    principalRegisterDTO.OfficeRoomId = officeRoomId;
                }

            });
    }

    public static AdminRegisterDTO PromptTeacherRegistration(BaseRegisterDTO baseRegisterDTO, int roleId)
    {
        return PromptRegistration<AdminRegisterDTO>(baseRegisterDTO,
            teacherRegisterDTO => teacherRegisterDTO.RoleId = roleId);
    }

    private static TRegistrationDTO PromptRegistration<TRegistrationDTO>(BaseRegisterDTO baseRegisterDTO, Action<TRegistrationDTO>? applySpecifics)
        where TRegistrationDTO : BaseRegisterDTO, new()
    {
        var registerDTO =  new TRegistrationDTO
        {
            FirstName = baseRegisterDTO.FirstName,
            LastName = baseRegisterDTO.LastName,
            PhoneNumber = baseRegisterDTO.PhoneNumber,
            Address = baseRegisterDTO.Address,
            PrivateId = baseRegisterDTO.PrivateId,
            Email = baseRegisterDTO.Email,
            DateOfBirth = baseRegisterDTO.DateOfBirth,
            Password = baseRegisterDTO.Password,
        };
        applySpecifics?.Invoke(registerDTO);
        return registerDTO;
    }
}