using Microsoft.AspNetCore.Identity;

namespace Snackis_Attempt_1.Methods
{
	public class AdminMethods
	{
		public RoleManager<IdentityRole> _roleManager { get; set; }
        public AdminMethods(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task CreateRole(string roleName)
		{
			bool roleExist = await _roleManager.RoleExistsAsync(roleName);

			if (!roleExist)
			{
				var role = new IdentityRole
				{
					Name = roleName,
				};

				await _roleManager.CreateAsync(role);
				//UpdateAsync(role) för att ändra/uppdatera en befintlig post

			}
		}
	}
}
