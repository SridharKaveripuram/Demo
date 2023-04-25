using FluentValidation;

namespace Common.Validator
{   
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {   
            RuleFor(x => x.Name).Length(50);
            RuleFor(x => x.HoursWorked).InclusiveBetween(1, 8);
            RuleFor(x => x.HourlyRate).InclusiveBetween(1, 100);
        }
    }
}
