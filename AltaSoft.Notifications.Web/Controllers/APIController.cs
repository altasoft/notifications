using AltaSoft.Notifications.DAL;
using AltaSoft.Notifications.Web.Common;
using AltaSoft.Notifications.Web.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AltaSoft.Notifications.Web.Controllers
{
    public class APIController : ApiController
    {
        [HttpPost]
        public APIResult<SendResultModel> Send(SendModel model)
        {
            var result = new SendResultModel();

            try
            {
                result = SendLogic(model);
            }
            catch (Exception ex)
            {
                return new APIResult<SendResultModel>(ex.Message, ex.ToString());
            }

            return new APIResult<SendResultModel>(result);
        }

        [HttpPost]
        public APIResult SaveUser(SaveUserModel model)
        {
            try
            {
                if (model == null)
                    throw new Exception("Please pass model");

                using (var bo = new ApplicationBusinessObject())
                {
                    if (!bo.Check(model.ApplicationId, model.ApplicationSecretKey))
                        throw new Exception("Invalid application credentials");
                }


                var user = new User
                {
                    ApplicationId = model.ApplicationId,
                    ExternalUserId = model.ExternalUserId,
                    FirstName = model.FirstName,
                    FullName = model.FullName,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber
                };

                SaveUserLogic(user);
            }
            catch (Exception ex)
            {
                return new APIResult(ex.Message, ex.ToString());
            }

            return new APIResult();
        }

        [HttpPost]
        public APIResult SaveUsers(SaveUsersModel model)
        {
            try
            {
                if (model == null)
                    throw new Exception("Please pass model");

                using (var bo = new ApplicationBusinessObject())
                {
                    if (!bo.Check(model.ApplicationId, model.ApplicationSecretKey))
                        throw new Exception("Invalid application credentials");
                }


                foreach (var item in model.Users)
                {
                    var user = new User
                    {
                        ApplicationId = model.ApplicationId,
                        ExternalUserId = item.ExternalUserId,
                        FirstName = item.FirstName,
                        FullName = item.FullName,
                        Email = item.Email,
                        MobileNumber = item.MobileNumber
                    };

                    SaveUserLogic(user);
                }


            }
            catch (Exception ex)
            {
                return new APIResult(ex.Message, ex.ToString());
            }

            return new APIResult();
        }

        [HttpGet]
        public APIResult<List<SubscriptionResult>> Subscriptions(GetSubscriptionsModel model, [FromUri]GetSubscriptionsModel uriModel)
        {
            try
            {
                if (model == null)
                    model = uriModel;

                if (model == null)
                    throw new Exception("Please pass model");

                using (var bo = new ApplicationBusinessObject())
                {
                    if (!bo.Check(model.ApplicationId, model.ApplicationSecretKey))
                        throw new Exception("Invalid application credentials");
                }

                var eventKeys = new List<string>();
                var providerKeys = new List<string>();
                var userKeys = new List<string>();

                if (!String.IsNullOrEmpty(model.EventKeys))
                    eventKeys = model.EventKeys.Split(',').ToList();

                if (!String.IsNullOrEmpty(model.ProviderKeys))
                    providerKeys = model.ProviderKeys.Split(',').ToList();

                if (!String.IsNullOrEmpty(model.ExternalUserIds))
                    userKeys = model.ExternalUserIds.Split(',').ToList();


                //using (var bo = new SubscriptionBusinessObject())
                //{
                //    var items = bo.GetList(model.ApplicationId, eventKeys, providerKeys, userKeys);

                //    var result = items.Select(x => new SubscriptionResult
                //    {
                //        EventKey = x.Event.Key,
                //        EventDescription = x.Event.Description,
                //        ProviderKey = x.Provider.Key,
                //        ExternalUserId = x.User.ExternalUserId
                //    }).ToList();

                //    return new APIResult<List<SubscriptionResult>>(result);
                //}

                return null;

            }
            catch (Exception ex)
            {
                return new APIResult<List<SubscriptionResult>>(ex.Message, ex.ToString());
            }
        }

        [HttpGet]
        public APIResult<List<MessageResult>> Messages(GetMessagesModel model, [FromUri]GetMessagesModel uriModel)
        {
            try
            {
                if (model == null)
                    model = uriModel;

                if (model == null)
                    throw new Exception("Please pass model");

                using (var bo = new ApplicationBusinessObject())
                {
                    if (!bo.Check(model.ApplicationId, model.ApplicationSecretKey))
                        throw new Exception("Invalid application credentials");
                }

                if (String.IsNullOrEmpty(model.Ids))
                    throw new Exception("Please pass Ids parameter");

                var ids = model.Ids.Split(',')
                    .Select(x =>
                    {
                        int i;
                        return Int32.TryParse(x, out i) ? (int?)i : null;
                    })
                    .Where(x => x.HasValue)
                    .ToList();


                if (ids.Count == 0)
                    return new APIResult<List<MessageResult>>(new List<MessageResult>());


                using (var bo = new MessageBusinessObject())
                {
                    var result = bo.GetListWithUserAndProvider(x => ids.Contains(x.Id)).ConvertAll(x =>
                    {
                        return new MessageResult
                        {
                            Content = x.Content,
                            ErrorDetails = x.ErrorDetails,
                            ErrorMessage = x.ErrorMessage,
                            ExternalUserId = x.User.ExternalUserId,
                            Priority = x.Priority,
                            ProcessDate = x.ProcessDate,
                            ProviderKey = x.Provider.Key,
                            RetryCount = x.RetryCount,
                            State = x.State,
                            Subject = x.Subject,
                            To = x.To
                        };
                    });

                    return new APIResult<List<MessageResult>>(result);
                }
            }
            catch (Exception ex)
            {
                return new APIResult<List<MessageResult>>(ex.Message, ex.ToString());
            }
        }



        static void SaveUserLogic(User user)
        {
            if (!String.IsNullOrEmpty(user.MobileNumber))
            {
                if (user.MobileNumber.Length == 9)
                    user.MobileNumber = "995" + user.MobileNumber;
            }

            using (var bo = new UserBusinessObject())
            {
                bo.Save(user);
            }
        }

        static List<Tuple<int?, string>> GetUserInfos(int applicationId, List<int> externalUserIds, int providerId)
        {
            var result = new List<Tuple<int?, string>>();

            foreach (var item in externalUserIds)
            {
                using (var bo = new UserBusinessObject())
                {
                    var user = bo.GetByExternalUserId(applicationId, item);
                    if (user == null)
                        continue;

                    var to = Helper.GetToByProvider(user, providerId);
                    if (String.IsNullOrEmpty(to))
                        continue;

                    result.Add(new Tuple<int?, string>(user.Id, to));
                }
            }

            return result;
        }

        internal static SendResultModel SendLogic(SendModel model, int? providerId = null, int? applicationId = null)
        {
            var result = new SendResultModel();
            result.SuccessMessageIds = new List<int>();
            result.FailedMessages = new List<string>();

            if (model == null)
                throw new Exception("Please pass model");


            #region Application
            Application application;
            using (var bo = new ApplicationBusinessObject())
            {
                if (applicationId.HasValue)
                    application = bo.GetById(applicationId);
                else
                    application = bo.GetById(model.ApplicationId);

                if (!applicationId.HasValue)
                {
                    if (application == null || application.SecretKey != model.ApplicationSecretKey)
                        throw new Exception("Invalid application credentials");
                }
                else
                {
                    if (application == null)
                        throw new Exception("Invalid application");
                }
            }
            #endregion

            #region Application Product
            TimeSpan? SleepFromTime = null, SleepToTime = null;
            int? applicationProductId = null;
            if (!String.IsNullOrEmpty(model.ApplicationProductKey))
            {
                using (var bo = new ApplicationProductBusinessObject())
                {
                    var product = bo.GetByKey(model.ApplicationProductKey);
                    if (product == null)
                        throw new Exception("Invalid Product provided");

                    if (!product.IsActive)
                        throw new Exception("Product is not Active");

                    applicationProductId = product.Id;
                    SleepFromTime = product.SleepFromTime;
                    SleepToTime = product.SleepToTime;
                    model.Priority = product.Priority;
                }
            }

            #endregion



            if (String.IsNullOrEmpty(model.Content))
                throw new Exception("Content can't be empty");


            #region Provider
            if (!providerId.HasValue)
            {
                Provider provider;
                using (var bo = new ProviderBusinessObject())
                {
                    provider = bo.GetByKey(model.ProviderKey);
                    if (provider == null)
                        throw new Exception("Provider not found");
                }
                providerId = provider.Id;
            }
            #endregion


            if (model.ExternalUserIds == null)
                model.ExternalUserIds = new List<int>();

            if (model.ExternalUserId.HasValue)
                model.ExternalUserIds.Add(model.ExternalUserId.Value);

            var userInfos = GetUserInfos(application.Id, model.ExternalUserIds, providerId.Value);

            if (!String.IsNullOrEmpty(model.To))
            {
                model.To.Split(',').ToList().ForEach(x => userInfos.Add(new Tuple<int?, string>(null, x)));
            }



            #region Group
            if (!String.IsNullOrEmpty(model.GroupKey))
            {
                using (var bo = new GroupBusinessObject())
                {
                    var groupIds = bo.GetGroupIdsByKey(application.Id, model.GroupKey);
                    var items = bo.GetUsersByGroups(groupIds.ToArray());

                    userInfos.AddRange(items.ConvertAll(x => new Tuple<int?, string>(x.Id, Helper.GetToByProvider(x, providerId.Value))));
                }
            }
            #endregion


            //if (userInfos.Count == 0)
            //    throw new Exception("No Users found to send. Please set: ExternalUserId, ExternalUserIds, To, or EventKey");


            var groupId = Guid.NewGuid();

            // თუ SMS-ების გაგზავნაა
            if (providerId == 2)
            {
                result = SendSMSItems(userInfos.Select(x => x.Item2), providerId.Value, applicationProductId, groupId, application, SleepFromTime, SleepToTime, model.Content, model.ProcessDate, model.Priority, model.IsTest);
            }
            else
            {
                foreach (var info in userInfos)
                {
                    var message = new Message
                    {
                        UserId = info.Item1,
                        To = info.Item2,
                        ProviderId = providerId.Value,
                        ApplicationId = application.Id,
                        Subject = model.Subject,
                        Content = model.Content,
                        ProcessDate = model.ProcessDate,
                        GroupId = groupId,
                        Priority = (MessagePriority)model.Priority,
                        IsTest = model.IsTest
                    };

                    using (var bo = new MessageBusinessObject())
                    {
                        var id = bo.Create(message);
                        result.SuccessMessageIds.Add(id);
                    }
                }
            }

            return result;
        }



        static SendResultModel SendSMSItems(IEnumerable<string> mobileNumbers, int providerId, int? applicationProductId, Guid groupId, Application application, TimeSpan? SleepFromTime, TimeSpan? SleepToTime, string message, DateTime? processDate, int priority, bool isTest)
        {

            var result = new SendResultModel();
            result.SuccessMessageIds = new List<int>();
            result.FailedMessages = new List<string>();

            mobileNumbers = mobileNumbers.Distinct();

            using (var bo = new SMSBusinessObject())
            {
                foreach (var item in mobileNumbers)
                {
                    try
                    {
                        var to = item.Trim(' ', '+');

                        if (to.Length == 9)
                            to = "995" + to;

                        if (to.Length != 12)
                            throw new Exception("Invalid phone number");



                        var id = bo.Create(new SMS
                        {
                            To = to,
                            ProviderId = providerId,
                            GroupId = groupId,
                            SenderNumber = application.SMSSenderNumber,
                            ApplicationId = application.Id,
                            ApplicationProductId = applicationProductId,
                            SleepFromTime = SleepFromTime,
                            SleepToTime = SleepToTime,
                            Message = message,
                            ProcessDate = processDate,
                            Priority = priority,
                            IsTest = application.IsTestMode || isTest
                        });

                        result.SuccessMessageIds.Add(id);
                    }
                    catch (Exception ex)
                    {
                        result.FailedMessages.Add(String.Format("[{0}] {1}", item, ex.Message));
                    }
                }

                return result;
            }
        }
    }
}
