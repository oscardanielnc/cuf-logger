using cuf_admision_domain.Models;
using cuf_admision_domain.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuf_admision_data.Utils
{
    public class LogTools: IUtilsService
    {
        public bool LogSimple(LogModel body)
        {
            DateTime fechaHoraActual = DateTime.Now; 
            string formatoFechaHora = fechaHoraActual.ToString("dd/MM/yyyy HH:mm:ss");
            Console.WriteLine($"{formatoFechaHora}    {body.text}");
            return true;
        }
    }
}
