using System;
using System.Collections.Generic;
using System.Linq;
using NzzApp.Data;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Contracts.Departments;
using NzzApp.Providers.Helpers;
using NzzApp.Providers.Synchonisation;

namespace NzzApp.Providers.Departments
{
    public class DepartmentProvider : IDepartmentProvider
    {
        private readonly IDataProvider _dataProvider;
        private readonly ISyncProvider _syncProvider;

        private IList<IDepartment> _departments = new List<IDepartment>();
        private IList<IDepartment> _shiftedDepartments = new List<IDepartment>(); 

        public DepartmentProvider(IDataProvider dataProvider, ISyncProvider syncProvider)
        {
            _dataProvider = dataProvider;
            _syncProvider = syncProvider;

            _syncProvider.FetchDepartmentsCompleted += SyncProviderOnFetchDepartmentsCompleted;

            _departments = _dataProvider.GetDepartments().ToList();
        }

        private void SyncProviderOnFetchDepartmentsCompleted(object sender, TaskResult taskResult)
        {
            _departments = _dataProvider.GetDepartments().ToList();
        }

        public IList<IDepartment> GetMainDepartments()
        {
            if (_shiftedDepartments.Count < 1)
            {
                _shiftedDepartments = ShiftSubDepartments(_departments.ToList());
            }
            return _shiftedDepartments;
        }

        public IList<IDepartment> GetDepartmentsForArticle(IArticle article)
        {
            return _dataProvider.GetDepartments(article);
        }

        private IList<IDepartment> ShiftSubDepartments(IList<IDepartment> departments)
        {
            foreach (var department in departments.ToList())
            {
                if (!string.IsNullOrWhiteSpace(department.ParentDepartmentPath))
                {
                    var parentDepartment = departments.FirstOrDefault(d => d.Path == department.ParentDepartmentPath);
                    parentDepartment?.SubDepartments.Add(department);
                    departments.Remove(department);
                }
            }
            return departments;
        }
    }
}
