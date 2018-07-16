using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UM_TestAutomation.StepDefinitions
{
    public class UserCredentials
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ValidationErrors
    {
        public string ValidationError;
    }
}
