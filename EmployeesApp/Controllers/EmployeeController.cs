using EmployeesApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesApp.Controllers
{
   public enum SortDirection
    {
        Ascending, Descending
    }
    public class EmployeeController : Controller
    {
        HRDataBaseContext dbContext = new HRDataBaseContext();
        public IActionResult Index(string sortField, string currentSortField, SortDirection sortDirection)
        {
            // List<Employee> employees= dbContext.Employees.ToList();
            var employees = GetEmployees();
            return View(this.SortEmployees(employees,sortField,currentSortField,sortDirection));
        }

        private List<Employee> GetEmployees()
        {
            var employees = (from employee in dbContext.Employees
                             join department in dbContext.Departments on employee.DepartmentID equals department.DepartmentID
                             select new Employee
                             {
                                 EmployeeID = employee.EmployeeID,
                                 EmployeeName = employee.EmployeeName,
                                 DepartmentID = employee.DepartmentID,
                                 EmployeeNumber = employee.EmployeeNumber,
                                 DOB = employee.DOB,
                                 GrossSalary = employee.GrossSalary,
                                 NetSalary = employee.NetSalary,
                                 HiringDate = employee.HiringDate,
                                 DepartmentName = department.DepartmentName
                             }).ToList();
            return employees;
        }

        public IActionResult Create()
        {
            ViewBag.Departments = this.dbContext.Departments.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee model) {
            ModelState.Remove("EmployeeID");
            ModelState.Remove("Department");
            ModelState.Remove("DepartmentName");
            if (ModelState.IsValid)
            {
                dbContext.Employees.Add(model);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Departments = this.dbContext.Departments.ToList();
            return View();
        }

        public IActionResult Edit(int ID)
        {
            Employee data = this.dbContext.Employees.Where(e=>e.EmployeeID == ID).FirstOrDefault();
            ViewBag.Departments = this.dbContext.Departments.ToList();
            return View("Create",data);
        }

        [HttpPost]
        public IActionResult Edit(Employee model)
        {
            ModelState.Remove("EmployeeID");
            ModelState.Remove("Department");
            ModelState.Remove("DepartmentName");
            if (ModelState.IsValid)
            {
                dbContext.Employees.Update(model);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Departments = this.dbContext.Departments.ToList();
            return View("Create",model);
        }

        public IActionResult Delete(int ID)
        {
            Employee data = this.dbContext.Employees.Where(e => e.EmployeeID == ID).FirstOrDefault();
            if(data != null)
            {
                dbContext.Remove(data);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        private List<Employee> SortEmployees(List<Employee> employees,string sortField, string currentSortField, SortDirection sortDirection)
        {
            //if (string.IsNullOrEmpty(sortField))
            //{
            //    ViewBag.SortField = "EmployeeNumber";
            //    ViewBag.SortDirection = SortDirction.Ascnding;
            //}
            //else
            //{
            //    if (currentSortField == sortField)
            //        ViewBag.SortDirection = sortDirection == SortDirction.Ascnding ? SortDirction.Descending : SortDirction.Ascnding;
            //    else
            //        ViewBag.SortDirection = SortDirction.Ascnding;
            //    ViewBag.SortField = sortField;
            //}

            //var propertyInfo = typeof(Employee).GetProperty(ViewBag.SortField); 
            //if (ViewBag.SortDirction == SortDirction.Ascnding)
            //    employees = employees.OrderBy(e => propertyInfo.GetValue(e,null)).ToList();
            //else
            //    employees = employees.OrderByDescending(e => propertyInfo.GetValue(e, null)).ToList();
            //return employees;
            if (string.IsNullOrEmpty(sortField))
            {
                ViewBag.SortField = "EmployeeNumber";
                ViewBag.SortDirection = SortDirection.Ascending;
            }
            else
            {
                if (currentSortField == sortField)
                    ViewBag.SortDirection = sortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                else
                    ViewBag.SortDirection = SortDirection.Ascending;
                ViewBag.SortField = sortField;
            }

            if (ViewBag.SortDirection == SortDirection.Ascending)
            {
                switch (ViewBag.SortField)
                {
                    case "EmployeeNumber":
                        employees = employees.OrderBy(e => e.EmployeeNumber).ToList();
                        break;
                    case "EmployeeName":
                        employees = employees.OrderBy(e => e.EmployeeName).ToList();
                        break;
                        // Add cases for other fields as needed
                }
            }
            else
            {
                switch (ViewBag.SortField)
                {
                    case "EmployeeNumber":
                        employees = employees.OrderByDescending(e => e.EmployeeNumber).ToList();
                        break;
                    case "EmployeeName":
                        employees = employees.OrderByDescending(e => e.EmployeeName).ToList();
                        break;
                        // Add cases for other fields as needed
                }
            }

            return employees;
        }
    }
}
