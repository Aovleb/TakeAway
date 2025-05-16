using System.ComponentModel.DataAnnotations;
using TakeAway.Models;

namespace TakeAway.Validations
{
    public class ServiceTimeAttribute : ValidationAttribute
    {

        public ServiceTimeAttribute() : base() { }
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }
            Service service = (Service)value;
            if (service.StartTime == null || service.EndTime == null)
            {
                return false;
            }
            if (service.StartTime >= service.EndTime)
            {
                return false;
            }
            return true;
        }
    }
}
