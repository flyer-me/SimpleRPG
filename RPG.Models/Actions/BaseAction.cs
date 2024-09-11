using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RPG.Models;

namespace RPG.Models.Actions
{
    public abstract class BaseAction
    {
        protected readonly GameItem _itemInUse;
        public event EventHandler<string> OnActionPerformed;
        protected BaseAction(GameItem itemInUse)
        {
            _itemInUse = itemInUse;
        }
        protected void ReportResult(string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
    }
}