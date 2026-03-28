using System.Numerics;
using ProjectHelperLibrary.Response;

namespace SchoolManagementSystem.Service.BusinessLogic.Utilities;

public class MethodHelper
{
    public async Task<TResponse> Execute<TResponse, TSideResponse>(Func<TResponse, Task<TSideResponse>> func) 
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

    public async Task<TResponse> Execute<TResponse>(Func<TResponse, Task> func)
        where TResponse : BaseResponse, new()
    {
        var response = new TResponse();
        await func(response);
        return response;
    }

    public async Task<DataResponse<List<T>>> TasksToValues<T>(IEnumerable<Task<DataResponse<T>>> tasks)
    {
        var response = new DataResponse<List<T>>();
        
        var tasksResponse = await Task.WhenAll(tasks);
                
        var values = tasksResponse
            .Where(res => res.Success)
            .Select(res => res.Value)
            .ToList();
        
        if (values.Count == tasksResponse.Length)
        {
            response.SetData(values);
        }
        else
        {
            response.SetStatus(false, "Some data was lost during the operation");
        }

        return response;
    }
}