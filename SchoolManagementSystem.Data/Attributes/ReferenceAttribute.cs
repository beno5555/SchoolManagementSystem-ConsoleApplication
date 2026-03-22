using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class ReferenceAttribute<T> : Attribute where T : BaseModel
{
    public Type TargetType { get; set; } = typeof(T);
}