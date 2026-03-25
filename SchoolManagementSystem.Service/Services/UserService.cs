using SchoolManagementSystem.Data.Repositories;

namespace SchoolManagementSystem.Service.Services;

public class UserService
{
    private readonly UserRepository _userRepository = new();
    private readonly SubjectRepository _subjectRepository = new();
}