using SchoolManagementSystem.Data.Models;

namespace SchoolManagementSystem.Data.HelperClasses;

public static class IdHelper
{
    public static int GetMaxId<T>(List<T> collection) where T : BaseModel
    {
        int maxId = collection.Count > 0
            ? collection
                .Select(item => item.Id)
                .Max()
            : 0;
        return maxId;
    }
}