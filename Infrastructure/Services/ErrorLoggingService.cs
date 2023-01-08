using Core.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Infrastructure.Services
{
    public class ErrorLoggingService : IErrorLoggingService
    {
        public void LogError(HttpContext httpContext)
        {
            var exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = exceptionDetails?.Error;
    
            string dir = @"C:\Error Logs";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string Filepath = @"C:\Error Logs\errors.txt";
            if (!File.Exists(Filepath))
            {
                StreamWriter sw = File.CreateText(Filepath);
                sw.Dispose();
            }

            string errorMessage = WriteErrorMessage(ex);       
            File.AppendAllText(Filepath, errorMessage);
        }

        private string WriteErrorMessage(Exception ex)
        {
            StringBuilder sp = new StringBuilder();
            sp.Append('\n');
            sp.Append('\n');
            sp.Append("*******************************************************************************");
            sp.Append('\n');
            sp.Append('\n');
            sp.Append("Error: ");
            sp.Append(ex.Message);
            sp.Append('\n');
            sp.Append("Date: ");
            sp.Append(DateTime.Now);
            sp.Append("\n");
            sp.Append("\n");
            sp.Append("Error Deatils: ");
            sp.Append("\n");
            sp.Append(ex.StackTrace);
            sp.Append("\n");
            
            return sp.ToString();
            
        }
    }
}
