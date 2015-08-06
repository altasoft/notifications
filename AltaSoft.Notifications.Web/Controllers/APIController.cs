using AltaSoft.Notifications.DAL;
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
        public APIResult<List<int>> Send(SendModel model)
        {
            var messageIds = new List<int>();

            try
            {
                if (model == null)
                    throw new Exception("Please pass model");

                using (var bo = new ApplicationBusinessObject())
                {
                    if (!bo.Check(model.ApplicationId, model.ApplicationSecretKey))
                        throw new Exception("Invalid application credentials");
                }

                if (String.IsNullOrEmpty(model.Content))
                    throw new Exception("Content can't be empty");


                Provider provider;
                using (var bo = new ProviderBusinessObject())
                {
                    provider = bo.GetByKey(model.ProviderKey);
                    if (provider == null)
                        throw new Exception("Provider not found");
                }



                if (model.ExternalUserIds == null)
                    model.ExternalUserIds = new List<string>();

                if (!String.IsNullOrEmpty(model.ExternalUserId))
                    model.ExternalUserIds.Add(model.ExternalUserId);

                var userInfos = GetUserInfos(model.ApplicationId, model.ExternalUserIds, provider.Id);

                if (!String.IsNullOrEmpty(model.To))
                    userInfos.Add(new Tuple<int?, string>(null, model.To));


                Event ev;
                using (var bo = new EventBusinessObject())
                    ev = bo.GetByKey(model.ApplicationId, model.EventKey);

                if (ev != null)
                {
                    using (var bo = new SubscriptionBusinessObject())
                    {
                        var items = bo.GetList(x => x.EventId == ev.Id);
                        userInfos.AddRange(items.ConvertAll(x => new Tuple<int?, string>(x.UserId, GetToByProvider(x.User, x.ProviderId))));
                    }
                }


                //if (userInfos.Count == 0)
                //    throw new Exception("No Users found to send. Please set: ExternalUserId, ExternalUserIds, To, or EventKey");

                var groupId = Guid.NewGuid();

                foreach (var info in userInfos)
                {
                    var message = new Message
                    {
                        UserId = info.Item1,
                        To = info.Item2,
                        ProviderId = provider.Id,
                        ApplicationId = model.ApplicationId,
                        Subject = model.Subject,
                        Content = model.Content,
                        ProcessDate = model.ProcessDate,
                        GroupId = groupId,
                        Priority = model.Priority ?? MessagePriority.Normal
                    };

                    using (var bo = new MessageBusinessObject())
                    {
                        var id = bo.Create(message);
                        messageIds.Add(id);
                    }
                }
            }
            catch (Exception ex)
            {
                return new APIResult<List<int>>(ex.Message, ex.ToString());
            }

            return new APIResult<List<int>>(messageIds);
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

                using (var bo = new UserBusinessObject())
                {
                    bo.Save(user);
                }

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

                    using (var bo = new UserBusinessObject())
                    {
                        bo.Save(user);
                    }
                }


            }
            catch (Exception ex)
            {
                return new APIResult(ex.Message, ex.ToString());
            }

            return new APIResult();
        }

        [HttpPost]
        public APIResult SaveEvent(SaveEventViewModel model)
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

                using (var bo = new EventBusinessObject())
                {
                    var item = new Event
                    {
                        ApplicationId = model.ApplicationId,
                        Key = model.Key,
                        Description = model.Description
                    };

                    bo.Save(item);
                }
            }
            catch (Exception ex)
            {
                return new APIResult(ex.Message, ex.ToString());
            }

            return new APIResult();
        }

        [HttpPost]
        public APIResult DeleteEvent(DeleteEventModel model)
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

                using (var bo = new EventBusinessObject())
                {
                    bo.Delete(model.ApplicationId, model.EventKey);
                }
            }
            catch (Exception ex)
            {
                return new APIResult(ex.Message, ex.ToString());
            }

            return new APIResult();
        }

        [HttpPost]
        public APIResult SubscribeEvent(SubscribeUserModel model)
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

                int eventId;
                using (var bo = new EventBusinessObject())
                {
                    var ev = bo.GetByKey(model.ApplicationId, model.EventKey);
                    if (ev == null)
                        throw new Exception("Event not found");

                    eventId = ev.Id;
                }

                int userId;
                using (var bo = new UserBusinessObject())
                {
                    var user = bo.GetByExternalUserId(model.ApplicationId, model.ExternalUserId);
                    if (user == null)
                        throw new Exception("User not found");

                    userId = user.Id;
                }

                Provider provider;
                using (var bo = new ProviderBusinessObject())
                {
                    provider = bo.GetByKey(model.ProviderKey);
                    if (provider == null)
                        throw new Exception("Provider not found");
                }


                var subscription = new Subscription
                {
                    EventId = eventId,
                    UserId = userId,
                    ProviderId = provider.Id
                };

                using (var bo = new SubscriptionBusinessObject())
                {
                    bo.Save(subscription);
                }

            }
            catch (Exception ex)
            {
                return new APIResult(ex.Message, ex.ToString());
            }

            return new APIResult();
        }

        [HttpPost]
        public APIResult UnsubscribeEvent(SubscribeUserModel model)
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

                int eventId;
                using (var bo = new EventBusinessObject())
                {
                    var ev = bo.GetByKey(model.ApplicationId, model.EventKey);
                    if (ev == null)
                        throw new Exception("Event not found");

                    eventId = ev.Id;
                }

                int userId;
                using (var bo = new UserBusinessObject())
                {
                    var user = bo.GetByExternalUserId(model.ApplicationId, model.ExternalUserId);
                    if (user == null)
                        throw new Exception("User not found");

                    userId = user.Id;
                }

                Provider provider;
                using (var bo = new ProviderBusinessObject())
                {
                    provider = bo.GetByKey(model.ProviderKey);
                    if (provider == null)
                        throw new Exception("Provider not found");
                }


                var subscription = new Subscription
                {
                    EventId = eventId,
                    UserId = userId,
                    ProviderId = provider.Id
                };

                using (var bo = new SubscriptionBusinessObject())
                {
                    bo.Unsubscribe(subscription);
                }

            }
            catch (Exception ex)
            {
                return new APIResult(ex.Message, ex.ToString());
            }

            return new APIResult();
        }



        [HttpGet]
        public APIResult<List<EventResult>> Events(ApplicationCredentialsModel model)
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

                using (var bo = new EventBusinessObject())
                {
                    var result = bo.GetList().ConvertAll(x => new EventResult
                    {
                        Key = x.Key,
                        Description = x.Description,
                        RegDate = x.RegDate
                    });

                    return new APIResult<List<EventResult>>(result);
                }

            }
            catch (Exception ex)
            {
                return new APIResult<List<EventResult>>(ex.Message, ex.ToString());
            }
        }

        [HttpGet]
        public APIResult<List<SubscriptionResult>> Subscriptions(GetSubscriptionsModel model)
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

                var eventKeys = new List<string>();
                var providerKeys = new List<string>();
                var userKeys = new List<string>();

                if (!String.IsNullOrEmpty(model.EventKeys))
                    eventKeys = model.EventKeys.Split(',').ToList();

                if (!String.IsNullOrEmpty(model.ProviderKeys))
                    providerKeys = model.ProviderKeys.Split(',').ToList();

                if (!String.IsNullOrEmpty(model.ExternalUserIds))
                    userKeys = model.ExternalUserIds.Split(',').ToList();


                using (var bo = new SubscriptionBusinessObject())
                {
                    var items = bo.GetList(model.ApplicationId, eventKeys, providerKeys, userKeys);

                    var result = items.Select(x => new SubscriptionResult
                    {
                        EventKey = x.Event.Key,
                        ProviderKey = x.Provider.Key,
                        ExternalUserId = x.User.ExternalUserId
                    }).ToList();

                    return new APIResult<List<SubscriptionResult>>(result);
                }

            }
            catch (Exception ex)
            {
                return new APIResult<List<SubscriptionResult>>(ex.Message, ex.ToString());
            }
        }

        [HttpGet]
        public APIResult<List<MessageResult>> Messages(GetMessagesModel model)
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
                    var result = bo.GetListWithUserAndProvider(x => ids.Contains(x.Id)).ConvertAll(x=>
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



        List<Tuple<int?, string>> GetUserInfos(int applicationId, List<string> externalUserIds, int providerId)
        {
            var result = new List<Tuple<int?, string>>();

            foreach (var item in externalUserIds)
            {
                using (var bo = new UserBusinessObject())
                {
                    var user = bo.GetByExternalUserId(applicationId, item.Trim());
                    if (user == null)
                        continue;

                    var to = GetToByProvider(user, providerId);
                    if (String.IsNullOrEmpty(to))
                        continue;

                    result.Add(new Tuple<int?, string>(user.Id, to));
                }
            }

            return result;
        }

        string GetToByProvider(User user, int providerId)
        {
            switch (providerId)
            {
                case 1: return user.Email;
                case 2: return user.MobileNumber;
                case 3: return user.ExternalUserId;
                case 4: return user.Email;
                default: return String.Empty;
            }
        }
    }
}
