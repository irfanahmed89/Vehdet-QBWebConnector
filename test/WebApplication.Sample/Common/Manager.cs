using QbSync.QbXml.Objects;
using System.Collections.Generic;
using Newtonsoft.Json;
using WebApplication.Sample.FrappeModels;
using System.Linq;
using FrappeQbwcService.FrappeModels;
using System;
using Microsoft.SqlServer.Server;
using WebApplication.Sample.Application.Steps;
using System.Security.Cryptography;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApplication.Sample
{
    public class Manager : HttpJson
    {
        private Manager()
        {
        }
        private static Manager _Manager;
        public bool error = false;        
        public static Manager GetManager()
        {

            if (_Manager == null)
                _Manager = new Manager();
            return _Manager;

        }
        #region Users
        public QbUser? GetUser(string login, string password)
        {
            QbUser? qbUser = null;
            string endPoint = "api/resource/QB%20User?fields=[\"user_name\",\"password\"]&filters={\"user_name\":\"" + login + "\",\"password\":\"" + password + "\"}";
            string response = Request(endPoint);
            if (!string.IsNullOrEmpty(response))
            {
                APIResponse<QbUser>? apiResponse = JsonConvert.DeserializeObject<APIResponse<QbUser>>(response);
                qbUser = apiResponse?.Data.FirstOrDefault();
            }
            return qbUser;
        }

        #endregion

        #region Settings
        public QbSetting? GetSetting(string settingName)
        {
            QbSetting? qbSetting = null;
            string endPoint = "api/resource/QB%20Settings?fields=[\"name\",\"setting_name\",\"value\"]&filters={\"setting_name\":\"" + settingName + "\"}";
            string response = Request(endPoint);
            if (!string.IsNullOrEmpty(response))
            {
                APIResponse<QbSetting>? apiResponse = JsonConvert.DeserializeObject<APIResponse<QbSetting>>(response);
                qbSetting = apiResponse?.Data.FirstOrDefault();
            }
            return qbSetting;
        }

        public void SaveSetting(QbSetting qbSetting, bool isCreate)
        {
            string endPoint = "";
            string actionMethod = "PUT";
            string json = "{\"setting_name\":\"" + qbSetting.setting_name + "\",\"value\":\"" + qbSetting.value + "\"}";
            if (isCreate)
            {
                endPoint = "api/resource/QB%20Settings";
                actionMethod = "POST";
            }
            else
            {
                endPoint = string.Format("api/resource/QB%20Settings/{0}", qbSetting.name);
            }

            string response = Request(endPoint, actionMethod, json);
        }
        #endregion

        #region Ticket
        public QbTicket? GetTicket(string ticket)
        {
            QbTicket? qbTicket = null;
            string endPoint = "api/resource/QB%20Ticket?fields=[\"name\",\"ticket\",\"authenticated\",\"current_step\"]&filters={\"ticket\":\"" + ticket + "\"}";
            string response = Request(endPoint);
            if (!string.IsNullOrEmpty(response))
            {
                APIResponse<QbTicket>? apiResponse = JsonConvert.DeserializeObject<APIResponse<QbTicket>>(response);
                qbTicket = apiResponse?.Data.FirstOrDefault();
            }
            return qbTicket;
        }

        public void DeleteTicket(string ticket)
        {
            string actionMethod = "DELETE";
            string endPoint = string.Format("api/resource/QB%20Ticket/{0}", ticket);
            string response = Request(endPoint, actionMethod);

        }

        public List<QbTicket>? GetTickets(string ticket)
        {
            List<QbTicket>? qbTicket = null;
            string endPoint = "api/resource/QB%20Ticket?fields=[\"name\",\"ticket\",\"authenticated\",\"current_step\"]&filters={\"ticket\":\"" + ticket + "\"}";
            string response = Request(endPoint);
            if (!string.IsNullOrEmpty(response))
            {
                APIResponse<QbTicket>? apiResponse = JsonConvert.DeserializeObject<APIResponse<QbTicket>>(response);
                qbTicket = apiResponse?.Data.ToList();
            }
            return qbTicket;
        }

        public void SaveTicket(QbTicket qbTicket, bool isCreate)
        {
            string endPoint = "";
            string actionMethod = "PUT";
            string json = "{\"ticket\":\"" + qbTicket.ticket + "\",\"authenticated\":\"" + qbTicket.authenticated + "\",\"current_step\":\"" + qbTicket.current_step + "\"}";
            if (isCreate)
            {
                //QbTicket ticket = new QbTicket()
                //{
                //    ticket = qbTicket.ticket,
                //    authenticated = qbTicket.authenticated,
                //    current_step = qbTicket.current_step

                //};
                //json = JsonConvert.SerializeObject(ticket);

                endPoint = "api/resource/QB%20Ticket";
                actionMethod = "POST";
            }
            else
            {
                //QbTicket ticket = new QbTicket()
                //{
                //    ticket = qbTicket.ticket,
                //    authenticated = qbTicket.authenticated,
                //    current_step = qbTicket.current_step

                //};
                //json = JsonConvert.SerializeObject(ticket);

                endPoint = string.Format("api/resource/QB%20Ticket/{0}", qbTicket.name);
            }

            string response = Request(endPoint, actionMethod, json);
        }

        #endregion

        #region Kvp States

        public QbKvpState? GetKvpState(string ticket, string currentStep, string key)
        {
            QbKvpState? qbKvpState = null;
            string endPoint = "api/resource/QB%20KVP%20State?fields=[\"name\",\"ticket\",\"current_step\",\"key\",\"value\"]&filters={\"ticket\":\"" + ticket + "\", \"current_step\":\"" + currentStep + "\", \"key\":\"" + key + "\"}";
            string response = Request(endPoint);
            if (!string.IsNullOrEmpty(response))
            {
                APIResponse<QbKvpState>? apiResponse = JsonConvert.DeserializeObject<APIResponse<QbKvpState>>(response);
                qbKvpState = apiResponse?.Data.FirstOrDefault();
            }
            return qbKvpState;
        }

        public List<QbKvpState>? GetKvpStates(string ticket)
        {
            List<QbKvpState>? qbKvpStates = null;
            string endPoint = "api/resource/QB%20KVP%20State?fields=[\"name\",\"ticket\",\"current_step\",\"key\",\"value\"]&filters={\"ticket\":\"" + ticket + "\"}";
            string response = Request(endPoint);
            if (!string.IsNullOrEmpty(response))
            {
                APIResponse<QbKvpState>? apiResponse = JsonConvert.DeserializeObject<APIResponse<QbKvpState>>(response);
                qbKvpStates = apiResponse?.Data.ToList();
            }
            return qbKvpStates;
        }

        public void SaveKvpState(QbKvpState qbKvpState, bool isCreate)
        {
            string endPoint = "";
            string actionMethod = "PUT";
            string json = "{\"ticket\":\"" + qbKvpState.ticket + "\",\"current_step\":\"" + qbKvpState.current_step + "\",\"key\":\"" + qbKvpState.key + "\",\"key\":\"" + qbKvpState.value + "\"}";

            if (isCreate)
            {
                QbKvpState state = new QbKvpState()
                {
                    ticket = qbKvpState.ticket,
                    current_step = qbKvpState.current_step,
                    key = qbKvpState.key,
                    value = qbKvpState.value

                };
                json = JsonConvert.SerializeObject(state);

                endPoint = "api/resource/QB%20KVP%20State";
                actionMethod = "POST";
            }
            else
            {
                QbKvpState state = new QbKvpState()
                {
                    ticket = qbKvpState.ticket,
                    current_step = qbKvpState.current_step,
                    key = qbKvpState.key,
                    value = qbKvpState.value

                };
                json = JsonConvert.SerializeObject(state);

                endPoint = string.Format("api/resource/QB%20KVP%20State/{0}", qbKvpState.name);
            }

            string response = Request(endPoint, actionMethod, json);
        }

        public void DeleteState(string ticket)
        {
            string actionMethod = "DELETE";
            string endPoint = string.Format("api/resource/QB%20KVP%20State/{0}", ticket);
            string response = Request(endPoint, actionMethod);

        }

        #endregion

    }

}
