using SchoolManagementSystem.Data;

namespace SchoolManagementSystem.Service;

public static class Initializer
{
    public static async Task Execute() => await SchoolContext.InitializeAsync();
}