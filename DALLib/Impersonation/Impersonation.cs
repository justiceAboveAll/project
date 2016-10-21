using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace DALLib.Impersonation
{
   public class Impersonation
    {

        public enum SECURITY_IMPERSONATION_LEVEL : int
        {
            SecurityAnonymous = 0,
            SecurityIdentification = 1,
            SecurityImpersonation = 2,
            SecurityDelegation = 3
        }

        // obtains user token
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword,
                int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        // closes open handes returned by LogonUser
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);


        // creates duplicate token handle
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle,
            int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);

        // private System.Security.Principal.WindowsImpersonationContext newUser;


        public string getCurrentUserName()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }


        public void UndoImpersonation(System.Security.Principal.WindowsImpersonationContext user)
        {
            if (user != null) user.Undo();
        }



        public ImpersonationResult Impersonate(string userName, string domain, string password)
        {
            ImpersonationResult result = new ImpersonationResult();

            // initialize tokens
            IntPtr pExistingTokenHandle = new IntPtr(0);
            IntPtr pDuplicateTokenHandle = new IntPtr(0);
            pExistingTokenHandle = IntPtr.Zero;
            pDuplicateTokenHandle = IntPtr.Zero;

            // if domain name was blank, assume local machine
            if (domain == "")
                domain = System.Environment.MachineName;

            try
            {
                // string sResult = null;

                const int LOGON32_PROVIDER_DEFAULT = 0;

                // create token
                const int LOGON32_LOGON_INTERACTIVE = 2;


                // get handle to token
                bool bImpersonated = LogonUser(userName, domain, password,
                    LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref pExistingTokenHandle);

                // did impersonation fail?
                if (false == bImpersonated)
                {
                    int nErrorCode = Marshal.GetLastWin32Error();

                    result.ErrorString = "LogonUser() failed with error code: " + nErrorCode + "\r\n";
                    result.HasError = true;
                    return result;

                }


                // "Before impersonation: " + WindowsIdentity.GetCurrent().Name + "\r\n";

                bool bRetVal = DuplicateToken(pExistingTokenHandle, (int)SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation, ref pDuplicateTokenHandle);

                // did DuplicateToken fail?
                if (false == bRetVal)
                {
                    int nErrorCode = Marshal.GetLastWin32Error();
                    CloseHandle(pExistingTokenHandle); // close existing handle


                    result.ErrorString = "DuplicateToken() failed with error code: " + nErrorCode + "\r\n";
                    result.HasError = true;
                    return result;
                }
                else
                {
                    // create new identity using new primary token
                    WindowsIdentity newId = new WindowsIdentity(pDuplicateTokenHandle);
                    WindowsImpersonationContext impersonatedUser = newId.Impersonate();

                    // check the identity after impersonation
                    result.ErrorString = "After impersonation: " + WindowsIdentity.GetCurrent().Name + "\r\n";

                    // MessageBox.Show(this, sResult, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return result;
                }
            }
            catch (Exception ex)
            {

                result.HasError = true;
                result.ErrorString = ex.Message;

                return result;
            }
            finally
            {
                // close handle(s)
                if (pExistingTokenHandle != IntPtr.Zero) CloseHandle(pExistingTokenHandle);
                if (pDuplicateTokenHandle != IntPtr.Zero) CloseHandle(pDuplicateTokenHandle);
            }
        }

    }
}
