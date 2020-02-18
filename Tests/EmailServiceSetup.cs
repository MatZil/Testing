using System;
using System.Collections.Generic;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;

namespace Tests
{
    public static class EmailServiceSetup
    {
        public static Employee GetInitializedEmployee()
        {
            var employee = new Employee
            {
                ClientId = 1,
                Name = "EmployeeName1",
                Surname = "EmployeeSurname1",
                Email = "taken1@email",
                WorksFromDate = DateTime.MinValue,
                DaysOfVacation = 20,
                BirthdayDate = DateTime.MinValue,
                FreeWorkDays = 10,
                OvertimeHours = 24,
                ParentalLeaveLimit = 2,
                CurrentAvailableLeaves = 1,
                NextMonthAvailableLeaves = 2
            };

            return employee;
        }

        public static ICollection<Employee> GetInitializedAdmins()
        {
            var admins = new Employee[]
            {
                new Employee
                {
                    Name = "AdminName1",
                    Surname = "AdminSurname1",
                    Email = "admin1@email",
                    WorksFromDate = DateTime.MinValue,
                    DaysOfVacation = 20,
                    BirthdayDate = DateTime.MinValue,
                    FreeWorkDays = 10,
                    OvertimeHours = 24,
                    ParentalLeaveLimit = 2,
                    CurrentAvailableLeaves = 1,
                    NextMonthAvailableLeaves = 2
                },
                new Employee
                {
                    Name = "AdminName2",
                    Surname = "AdminSurname2",
                    Email = "admin2@email",
                    WorksFromDate = DateTime.MinValue,
                    DaysOfVacation = 20,
                    BirthdayDate = DateTime.MinValue,
                    FreeWorkDays = 10,
                    OvertimeHours = 24,
                    ParentalLeaveLimit = 2,
                    CurrentAvailableLeaves = 1,
                    NextMonthAvailableLeaves = 2
                }
            };

            return admins;
        }

        public static Holiday GetInitializedHoliday()
        {
            var holiday = new Holiday
            {
                EmployeeId = 1,
                Employee = GetInitializedEmployee(),
                Type = HolidayType.Parental,
                FromInclusive = DateTime.Today.AddDays(1),
                ToInclusive = DateTime.Today.AddDays(14),
                Status = HolidayStatus.Pending,
                RequestCreatedDate = DateTime.Today,
                Paid = false
            };

            return holiday;
        }

        public static Client GetInitializedClient()
        {
            var client = new Client
            {
                CompanyName = "CompanyName1",
                OwnerName = "OwnerName1",
                OwnerSurname = "OwnerSurname1",
                OwnerEmail = "owner@gmail.com",
                OwnerPhone = "111"
            };

            return client;
        }
    }
}
