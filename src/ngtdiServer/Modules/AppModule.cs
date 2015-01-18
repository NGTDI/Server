using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.Security;
using Newtonsoft.Json;
using NGTDI.Library.Managers;
using NGTDI.Library.Objects;
using Site.Helpers;
using Site.JsonObjects;
using TreeGecko.Library.Net.Objects;
using AntiResolution = NGTDI.Library.Objects.AntiResolution;

namespace Site.Modules
{
    public class AppModule : NancyModule
    {
        public AppModule()
        {
            this.RequiresAuthentication();

            Get["/home"] = _parameters =>
            {
                return View["home.sshtml"];
            };

            Get["/addantiresolution"] = _parameters =>
            {
                return View["addantiresolution.sshtml"];
            };

            Get["/editantiresolution/{guid}"] = _parameters =>
            {
                NGTDIManager manager = new NGTDIManager();
                User user = manager.GetUser(Context.CurrentUser.UserName);

                if (user != null)
                {
                    string sGuid = _parameters.guid;
                    Guid arGuid;

                    if (Guid.TryParse(sGuid, out arGuid))
                    {
                        AntiResolution resolution = manager.GetAntiResolution(arGuid);
                        JsonObjects.AntiResolution ar = new JsonObjects.AntiResolution(user, resolution, false);

                        return View["editantiresolution.sshtml", ar];
                    }
                }

                return View["addantiresolution.sshtml"];
            };

            Post["/addantiresolution"] = _parameters =>
            {
                Response response = HandleAddAntiResolution(_parameters);
                response.ContentType = "application/json";
                return response;
            };

            Get["/deleteantiresolution/{guid}"] = _parameters =>
            {
                Response response = HandleDeleteAntiResolution(_parameters);
                response.ContentType = "application/json";
                return response;
            };

            Get["/myantiresolutions"] = _parameters =>
            {
                return View["myantiresolutions.sshtml"];
            };

            Get["/changepassword"] = _parameters =>
            {
                return View["changepassword.sshtml"];
            };

            Post["/changepassword"] = _parameters =>
            {
                Response response = HandleChangePassword(_parameters);
                response.ContentType = "application/json";
                return response;
            };

            Get["/getantiresolutions"] = _parameters =>
            {
                Response response = HandleGetAntiResolutions(_parameters);
                response.ContentType = "application/json";
                return response;
            };

            Post["/signeula/{guid}"] = _parameters =>
            {
                Response response = HandleSignEula(_parameters);
                response.ContentType = "application/json";
                return response;
            };

            Get["/suggestion"] = _parameters =>
            {
                return View["suggestion.sshtml"];
            };

            Post["/suggestion"] = _parameters =>
            {
                NGTDIManager manager = new NGTDIManager();
                User user = manager.GetUser(Context.CurrentUser.UserName);

                if (user != null)
                {
                    string suggestionText = Request.Form["txtSuggestion"];

                    if (suggestionText != null)
                    {
                        Suggestion suggestion = new Suggestion
                        {
                            Active = true,
                            SubmittedByUserGuid = user.Guid,
                            Text = suggestionText,
                            LastModifiedBy = user.Guid,
                            LastModifiedDateTime = DateTime.UtcNow
                        };

                        manager.Persist(suggestion);
                    }
                }

                return View["suggestionthankyou.sshtml"];
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_parameters"></param>
        /// <returns></returns>
        private string HandleSignEula(DynamicDictionary _parameters)
        {
            User user;
            BasicResponse response = new BasicResponse { Result = "Failure" }; 

            if (AuthHelper.IsAuthorized(Request, out user))
            {
                if (user != null)
                {
                    Guid eulaGuid;

                    if (Guid.TryParse(_parameters["guid"], out eulaGuid))
                    {
                        NGTDIManager manager = new NGTDIManager();
                        TGEula eula = manager.GetEula(eulaGuid);

                        if (eula != null)
                        {
                            TGEulaAgreement agreement = manager.GetEulaAgreement(user.Guid, eulaGuid);

                            if (agreement == null)
                            {
                                agreement = new TGEulaAgreement
                                {
                                    Active = true,
                                    AgreementDateTime = DateTime.UtcNow,
                                    EulaGuid = eulaGuid,
                                    UserGuid = user.Guid
                                };

                                manager.Persist(agreement);


                            }
                            user.EulaAccepted = true;
                            manager.Persist(user);

                            response.Result = "Success";
                        }
                    }
                }
            }

            return JsonConvert.SerializeObject(response);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_parameters"></param>
        /// <returns></returns>
        private string HandleDeleteAntiResolution(DynamicDictionary _parameters)
        {
            User user;
            BasicResponse response = new BasicResponse {Result = "Failure"};
            
            if (AuthHelper.IsAuthorized(Request, out user))
            {
                if (user != null)
                {
                    NGTDIManager manager = new NGTDIManager();

                    string sGuid = _parameters["guid"];
                    Guid guid;

                    if (Guid.TryParse(sGuid, out guid))
                    {
                        AntiResolution ar = manager.GetAntiResolution(guid);

                        if (ar != null)
                        {
                            manager.Delete(ar);
                            response.Result = "Success";
                        }
                    }
                }
            }

            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_parameters"></param>
        /// <returns></returns>
        private string HandleGetAntiResolutions(DynamicDictionary _parameters)
        {
            User user;
            ARTableResponse response = null;
            
            if (AuthHelper.IsAuthorized(Request, out user))
            {
                if (user != null)
                {
                    NGTDIManager manager = new NGTDIManager();
                    List<AntiResolution> ars = manager.GetAntiResolutions(user.Guid);
                    response = new ARTableResponse(true, user, ars);
                }

                if (response == null)
                {
                    response = new ARTableResponse(false, null, null);
                }
            }

            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_parameters"></param>
        /// <returns></returns>
        private string HandleAddAntiResolution(DynamicDictionary _parameters)
        {
            User user;

            if (AuthHelper.IsAuthorized(Request, out user))
            {
                if (user != null)
                {
                    NGTDIManager manager = new NGTDIManager();

                    string sGuid = Request.Form["Guid"];
                    string sType = Request.Form["Type"];
                    string sStart = Request.Form["Start"];
                    string sEnd = Request.Form["End"];
                    string sAntiResolution = Request.Form["AntiResolution"];
                    string sIsPublic = Request.Form["IsPublic"];

                    DateTime? startDate = null;
                    if (!string.IsNullOrEmpty(sStart))
                    {
                        startDate = DateTime.Parse(sStart);
                    }

                    DateTime? endDate = null;
                    if (!string.IsNullOrEmpty(sEnd))
                    {
                        endDate = DateTime.Parse(sEnd);
                    }

                    bool isPublic = false;
                    if (sIsPublic != null
                        && sIsPublic.Equals("True", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isPublic = true;
                    }

                    AntiResolution ar = null;
                    if (!string.IsNullOrEmpty(sGuid))
                    {
                        Guid guid;

                        if (Guid.TryParse(sGuid, out guid))
                        {
                            ar = manager.GetAntiResolution(guid);
                        }
                    }

                    if (ar == null)
                    {
                        ar = new AntiResolution();
                    }

                    ar.Active = true;
                    ar.EndDate = endDate;
                    ar.Period = sType;
                    ar.StartDate = startDate;
                    ar.SetAntiResolutionText(user, sAntiResolution);
                    ar.ParentGuid = user.Guid;
                    ar.IsPublic = isPublic;

                    manager.Persist(ar);
                }

                return "{ \"Result\":\"Success\" }";
            }

            return "{ \"Result\":\"Failure\" }";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_parameters"></param>
        /// <returns></returns>
        private string HandleChangePassword(DynamicDictionary _parameters)
        {
            string currentPassword = Request.Form["currentpassword"];
            string newPassword = Request.Form["newpassword"];
            string username = Request.Headers["Username"].First();

            User user;

            if (AuthHelper.IsAuthorized(Request, out user))
            {
                if (user != null)
                {
                    TGUserPassword up = TGUserPassword.GetNew(user.Guid, user.Username, newPassword);

                    NGTDIManager manager = new NGTDIManager();
                    manager.Persist(up);

                    return @"{ ""Result"":""Success"" }";
                    ;
                }
            }

            return @"{ ""Result"":""Failure"" }";
        }
    }
}