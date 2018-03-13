using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants.Interface;

namespace Constants
{
    public class MenuLayoutVisibilityConstant
    {
    private static IEmployee _loginAccount = null;

        public _Demo Demo = new _Demo();

        #region Modules

        public class _Demo
        {
            public bool Any = false;

            public bool Get = false;
            public bool Insert = false;
            public bool Update = false;
            public bool Delete = false;
        }
        #endregion


        public void CheckMenuVisibilityState(IEmployee loginAccount)
        {
            if (loginAccount?.IdEmployee == null)
            {
                return;
            }
            _loginAccount = loginAccount;

            #region ModuleVisibilityState

            Demo.Any = CheckModuleVisibility<PermissionConstants.Demo>();

            #endregion

            #region ModuleActionVisibilityState

            Demo.Get = CheckModuleActionVisibility(PermissionConstants.Demo.Get);
            Demo.Insert = CheckModuleActionVisibility(PermissionConstants.Demo.Insert);
            Demo.Update = CheckModuleActionVisibility(PermissionConstants.Demo.Update);
            Demo.Delete = CheckModuleActionVisibility(PermissionConstants.Demo.Delete);
            
            #endregion

        }

        #region Function

        private bool CheckModuleVisibility<T>()
        {
            if (_loginAccount != null)
            {
                var temp = typeof(T).GetAllPublicConstantValues<string>();
                var permissionList = temp.Select(int.Parse).ToList();

                var accPermissions = _loginAccount.Permissions;
                if (accPermissions.Any(a => permissionList.Any(p => p == a)))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckModuleActionVisibility(string permissionStr)
        {
            if (_loginAccount != null)
            {
                int.TryParse(permissionStr, out var permission);

                var accPermissions = _loginAccount.Permissions;
                if (accPermissions.Any(a => a == permission))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
