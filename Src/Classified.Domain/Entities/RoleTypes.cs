using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classified.Domain.Entities
{
    /// <summary>
    /// Different role types of the system
    /// </summary>
    public class RoleTypes
    {
        /// <summary>
        /// Constant value of Admin User in database.
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// Constant value of default user in database.
        /// </summary>
        public const string User = "User";

        /// <summary>
        /// Constant value of advanced user in database.
        /// </summary>
        public const string AdvancedUser = "AdvancedUser";

    }
}
