using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SchoolManagementSystem.Data.HelperClasses;

namespace SchoolManagementSystem.Data.Models.Base
{
    public abstract class BaseModel
    {
        // makes sure id is serialized first in the JSON
        [JsonPropertyOrder(-1)]
        public int Id { get; internal set; }
    }
}
