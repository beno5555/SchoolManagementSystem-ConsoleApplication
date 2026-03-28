using ProjectHelperLibrary.Response;

namespace SchoolManagementSystem.Service.BusinessLogic.Services;

public class BaseService
{
    protected async Task<TResponse> Execute<TResponse, TSideResponse>(Func<TResponse, Task<TSideResponse>> func) 
        where TResponse : BaseResponse, new()
        where TSideResponse : BaseResponse, new()
    {
        var mainResponse = new TResponse();
        var sideResponse = await func(mainResponse);
        if (!sideResponse.Success)
        {
            mainResponse.SetStatus(false, sideResponse.Message);
        }

        return mainResponse;
    }

    protected async Task<TResponse> Execute<TResponse>(Func<TResponse, Task> func)
        where TResponse : BaseResponse, new()
    {
        var response = new TResponse();
        await func(response);
        return response;
    }

}