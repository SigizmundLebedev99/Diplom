using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamEdge.WebLayer
{
    public class GitCmdResult : ActionResult
    {
        private readonly string contentType;
        private readonly string advertiseRefsContent;
        private readonly Action<Stream> executeGitCommand;

        public GitCmdResult(string contentType, Action<Stream> executeGitCommand)
            : this(contentType, executeGitCommand, null)
        {
        }

        public GitCmdResult(string contentType, Action<Stream> executeGitCommand, string advertiseRefsContent)
        {
            this.contentType = contentType;
            this.advertiseRefsContent = advertiseRefsContent;
            this.executeGitCommand = executeGitCommand;
        }

        public override async void ExecuteResult(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            if (advertiseRefsContent != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(advertiseRefsContent);
                await response.Body.WriteAsync(bytes, 0, bytes.Length);
            }

            response.Headers.Add("Expires", "Fri, 01 Jan 1980 00:00:00 GMT");
            response.Headers.Add("Pragma", "no-cache");
            response.Headers.Add("Cache-Control", "no-cache, max-age=0, must-revalidate");

            response.ContentType = contentType;

            executeGitCommand(response.Body);
        }
    }
}
