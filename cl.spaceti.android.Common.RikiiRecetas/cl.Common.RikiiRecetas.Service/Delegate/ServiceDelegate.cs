using System;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using cl.Common.RikiiRecetas.Service.Result;
using cl.Common.RikiiRecetas.Service.Properties;
using cl.Common.RikiiRecetas.Utils;
using cl.Common.RikiiRecetas.Models.Modelo;
using System.IO;
using Android.App;

namespace cl.Common.RikiiRecetas.Service.Delegate
{
    /// <summary>
    /// Service delegate.
    /// </summary>
    public class ServiceDelegate
    {
        static ServiceDelegate instance = null;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ServiceDelegate Instance
        {
            get
            {
                if (instance == null)
                    instance = new ServiceDelegate();
                return instance;
            }
        }

        #region WEB_API_MAPAS
        /// <summary>
        /// Gets the locales.
        /// </summary>
        /// <returns>The locales.</returns>
        public async Task<ServiceResult> GetPaises(string json)
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {                   
                    string responseString = json;
                    if (!string.IsNullOrEmpty(responseString))
                    {
                        List<Pais> paises = JsonConvert.DeserializeObject<List<Pais>>(responseString);
                        result.Success = true;
                        result.Response = paises;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "";
                        result.Response = 999;
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }
            else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }
        #endregion
        public async Task<ServiceResult> GetRecetas(string json)
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {
                    string responseString = json;
                    if (!string.IsNullOrEmpty(responseString))
                    {
                        List<Receta> receta = JsonConvert.DeserializeObject<List<Receta>>(responseString);
                        result.Success = true;
                        result.Response = receta;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "";
                        result.Response = 999;
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }
            else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }
       

        /// <summary>
        /// Gets the network status.
        /// </summary>
        /// <returns><c>true</c>, if network status was gotten, <c>false</c> otherwise.</returns>
        private bool GetNetworkStatus()
        {
            return ValidationUtils.GetNetworkStatus();
        }

    }
}
