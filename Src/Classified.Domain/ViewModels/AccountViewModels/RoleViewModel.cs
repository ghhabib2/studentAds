using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classified.Domain.Entities;

namespace Classified.Domain.ViewModels.AccountViewModels
{
    /// <summary>
    /// View Models Related to Roles
    /// </summary>
    public class RoleViewModel
    {
    }

    public class RoleRegisterViewModel
    {
        /// <summary>
        /// Name of the Role
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the Role
        /// </summary>
        public string Description { get; set; }
    }
}
