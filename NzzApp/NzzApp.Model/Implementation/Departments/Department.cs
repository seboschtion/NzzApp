using System;
using System.Collections.Generic;
using NzzApp.Model.Contracts.Departments;
using Sebastian.Toolkit.Util;

namespace NzzApp.Model.Implementation.Departments
{
    public class Department : PropertyChangedBase, IDepartment
    {
        private string _name;
        private DepartmentSerialisationType _departmentSerialisationType;
        private string _parentDepartmentPath;
        private IList<DayOfWeek> _showOn = new List<DayOfWeek>();

        public Department(string path)
        {
            Path = path;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Path { get; }

        public DepartmentSerialisationType DepartmentSerialisationType
        {
            get { return _departmentSerialisationType; }
            set
            {
                _departmentSerialisationType = value;
                OnPropertyChanged();
            }
        }

        public IList<IDepartment> SubDepartments { get; } = new List<IDepartment>();

        public string ParentDepartmentPath
        {
            get { return _parentDepartmentPath; }
            set
            {
                _parentDepartmentPath = value;
                OnPropertyChanged();
            }
        }

        public bool IsStartPage => Name.Equals("Startseite");

        public bool ShowAlways => string.IsNullOrWhiteSpace(ShowOn);

        public string ShowOn { get; set; }

        public bool Equals(IDepartment other)
        {
            return Path.Equals(other.Path);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
