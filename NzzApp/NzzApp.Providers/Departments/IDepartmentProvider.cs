using System.Collections.Generic;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Contracts.Departments;

namespace NzzApp.Providers.Departments
{
    public interface IDepartmentProvider
    {
        IList<IDepartment> GetMainDepartments();
        IList<IDepartment> GetDepartmentsForArticle(IArticle article);
    }
}
