using System;
using System.Collections.Generic;

namespace NzzApp.Model.Contracts.Departments
{
    public interface IDepartment : IEquatable<IDepartment>
    {
        string Name { get; set; }
        string Path { get; }
        DepartmentSerialisationType DepartmentSerialisationType { get; set; }
        IList<IDepartment> SubDepartments { get; } 
        string ParentDepartmentPath { get; set; }
        bool IsStartPage { get; }
        string ShowOn { get; set; }
        bool ShowAlways { get; }
    }
}