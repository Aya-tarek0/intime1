﻿namespace InTime.Features.Employee.GetById
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

}
