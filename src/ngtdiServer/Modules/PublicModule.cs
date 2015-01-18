using System;
using System.Collections.Generic;
using Nancy;
using Newtonsoft.Json;
using NGTDI.Library.Managers;
using NGTDI.Library.Objects;
using Site.JsonObjects;
using AntiResolution = NGTDI.Library.Objects.AntiResolution;

namespace Site.Modules
{
    public class PublicModule : NancyModule
    {
        public PublicModule()
        {   
            Get["/recentfeed"] = _parameters =>
            {
                return View["recentfeed.sshtml"];
            };

            Get["/rest/recentfeed"] = _parameters =>
            {
                Response response = HandleRecentFeed(_parameters);
                response.ContentType = "application/json";
                return response;
            };

            Get["/publicantiresolution/{guid}"] = _parameters =>
            {
                string sGuid = _parameters.guid;
                Guid arGuid;

                if (Guid.TryParse(sGuid, out arGuid))
                {
                    NGTDIManager manager = new NGTDIManager();
                    AntiResolution resolution = manager.GetAntiResolution(arGuid);

                    if (resolution != null
                        && resolution.ParentGuid != null)
                    {
                        User user = (User) manager.GetUser(resolution.ParentGuid.Value);

                        if (user != null)
                        {
                            JsonObjects.PublicAntiResolution ar = new JsonObjects.PublicAntiResolution(user, 
                                resolution,
                                true);

                            return View["publicantiresolution.sshtml", ar];
                        }
                    }
                }

                return null;
            };

            Get["/rest/publicantiresolution/{guid}"] = _parameters =>
            {
                Response response = HandleGetAR(_parameters);
                response.ContentType = "application/json";
                return response;
            };
        }

        public string HandleRecentFeed(DynamicDictionary _parameters)
        {
            PublicARTableResponse response = new PublicARTableResponse();
            List<PublicAntiResolution> publicars = new List<PublicAntiResolution>();

            NGTDIManager manager = new NGTDIManager();

            List<AntiResolution> ars = manager.GetPublicAntiResolutions(20);
            foreach (AntiResolution antiResolution in ars)
            {
                if (antiResolution.IsPublic 
                    && antiResolution.ParentGuid != null)
                {
                    User user = (User)manager.GetUser(antiResolution.ParentGuid.Value);

                    PublicAntiResolution par = new PublicAntiResolution(user, antiResolution);
                    publicars.Add(par);
                }
            }

            response.Result = "Success";
            response.Data = publicars;

            return JsonConvert.SerializeObject(response);
        }

        public string HandleGetAR(DynamicDictionary _parameters)
        {
            string sGuid = _parameters["guid"];
            Guid arGuid;

            if (Guid.TryParse(sGuid, out arGuid))
            {
                NGTDIManager manager = new NGTDIManager();
                AntiResolution resolution = manager.GetAntiResolution(arGuid);

                if (resolution != null
                    && resolution.ParentGuid != null)
                {
                    User user = (User)manager.GetUser(resolution.ParentGuid.Value);

                    if (user != null)
                    {
                        JsonObjects.PublicAntiResolution ar = new JsonObjects.PublicAntiResolution(user, resolution,
                            false);

                        return JsonConvert.SerializeObject(ar);
                    }
                }
            }

            return null;
        }
    }
}