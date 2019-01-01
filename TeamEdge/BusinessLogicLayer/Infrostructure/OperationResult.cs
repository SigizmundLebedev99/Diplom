using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace TeamEdge.BusinessLogicLayer.Infrastructure
{
    public class OperationResult<T> : OperationResult
    {
        public OperationResult(bool succeded) : base(succeded) { }

        public T Result { get; set; }

        public override IActionResult GetResult()
        {
            if (Succeded)
                return new OkObjectResult(Result);
            else
                return new BadRequestObjectResult(ErrorMessages);
        }
    }

    public class OperationResult
    {
        public OperationResult(bool succeded)
        {
            Succeded = succeded;
        }

        protected List<ErrorMessage> ErrorMessages { get; set; }
        public bool Succeded { get; set; }

        public void AddErrorMessage(string alias, object message)
        {
            Succeded = false;
            ErrorMessages.Add(new ErrorMessage { Alias = alias, Message = message });
        }

        public void AddErrorMessage(string alias)
        {
            Succeded = false;
            ErrorMessages.Add(new ErrorMessage { Alias = alias });
        }

        public virtual IActionResult GetResult()
        {
            if (Succeded)
                return new OkResult();
            else
                return new BadRequestObjectResult(ErrorMessages);
        }

        public void Plus(OperationResult b)
        {
            if (!b.Succeded)
                this.Succeded = false;
            this.ErrorMessages = this.ErrorMessages.Concat(b.ErrorMessages).ToList();
        }
    }
}
