using ProjectHelperLibrary.Response;

namespace SchoolManagementSystem.Data.HelperClasses;

public static class RepoHelper
{
    internal static async Task<BaseResponse> HandleIndex<T>(int index, List<T> collection, Action action)
    {
        var response = new BaseResponse();
        if (index == -1)
        {
            response.SetStatus(false, $"{typeof(T).Name} not found");
        }
        else
        {
            action();
            await collection.SaveAsync();
        }

        return response;
    }
}