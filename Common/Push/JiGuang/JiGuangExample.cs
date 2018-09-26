using System;
using System.Collections.Generic;
using System.Linq;
using Jiguang.JPush;
using Jiguang.JPush.Model;

namespace Common.Push.JiGuang
{
    public class JiGuangExample
	{
		private static readonly JPushClient Client = new JPushClient("6e2b074f2352468bc5aa5c40", "237864f629427604c59ffe16");
		
		public static void Push(string messageTitle,string messageContent, List<long> userIds)
		{
			var audience = new Audience
			{
				Alias = userIds.Select(e=>$"prn_{e.ToString()}").Union(userIds.Select(e => $"dev_{e.ToString()}")).ToList()
			};
			PushPayload pushPayload = new PushPayload
			{
				Platform = new List<string> { "android", "ios" },
				Audience = audience,
				Notification = new Notification
				{
					Alert = messageTitle,
					Android = new Android
					{
						Alert = messageTitle,
						Title = "title"
					},
					IOS = new IOS
					{
						Alert = messageTitle,
						Badge = "+1"
					}
				},
				Message = new Message
				{
					Title = messageTitle,
					Content = messageContent
				},
				Options = new Options
				{
					IsApnsProduction = true // 设置 iOS 推送生产环境。不设置默认为开发环境。
				}
			};
			var response = Client.SendPush(pushPayload);
		}


	}
}
